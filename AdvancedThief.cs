using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using SDG.Unturned;
using OpenMod.API.Users;
using System.Collections.Generic;

[assembly: PluginMetadata("SS.AdvancedThief", DisplayName = "AdvancedThief")]
namespace AdvancedThief
{
    public class AdvancedThief : OpenModUnturnedPlugin
    {
        private readonly ILogger<AdvancedThief> ro_Logger;

        public AdvancedThief(
            ILogger<AdvancedThief> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ro_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            ro_Logger.LogInformation("Plugin loaded correctly!");
            ro_Logger.LogInformation("If you have any error you can contact the owner in discord: Senior S#9583");
        }

        protected override async UniTask OnUnloadAsync()
        {
            ro_Logger.LogInformation("Plugin unloaded correctly!");
        }
    }
}
