using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = Rocket.Core.Logging.Logger;

namespace EnvyPermissions
{
    public class EnvyPermissionsPlugin : RocketPlugin
    {
        public static EnvyPermissionsPlugin Instance { get; set; }
        protected override void Load()
        {   
            Instance = this;
            Logger.Log("<=================================>");
            Logger.Log("Plugin By: Margarita#8172 - EnvyHosting");
            Logger.Log("<=================================>");
        }

        protected override void Unload()
        {
            base.Unload();
        }
    }
}
