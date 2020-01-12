/*
 *      Simple IPL loader - github.com/ignurof
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FreeroamClient
{
    class LoadIPL : BaseScript
    {
        public LoadIPL()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            // If we don't do this check, the rest of the method will run every time any resource has started.
            if (GetCurrentResourceName() != resourceName) return;

            // Trevors trailer: 1975.552, 3820.538, 33.4483
            RequestIpl("trevorstrailertidy");
            // Union depository: 2.6968, -667.0166, 16.13061
            RequestIpl("finbank");
            // Lesters factory: 716.84, -962.05, 31.59
            RequestIpl("id2_14_during1");
            // Pillbox hospital: 307.1680, -590.807, 43.280
            RequestIpl("rc12b_default");
            // Simeons dealership:
            RequestIpl("shr_int");
            // LifeInvader lobby: -1047.9, -233.0, 39.0
            RequestIpl("facelobby");
            // FIB lobby: 105.4557, -745.4835, 44.7548
            RequestIpl("FIBlobby");
            // Vangelico jewels: -637.20159 - 239.16250 38.1
            RequestIpl("post_hiest_unload");
        }
    }
}
