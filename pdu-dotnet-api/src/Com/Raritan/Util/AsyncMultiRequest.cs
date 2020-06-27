// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Util {

    /**
     * Tracks multiple {@link AsyncTask}'s tasks running in parallel.
     * 
     * It will go to success state, when all tasks tracked by this instance have
     * succeeded. It will immediately go to failure state, when any of the tasks has
     * failed.
     * 
     * Note that this is only _tracks_ activities, but does not manage or provide
     * the implementations of them, as opposed to {@link AsyncCommand}.
     */
    public class AsyncMultiRequest : AsyncRequest {
        private List<AsyncRequest> m_requests = new List<AsyncRequest>();
        private List<AsyncRequest> m_failed = new List<AsyncRequest>();
        private List<AsyncRequest> m_succeeded = new List<AsyncRequest>();
        int tasksPending = 0;

        public List<AsyncRequest> FailedRequests {
            get { return m_failed; }
        }

        /**
         * Constructor.
         * 
         * @param label  a (unique) string to associate with this task - for debugging
         */
        public AsyncMultiRequest(string label) : base(label) {
        }

        /**
         * Add an {@link AsyncTask} to the list of tracked tasks.
         */
        public void Add(AsyncRequest request) {
            Debug.Assert(IsNew);

            m_requests.Add(request);
            tasksPending++;

            request.Success += data => {
                                   m_succeeded.Add(request);
                                   if (--tasksPending == 0 && HasStarted) AllDone();
                               };
            request.Failure += exc => {
                                   m_failed.Add(request);
                                   if (--tasksPending == 0 && HasStarted) AllDone();
                               };
        }

        /**
         * Inform this instance that all tasks have been added and tracking can start.
         *
         * Note about running success/failure commands (SFCs):
         * We cannot call SFCs before this method, because we don't know about the
         * state of any tasks added in the future.  However, once this method is
         * called, we imply (and enforce) that no further tasks will be added.
         * Hence, SFCs can then be called once all tasks have finished.
         *
         * The above handles the following corner-cases gracefully:
         * 1. Multiple already-finished tasks can be added w/o calling SFCs too early.
         * 2. It's possible to add no tasks at all and still have SFCs called.
         */
        public override AsyncRequest Started() {
            base.Started();
            if (tasksPending == 0) AllDone();
            return this;
        }

        public override AsyncRequest Succeeded(object data) {
            Debug.Assert(false);  // not for public use
            return this;
        }
        public override AsyncRequest Failed(Exception exc) {
            Debug.Assert(false);  // not for public use
            return this;
        }

        private void AllDone() {
            if (m_failed.Count == 0) {
                // this includes the "no tasks added" case
                base.Succeeded(m_succeeded);
            } else {
                base.Failed(new AsyncMultiRequestFailure(m_failed));
            }
        }
    
    }

}
