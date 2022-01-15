using EnvyPermissions.Helpers;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyPermissions
{
    internal class CommandEnvyPermissions : IRocketCommand

    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "envypermissions";

        public string Help => "manager permissiosn to server";

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string> { "enp", "ep", "envyp" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
          
            
            if(command.Length == 0) { UnturnedChat.Say("Usa /ep <Create|Erase|Check|Add|Remove|View|List>"); return; }

            switch (command[0].ToLower())
            {
                case "check":
                    if (command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep check <player>"); return; }

                    UnturnedPlayer target3 = UnturnedPlayer.FromName(command[1]);
                    if (target3 == null) { UnturnedChat.Say(caller, "No se encontro al jugador"); return; }

                    var groups = R.Permissions.GetGroups(target3, true);
                    string _groups = string.Empty;
                    groups.ForEach(delegate (RocketPermissionsGroup group2)
                    {
                        _groups += group2.Id + ", ";
                    });
                    UnturnedChat.Say(caller, "Esta en los grupos:" + _groups);
                    break;
                case "add":
                    if(command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep add <player> <groupName>"); return; }

                    UnturnedPlayer target = UnturnedPlayer.FromName(command[1]);
                    if(target == null) { UnturnedChat.Say(caller, "No se encontro al jugador"); return; }

                    R.Permissions.AddPlayerToGroup(command[2], target);
                    UnturnedChat.Say(caller, "Has agregado a la persona al grupo correctamente");
                    break;

                case "remove":
                    if (command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep remove <player> <groupName>"); return; }

                    UnturnedPlayer target2 = UnturnedPlayer.FromName(command[1]);
                    if (target2 == null) { UnturnedChat.Say(caller, "No se encontro al jugador"); return; }

                    R.Permissions.RemovePlayerFromGroup(command[2], target2);
                    UnturnedChat.Say(caller, "Has removido a la persona del grupo correctamente");
                    break;
                // GROUP CONTROLLER
                case "list":
                    // On Maintenanece
                    break;
                case "create":
                    if(command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep create <groupName>"); return; }
                    PermissionHelper.CreateGroup(caller, command[1].ToLower());
                    break;
                case "erase":
                    if (command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep erase <groupName>"); return; }
                    PermissionHelper.RemoveGroup(caller, command[1].ToLower());
                    break;
                case "view":
                    if (command.Length == 1) { UnturnedChat.Say(caller, "Usa /ep view <groupName> <members|permissions>"); return; }
                    var group = R.Permissions.GetGroup(command[1]);


                    switch (command[2].ToLower())
                    {
                        case "members":
                            UnturnedChat.Say(caller, "---------------------------------");
                            group.Members.ForEach(delegate (string member)
                            {
                                UnturnedChat.Say(caller, member);
                            });
                            UnturnedChat.Say(caller, "---------------------------------");
                            UnturnedChat.Say(caller, "En total hay: " + group.Members.Count);
                            break;
                        case "permissions":
                            UnturnedChat.Say(caller, "---------------------------------");
                            group.Permissions.ForEach(delegate (Permission permission)
                            {
                                UnturnedChat.Say(caller, string.Format("Permiso: {0} - Cooldown: {1}", permission.Name, permission.Cooldown));
                            });
                            UnturnedChat.Say(caller, "---------------------------------");
                            UnturnedChat.Say(caller, "En total hay: " + group.Permissions.Count);
                            break;
                        default:
                            UnturnedChat.Say(caller, "members|permissions");
                            break;
                    }

                    break;
                default:
                    UnturnedChat.Say("Usa /ep <Create|Erase|Check|Add|Remove|View>");
                    break;
            }






        }
    }
}
