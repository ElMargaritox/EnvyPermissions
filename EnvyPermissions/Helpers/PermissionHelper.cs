using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvyPermissions.Helpers
{
    public static class PermissionHelper
    {

        private static readonly List<string> errors = new List<string> {":", ">", "<", "/", "|", "´´", "[", "]", "#"};

        public static bool ExistGroup(string groupName)
        {
            var group = R.Permissions.GetGroup(groupName);
            if (group == null) return false; else return true;
        }

        public static string HasPermissionOnGroup(string groupName, string permission)
        {
            try
            {
                var group = R.Permissions.GetGroup(groupName);

                var _permission = group.Permissions.Find(x => x.Name == permission);
                if (_permission == null) return string.Format("El grupo {0} no tiene el permiso {1}", groupName, permission);
                return string.Format("El grupo {0} si tiene el permiso {1}", groupName, permission);
            }
            catch
            {
                return "No exíste el grupo o esta mal especificado el nombre o permiso";
            }
           
        }

        public static void RemoveGroup(IRocketPlayer caller, string groupName)
        {
            if (!ExistGroup(groupName))
            {
                UnturnedChat.Say(caller, "El grupo no existe");
                return;

            }


            R.Permissions.DeleteGroup(groupName);
            R.Permissions.Reload();


        }
        public static void CreateGroup(IRocketPlayer caller, string groupName)
        {
            if (!ExistGroup(groupName))
            {
                if(caller is ConsolePlayer)
                {
                    UnturnedChat.Say(caller, "Escribe los permisos a continuacion...");

                    

                    TaskDispatcher.QueueOnMainThread(() =>
                    {
                        List<Permission> permissions = new List<Permission>();
                        bool salir = true;
                        while (salir)
                        {

                            Console.Clear();
                            Console.WriteLine(string.Format("Grupo a crear: {0}", groupName));
                            Console.WriteLine("Lista de permisos actuales:");

                            foreach (var item in permissions)
                            {
                                Console.WriteLine(string.Format("Permiso: {0} - Cooldown {1}", item.Name, item.Cooldown));
                            }



                            Console.WriteLine(" ");
                            Console.Write("Escribe el "); Escribir("permiso ", ConsoleColor.Yellow); Console.Write("dentro de la consola para agregarlo \n");
                            Console.Write("Escribe '"); Escribir("guardar", ConsoleColor.Cyan); Console.Write("' para terminar con los permisos del grupo \n");

                            string opc = Console.ReadLine();
                            if (opc.ToLower() == "guardar") break;

                            bool permissionnoagregado = false;
                            foreach (var item in errors)
                            {
                                if (opc.Contains(item))
                                {
                                    permissionnoagregado = true;
                                }
                            }

                            if (permissionnoagregado)
                            {

                            }
                            else
                            {
                                Permission permission = new Permission(opc, 1);
                                permissions.Add(permission);
                            }
                            
                        }

                        Console.Clear();
                        Console.Write("Escribe el color: ");
                        string color = Console.ReadLine();
                        Console.WriteLine(" ");
                        Console.Write("Escribe la prioridad (1-100): ");
                        short.TryParse(Console.ReadLine(), out short prioridad);

                        
                        RocketPermissionsGroup rocketPermissionsGroup = new RocketPermissionsGroup(groupName, groupName, "default", new List<string>(), permissions, color, prioridad);


                        Console.WriteLine(" ");
                        Console.Write("Escribe el prefix: ");
                        string prefix = Console.ReadLine();
                        Console.WriteLine(" ");
                        Console.Write("Escribe el suffix: ");
                        string suffix = Console.ReadLine();

                        rocketPermissionsGroup.Prefix = prefix;
                        rocketPermissionsGroup.Suffix = suffix;

                        R.Permissions.AddGroup(rocketPermissionsGroup);
                        R.Permissions.SaveGroup(rocketPermissionsGroup);
                        R.Permissions.Reload();
                        UnturnedChat.Say(caller, "Grupo creado correctamente");
                    });
                }


            }
            else
            {
                UnturnedChat.Say(caller, "el grupo ya existe");
            }

        }


        private static void Escribir(string xd, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(xd);
            Console.ResetColor();
        }
    }
}
