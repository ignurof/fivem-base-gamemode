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

            // load interiors missing from the world. TODO: add more interiors
            RequestIpl("trevorstrailer");
            RequestIpl("finbank");
        }
    }
}
