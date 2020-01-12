using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FreeroamClient
{
    class RemoveAIPolice : BaseScript
    {
        public RemoveAIPolice()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            Tick += RemoveWanted;
        }

        int ply = Game.Player.Handle;

        // TODO: Lista ut vad fan async innebär. Vad gör Task också?
        private async Task RemoveWanted()
        {
            if (GetPlayerWantedLevel(ply) > 0)
            {
                ClearPlayerWantedLevel(ply);
            }
        }

        private void OnClientResourceStart(string resourceName)
        {
            // If we don't do this check, the rest of the method will run every time any resource has started.
            if (GetCurrentResourceName() != resourceName) return;

            // Move this section into a seperate file
            /*  DT_PoliceAutomobile = 1,  
                DT_PoliceHelicopter = 2,  
                DT_FireDepartment = 3,  
                DT_SwatAutomobile = 4,  
                DT_AmbulanceDepartment = 5,  
                DT_PoliceRiders = 6,  
                DT_PoliceVehicleRequest = 7,  
                DT_PoliceRoadBlock = 8,  
                DT_PoliceAutomobileWaitPulledOver = 9,  
                DT_PoliceAutomobileWaitCruising = 10,  
                DT_Gangs = 11,  
                DT_SwatHelicopter = 12,  
                DT_PoliceBoat = 13,  
                DT_ArmyVehicle = 14,  
                DT_BikerBackup = 15
            */
            EnableDispatchService(1, false);
            EnableDispatchService(2, false);
            EnableDispatchService(3, false);
            EnableDispatchService(4, false);
            EnableDispatchService(5, false);
            EnableDispatchService(6, false);
            EnableDispatchService(7, false);
            EnableDispatchService(8, false);
            EnableDispatchService(9, false);
            EnableDispatchService(10, false);
            EnableDispatchService(12, false);
            EnableDispatchService(13, false);
            EnableDispatchService(14, false);
            //SetPoliceIgnorePlayer(Game.Player.Handle, true);
        }
    }
}
