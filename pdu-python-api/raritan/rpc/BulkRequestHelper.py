# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2015 Raritan Inc. All rights reserved.

import raritan.rpc
import raritan.rpc.bulkrpc

class BulkRequestHelper:
    """Helper class to collect multiple JSON-RPC requests and execute them as a
    single bulk request.

    Example:
      helper = raritan.rpc.BulkRequestHelper(agent)
      helper.add_request(snmp_proxy.getV3EngineId)
      helper.add_request(snmp_proxy.setConfiguration, cfg)
      responses = helper.perform_bulk()"""

    def __init__(self, agent, raise_subreq_failure = False):
        """Creates a new BulkRequestHelper instance.

        raise_subreq_failure specifies the default value for the perform_bulk()
        argument of the same name. See perform_bulk() for details."""
        self.requests = []
        self.agent = agent
        self.raise_subreq_failure = raise_subreq_failure

    def add_request(self, method, *args):
        """Adds an RPC method call to the request queue.

        Note: The method arguments must be passed without parentheses!"""
        r = raritan.rpc.bulkrpc.BulkRequest.Request(method.parent.target,
                { "jsonrpc": "2.0",
                  "method": method.name,
                  "params": method.encode(*args),
                  "id": raritan.rpc.Agent.id }
        )
        r._id = raritan.rpc.Agent.id
        r._decode = method.decode
        raritan.rpc.Agent.id += 1
        self.requests.append(r)

    def clear(self):
        """Empties the request and response list."""
        self.requests = []

    def perform_bulk(self, raise_subreq_failure = None):
        """Performs all queued requests and returns a list of responses.

        The response list contains one entry per queued request. Each entry can be
        one of the following:
         - None if the called method returns void and has no out parameters.
         - A single value if the method has exactly one return type or out
           parameter.
         - A tuple if the method has more than one return type/out parameter.
         - An Exception object if the request has failed and raise_subreq_failure
           is False.

        If any request failed and raise_subreq_error is True the method will throw
        the Exception object of the first failed request. The complete list of
        results can still be accessed in the "responses" field of the instance.

        Note: The request queue is not automatically cleared. Call clear() before
        reusing the BulkRequestHelper instance."""

        def decode_response(request, rawresult):
            try:
                if request._id != rawresult.json['id']:
                    raise raritan.rpc.HttpException("JSON-RPC response ID does not match")
                resp_json = rawresult.json
                if rawresult.statcode != 200:
                    raise raritan.rpc.HttpException("HTTP Error %d\nResponse:\n%s" % (rawresult.statcode, resp_json))
                if resp_json["jsonrpc"] != "2.0":
                    raise raritan.rpc.HttpException("Malformed JSON-RPC response")
                if 'error' in resp_json:
                    try:
                        code = resp_json["error"]["code"]
                        msg = resp_json["error"]["message"]
                    except KeyError:
                        raise raritan.rpc.JsonRpcSyntaxException("JSON RPC returned malformed error: %s" % resp_json)
                    raise raritan.rpc.JsonRpcErrorException("JSON RPC returned error: code = %d, msg = %s" % (code, msg))

                return request._decode(resp_json['result'], self.agent)
            except Exception as e:
                return e

        bulk = raritan.rpc.bulkrpc.BulkRequest("/bulk", self.agent)
        self.rawresults = bulk.performBulk(self.requests)
        self.responses = [ decode_response(request, rawresult)
                           for (request, rawresult) in zip(self.requests, self.rawresults) ]

        if raise_subreq_failure == None:
            raise_subreq_failure = self.raise_subreq_failure
        if raise_subreq_failure:
            for result in self.responses:
                if isinstance(result, Exception):
                    raise result

        return self.responses
