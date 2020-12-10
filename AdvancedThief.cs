using Rocket.Core.Plugins;
using Rocket.Core.Logging;

namespace AdvancedThief
{
    public class AdvancedThief : RocketPlugin
    {
        protected override void Load()
        {
            Logger.Log(" Plugin loaded correctly!");
            Logger.Log(" More plugins: www.dvtserver.xyz");
        }

        protected override void Unload()
        {
            Logger.Log(" Plugin unloaded correctly!");
        }
    }
}
