using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

/*
 * 
 * RegisterCommand("", new Action<int, List<object>, string>((source, args, raw) =>
            {

            }), false);


    This is how to create a command
*/

namespace FreeroamClient
{
    public class ClientCommand : BaseScript
    {
        public ClientCommand()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            // Vad fan är detta, ett event?
            Tick += OnTick; 
        }

        bool isOn = false;

        // TODO: Lista ut vad fan async innebär. Vad gör Task också?
        private async Task OnTick()
        {
            if (isOn)
            {
                DisplayHelpText("Test");
            }
        }
        
        private void DisplayHelpText(string str)
        {
            SetTextComponentFormat("STRING");
            AddTextComponentString(str);
            DisplayHelpTextFromStringLabel(0, false, true, -1);
        }

        private void OnClientResourceStart(string resourceName)
        {
            // If we don't do this check, the rest of the method will run every time any resource has started.
            if (GetCurrentResourceName() != resourceName) return;

            // load interiors missing from the world. TODO: add more interiors
            RequestIpl("trevorstrailer");

            // Debugging only
            RegisterCommand("ui", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!isOn)
                {
                    isOn = true;
                }
                else
                {
                    isOn = false;
                }
            }), false);

            RegisterCommand("v", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                // account for the argument not being passed
                var model = "adder";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }

                // check if the model actually exists
                // assumes the directive `using static CitizenFX.Core.Native.API;`
                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { $"{model} does not exist." }
                    });
                    return;
                }

                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                // set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                // tell the player
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 200, 133, 0 },
                    args = new[] { $"Spawned a new ^*{model}!" }
                });

                string name = GetPlayerName(source);
                string text = "spawned a ";

                TriggerServerEvent("logCmdEvent", text + vehicle.DisplayName, "/v");
            }), false);

            RegisterCommand("wep", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if(args.Count > 0)
                {
                    switch (args[0].ToString())
                    {
                        case "pistol":
                            Game.PlayerPed.Weapons.Give(WeaponHash.Pistol, 100, false, false);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = new[] { 233, 133, 0 },
                                args = new[] { "Recieved pistol with 100 ammo.." }
                            });
                            break;
                        case "uzi":
                            Game.PlayerPed.Weapons.Give(WeaponHash.MicroSMG, 100, false, false);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = new[] { 233, 133, 0 },
                                args = new[] { "Recieved uzi with 100 ammo.." }
                            });
                            break;
                        case "shotgun":
                            Game.PlayerPed.Weapons.Give(WeaponHash.PumpShotgun, 100, false, false);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = new[] { 233, 133, 0 },
                                args = new[] { "Recieved shotgun with 100 ammo.." }
                            });
                            break;
                        case "sniper":
                            Game.PlayerPed.Weapons.Give(WeaponHash.SniperRifle, 100, false, false);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = new[] { 233, 133, 0 },
                                args = new[] { "Recieved sniper with 100 ammo.." }
                            });
                            break;
                    }

                    return;
                }

                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "Missing an argument!" }
                });
                TriggerServerEvent("logCmdEvent", "gave weapons", "/wep");
            }), false);

            RegisterCommand("kill", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Game.PlayerPed.Kill();

                string name = GetPlayerName(source);

                string text = "suicided.";

                TriggerServerEvent("logCmdEvent", text, "/kill");
            }), false);

            RegisterCommand("skin", new Action<int, List<object>, string>(async(source, args, raw) =>
            {
                if(args.Count > 0)
                {
                    switch (args[0].ToString())
                    {
                        case "cat":
                            await Game.Player.ChangeModel(0x573201B8);
                            break;
                        case "nude":
                            await Game.Player.ChangeModel(0x5442C66B);
                            break;
                    }
                    var ped = Game.PlayerPed.Model;
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 233, 133, 0 },
                        args = new[] { $"Changing model to.. {ped}" }
                    });

                    return;
                }
                else // If we don't have any args, use this
                {
                    await Game.Player.ChangeModel(PedHash.Ballasog);
                }
                TriggerServerEvent("logCmdEvent", "changed model", "/skin");
            }), false);

            RegisterCommand("name", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var name = GetPlayerName(source);

                TriggerEvent("chat:addMessage", new
                {
                    // No color argument = white colored text
                    args = new[] { $"Your name is: {name}" }
                });
                TriggerServerEvent("logCmdEvent", "retrieved own name", "/name");
            }), false);

            RegisterCommand("co", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Vector3 playerPos = Game.PlayerPed.Position;

                string text = playerPos.ToString();

                // Print out coordinates
                TriggerEvent("chat:addMessage", new
                {
                    // No color argument = white colored text
                    args = new[] { $"{playerPos}" }
                });

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/co");
            }), false);

            RegisterCommand("away", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if(GetPlayerWantedLevel(source) > 0)
                {
                    ClearPlayerWantedLevel(source);
                }
                else
                {
                    return;
                }

                string text = "wanted level cleared.";

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/away");
            }), false);

            RegisterCommand("vname", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!Game.PlayerPed.IsInVehicle())
                    return; // Exit the method if player is not in a vehicle

                var vehicle = Game.PlayerPed.CurrentVehicle;

                var model = vehicle.DisplayName;

                TriggerEvent("chat:addMessage", new
                {
                    args = new[] { $"Current vehicle: {model}" }
                });

                string text = "checked vehicled name";

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/vname");
            }), false);

            RegisterCommand("rep", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!Game.PlayerPed.IsInVehicle())
                    return; // Exit the method if player is not in a vehicle

                var vehicle = Game.PlayerPed.CurrentVehicle;

                var bodyHP = GetVehicleBodyHealth(vehicle.Handle);
                var engineHP = GetVehicleEngineHealth(vehicle.Handle);

                if(bodyHP < 1000f || engineHP < 1000f)
                {
                    SetVehicleFixed(vehicle.Handle);
                }
                else
                {
                    return;
                }
                
                TriggerEvent("chat:addMessage", new
                {
                    args = new[] { "Repaired vehicle." }
                });

                string text = "repaired vehicle";

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/rep");
            }), false);

            RegisterCommand("heal", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var maxhp = GetEntityMaxHealth(Game.PlayerPed.Handle);
                var current = GetEntityHealth(Game.PlayerPed.Handle);

                if(current < maxhp)
                {
                    SetEntityHealth(Game.PlayerPed.Handle, maxhp);
                }
                else
                {
                    return;
                }

                string text = "healed player";

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/heal");
            }), false);

            RegisterCommand("speed", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!Game.PlayerPed.IsInVehicle())
                    return; // Exit the method if player is not in a vehicle

                var vehicle = Game.PlayerPed.CurrentVehicle;
                
                ModifyVehicleTopSpeed(vehicle.Handle, 100f);
                ForceVehicleEngineAudio(vehicle.Handle, "sultanrs");
                
                string text = "speed up vehicle";

                // this can be found in ServerCommand.cs
                TriggerServerEvent("logCmdEvent", text, "/speed");
            }), false);
        }
    }
}
