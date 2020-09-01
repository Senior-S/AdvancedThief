using Rocket.Core.Plugins;
using Rocket.Core.Logging;

namespace AdvancedThief
{
    public class AdvancedThief : RocketPlugin
    {
        protected override void Load()
        {
            Logger.Log("[AdvancedThief] Plugin loaded correctly!");
            Logger.Log("[AdvancedThief] If you have any error you can contact the owner in discord: Senior S#9583");
        }

        protected override void Unload()
        {
            Logger.Log("[AdvancedThief] Plugin unloaded correctly!");
        }
    }
}
