using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace GoBackToYourBiome
{
    [HarmonyPatch(typeof(SpawnSystem), "FindBaseSpawnPoint")]
    public static class NightRestrictionsPatch2
    {
        public static bool Prefix(SpawnSystem.SpawnData spawn, List<Player> allPlayers, Vector3 spawnCenter, Player targetPlayer)
        {
            //Skip if any of these conditions
            if (spawn == null ||
                !spawn.m_spawnAtNight ||
                spawn.m_name.Equals("ODIN") || //Black Odin animation must go on
                Player.m_localPlayer == null ||
                !EnvMan.IsNight() ||
                string.IsNullOrEmpty(spawn.m_requiredGlobalKey) ||
                ZoneSystem.instance == null ||
                !ZoneSystem.instance.GetGlobalKey(spawn.m_requiredGlobalKey))
                return true;

            //Biome blocks
            if (ConfigurationFile.nightSpawnsFromAnyBiome.Value.Contains(spawn.m_requiredGlobalKey))
            {
                Logger.Log($"[{spawn.m_name} | id: {spawn.m_prefab.name}] listed as blocked by {spawn.m_requiredGlobalKey} key at nights.");
                return false;
            }
            string key = spawn.m_requiredGlobalKey;
            bool blocked = 
                (!ConfigurationFile.nightSpawnsFromBossesBiome1Meadows.Value && key == "defeated_eikthyr") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome2BlackForest.Value && key == "defeated_gdking") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome3Swamp.Value && key == "defeated_bonemass") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome4Mountain.Value && key == "defeated_dragon") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome5Plains.Value && key == "defeated_goblinking") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome6Mistlands.Value && key == "defeated_queen") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome7Ashlands.Value && key == "defeated_fader") ||
                (!ConfigurationFile.nightSpawnsFromBossesBiome8DeepNorth.Value && key == "defeated_frozenking_p3");
            if (blocked)
            {
                Logger.Log($"[{spawn.m_name} | id: {spawn.m_prefab.name}] from key {key} blocked at nights.");
                return false;
            }

            //General blocks
            if (ConfigurationFile.nightSpawnsEnemies.Value.Contains(spawn.m_prefab.name))
            {
                Logger.Log($"[{spawn.m_name} | id: {spawn.m_prefab.name}] id listed as blocked anywhere at nights.");
                return false;
            }
            Heightmap.Biome currentBiome = Player.m_localPlayer.GetCurrentBiome();
            switch (currentBiome)
            {
                case Heightmap.Biome.Meadows:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome1Meadows.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.BlackForest:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome2BlackForest.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.Swamp:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome3Swamp.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.Mountain:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome4Mountain.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.Plains:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome5Plains.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.Mistlands:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome6Mistlands.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.AshLands:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome7Ashlands.Value.Contains(spawn.m_prefab.name);
                    break;
                case Heightmap.Biome.DeepNorth:
                    blocked = ConfigurationFile.nightSpawnsInGeneralBiome8DeepNorth.Value.Contains(spawn.m_prefab.name);
                    break;
                default:
                    blocked = getNightSpawnsInGeneralCustomBiomes().Contains(spawn.m_prefab.name);
                    break;
            }
            if (blocked)
            {
                Logger.Log($"[{spawn.m_name} | id: {spawn.m_prefab.name}] id listed as blocked at {currentBiome} at nights.");
                return false;
            }
            
            Logger.Log($"[{spawn.m_name} | id: {spawn.m_prefab.name}] allowed.");
            return true;
        }
        
        private static List<string> getNightSpawnsInGeneralCustomBiomes()
        {
            List<string> list = new List<string>();
            Logger.Log("getNightSpawnsInGeneralCustomBiomes");
            if (string.IsNullOrEmpty(ConfigurationFile.nightSpawnsInGeneralCustomBiomes.Value) || Minimap.instance == null)
                return list;
            
            string currentCustomBiomeName = Minimap.instance.transform.Find("small/small_biome")?.GetComponent<TextMeshProUGUI>()?.text;
            if (currentCustomBiomeName == null) return list;

            try
            {
                foreach (var customBiomeInfo in ConfigurationFile.nightSpawnsInGeneralCustomBiomes.Value
                             .Split(';').ToList())
                {
                    string[] customBiomeInfoParts = customBiomeInfo.Split(':');
                    string customBiomeName = customBiomeInfoParts[0];
                    if (customBiomeName.Equals(currentCustomBiomeName))
                        list.AddRange(customBiomeInfoParts[1].Split(',').ToList());
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error while parsing the nightSpawnsInGeneralCustomBiomes value. Skipping... "+e.Message);
            }

            return list;
        }
    }
}