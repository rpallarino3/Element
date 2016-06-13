using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.GameObjects.Npcs;

namespace Element.ResourceManagement.NpcGeneration
{
    public static class NpcMapper
    {
        private static Dictionary<NpcNames, List<RegionNames>> _npcRegions;
        private static Dictionary<RegionNames, List<NpcNames>> _regionNpcs;
        private static Dictionary<NpcNames, NpcTypes> _npcTypes;

        static NpcMapper()
        {
            _npcRegions = new Dictionary<NpcNames, List<RegionNames>>();
            _regionNpcs = new Dictionary<RegionNames, List<NpcNames>>();
            _npcTypes = new Dictionary<NpcNames, NpcTypes>();

            #region TestRegion0

            _regionNpcs[RegionNames.Test0] = new List<NpcNames>();

            #endregion

            #region TestRegion1

            _regionNpcs[RegionNames.Test1] = new List<NpcNames>();

            #endregion

            #region TestRegion2

            _regionNpcs[RegionNames.Test2] = new List<NpcNames>();

            #endregion
        }

        public static List<NpcNames> GetCrossRegionNpcsForRegions(List<RegionNames> regions)
        {
            var npcNames = new List<NpcNames>();

            foreach (var region in regions)
            {
                var npcsToAdd = _regionNpcs[region];

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
