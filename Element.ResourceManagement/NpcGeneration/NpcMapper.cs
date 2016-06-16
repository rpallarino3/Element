using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.GameObjects.Npcs;
using Element.ResourceManagement.RegionGeneration;

namespace Element.ResourceManagement.NpcGeneration
{
    public static class NpcMapper
    {
        private static Dictionary<NpcNames, List<RegionNames>> _npcRegions;
        private static Dictionary<NpcNames, NpcTypes> _npcTypes;

        static NpcMapper()
        {
            _npcRegions = new Dictionary<NpcNames, List<RegionNames>>();
            _npcTypes = new Dictionary<NpcNames, NpcTypes>();
            
        }

        public static List<NpcNames> GetCrossRegionNpcsForRegions(List<RegionNames> regions)
        {
            var npcNames = new List<NpcNames>();

            foreach (var region in regions)
            {
                var npcsToAdd = RegionFactory.GetInfoForRegion(region).CrossRegionNpcs;

                foreach (var npc in npcsToAdd)
                {
                    if (!npcNames.Contains(npc))
                        npcNames.Add(npc);
                }
            }

            return npcNames;
        }

        public static List<RegionNames> GetRegionsForCrossRegionNpc(NpcNames npc)
        {
            return _npcRegions[npc];
        }

        public static List<Npc> CreateCrossRegionNpcs(List<NpcNames> npcsToCreate, SaveData data)
        {
            var npcList = new List<Npc>();

            return npcList;
        }

        public static NpcTypes GetTypeForNpc(NpcNames npc)
        {
            return _npcTypes[npc];
        }
    }
}
