using BepInEx;
using BepInEx.Configuration;
using ServerSync;

namespace GoBackToYourBiome
{
    internal class ConfigurationFile
    {
        private static ConfigEntry<bool> _serverConfigLocked;
        public static ConfigEntry<bool> debug;

        public static ConfigEntry<string> nightSpawnsFromAnyBiome;
        public static ConfigEntry<string> nightSpawnsEnemies;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome1Meadows;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome2BlackForest;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome3Swamp;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome4Mountain;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome5Plains;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome6Mistlands;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome7Ashlands;
        public static ConfigEntry<bool> nightSpawnsFromBossesBiome8DeepNorth;

        public static ConfigEntry<string> nightSpawnsInGeneralCustomBiomes;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome1Meadows;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome2BlackForest;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome3Swamp;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome4Mountain;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome5Plains;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome6Mistlands;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome7Ashlands;
        public static ConfigEntry<string> nightSpawnsInGeneralBiome8DeepNorth;


        private static ConfigFile configFile;

        private static readonly ConfigSync ConfigSync = new ConfigSync(GoBackToYourBiome.GUID)
        {
            DisplayName = GoBackToYourBiome.NAME,
            CurrentVersion = GoBackToYourBiome.VERSION,
            MinimumRequiredVersion = GoBackToYourBiome.VERSION
        };

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                configFile = plugin.Config;

                _serverConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
                _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

                debug = config("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)", false);

                nightSpawnsFromBossesBiome1Meadows     = config("2 - Night Spawns From Boss Victories", "From Biome 1 - Meadows", false, "If disabled, greydwarfs sent by Eikthyr will stop spawning at nights!");
                nightSpawnsFromBossesBiome2BlackForest = config("2 - Night Spawns From Boss Victories", "From Biome 2 - Black Forest", false, "If disabled, tougher greydwarfs sent by The Elder from black forest will stop spawning at nights!");
                nightSpawnsFromBossesBiome3Swamp       = config("2 - Night Spawns From Boss Victories", "From Biome 3 - Swamp", false, "If disabled, skeletons sent by Bonemass will stop spawning at nights!");
                nightSpawnsFromBossesBiome4Mountain    = config("2 - Night Spawns From Boss Victories", "From Biome 4 - Mountain", false, "If disabled, enemies sent by Moder will stop spawning at nights!");
                nightSpawnsFromBossesBiome5Plains      = config("2 - Night Spawns From Boss Victories", "From Biome 5 - Plains", false, "If disabled, fulings sent by Yagluth spawning at nights!");
                nightSpawnsFromBossesBiome6Mistlands   = config("2 - Night Spawns From Boss Victories", "From Biome 6 - Mistlands", false, "If disabled, seekers sent by The Queen will stop spawning at nights!");
                nightSpawnsFromBossesBiome7Ashlands    = config("2 - Night Spawns From Boss Victories", "From Biome 7 - Ashlands", false, "If disabled, charred enemies sent by Fader will stop spawning at nights!");
                nightSpawnsFromBossesBiome8DeepNorth   = config("2 - Night Spawns From Boss Victories", "From Biome 8 - Deep North", false, "If disabled, deep north enemies sent by the final boss will stop spawning at nights!");
                nightSpawnsFromAnyBiome                = config("2 - Night Spawns From Boss Victories", "Global keys whose enemies will be blocked in Any Biome", "", "Comma-separated list of global keys whose associated night spawns will be blocked");
                
                nightSpawnsInGeneralCustomBiomes      = config("3 - Night Spawns In General", "Block at Custom Biomes",          "", "Enemies prefab ids to be blocked at custom biomes at nights. Use format biomeName1:id1,id2;biomeName2:id3,id4;etc");
                nightSpawnsInGeneralBiome1Meadows     = config("3 - Night Spawns In General", "Block at Biome 1 - Meadows",      "", "Enemies prefab ids to be blocked at meadows at nights");
                nightSpawnsInGeneralBiome2BlackForest = config("3 - Night Spawns In General", "Block at Biome 2 - Black Forest", "", "Enemies prefab ids to be blocked at black forest at nights");
                nightSpawnsInGeneralBiome3Swamp       = config("3 - Night Spawns In General", "Block at Biome 3 - Swamp",        "", "Enemies prefab ids to be blocked at swamp at nights");
                nightSpawnsInGeneralBiome4Mountain    = config("3 - Night Spawns In General", "Block at Biome 4 - Mountain",     "", "Enemies prefab ids to be blocked at mountains at nights");
                nightSpawnsInGeneralBiome5Plains      = config("3 - Night Spawns In General", "Block at Biome 5 - Plains",       "", "Enemies prefab ids to be blocked at plains at nights");
                nightSpawnsInGeneralBiome6Mistlands   = config("3 - Night Spawns In General", "Block at Biome 6 - Mistlands",    "", "Enemies prefab ids to be blocked at mistlands at nights");
                nightSpawnsInGeneralBiome7Ashlands    = config("3 - Night Spawns In General", "Block at Biome 7 - Ashlands",     "", "Enemies prefab ids to be blocked at ashlands at nights");
                nightSpawnsInGeneralBiome8DeepNorth   = config("3 - Night Spawns In General", "Block at Biome 8 - Deep North",   "", "Enemies prefab ids to be blocked at deep north at nights");
                nightSpawnsEnemies                    = config("3 - Night Spawns In General", "Enemies spawned to block in Any Biome", "", "Comma-separated list of prefab ids whose enemies will not spawn at nights anywhere");
            }
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new ConfigDescription(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = configFile.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
    }
}
