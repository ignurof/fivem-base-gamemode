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

            // Trevors trailer
            RequestIpl("trevorstrailer");
            // Union depository
            RequestIpl("finbank");
            // Lesters factory
            RequestIpl("id2_14_during1");
        }
    }
}
