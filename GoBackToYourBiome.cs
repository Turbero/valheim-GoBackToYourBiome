using BepInEx;
using HarmonyLib;

namespace GoBackToYourBiome
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInIncompatibility("coemt.GlobalKeyFix")]
    public class GoBackToYourBiome : BaseUnityPlugin
    {
        public const string GUID = "Turbero.GoBackToYourBiome";
        public const string NAME = "Go Back To Your Biome";
        public const string VERSION = "1.0.0";

        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            ConfigurationFile.LoadConfig(this);
            harmony.PatchAll();
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
