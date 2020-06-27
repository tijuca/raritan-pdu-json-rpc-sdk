import time, socket

ZEROCONF_TIMEOUT_DEFAULT = 4

class ZeroconfDiscoveryListener:
    def __init__(self):
        self.found_pdus = {}
        self.pdu_prefixes = ["PX2", "PX3", "SRC", "BCM", "BCM2", "PX3TS", "PMC", "PMMC", "PXE", "SCH", "SRC", "LP", "PXO", "PXC", "EMX"]

    def remove_service(self, zeroconf, service_type, name):
        pass

    def add_service(self, zeroconf, service_type, name):
        try:
            entry = zeroconf.get_service_info(service_type, name)
            if not any([entry.name.startswith(pfx) for pfx in self.pdu_prefixes]):
                return
            self.found_pdus[entry.addresses[0]] = { "fw": entry.properties[b'rr_fw'], "port": entry.port }
        except (KeyboardInterrupt, ImportError, NameError):
            pass

def discover(timeout = ZEROCONF_TIMEOUT_DEFAULT):
    try:
        from zeroconf import ServiceBrowser, Zeroconf
        zeroconf = Zeroconf()
        listener = ZeroconfDiscoveryListener()
        browser_old_name = ServiceBrowser(zeroconf, "_raritan_rpcs._tcp.local.", listener)
        browser_new_name = ServiceBrowser(zeroconf, "_raritan-rpcs._tcp.local.", listener)
        print("Zeroconf discovery started")
        time.sleep(timeout)
        browser_old_name.cancel()
        browser_new_name.cancel()
        zeroconf.close()
        pdus = []
        print("Found %i PDUs in the local subnet:" % len(listener.found_pdus))
        for k, v in listener.found_pdus.items():
            ip = str(socket.inet_ntoa(k))
            port = v.get("port")
            fw_version = str(v.get("fw"), "utf-8")
            print("%s -- %s" % (ip, fw_version))
            pdus.append({ "ip": ip, "port": port, "fw": fw_version })
        return pdus
    except ImportError:
        print("Please install the zeroconf python module (pip3 install zeroconf)")
        raise
