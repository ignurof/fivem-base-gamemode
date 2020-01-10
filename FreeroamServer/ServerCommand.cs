using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FreeroamServer
{
    public class ServerCommand : BaseScript
    {
        public ServerCommand()
        {
            EventHandlers["logCmdEvent"] += new Action<Player, string, string>(LogCommand);
        }

        private void LogCommand([FromSource] Player source, string param1, string cmd)
        {
            string name = source.Name;
            // Code that gets executed once the event is triggered goes here.
            // The variable 'source' contains a reference to the player that triggered the event.
            Debug.WriteLine(name + ": " + cmd + " | " + param1);
        }



        // TODO: add server events that can be called by the client. start with /commands
    }
}
