using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;

namespace Element.ResourceManagement.RegionGeneration
{
    public static class RegionLayout
    {
        private static Dictionary<RegionNames, RegionInfo> _regionInfo;
        
        static RegionLayout()
        {
            _regionInfo = new Dictionary<RegionNames, RegionInfo>();

            _regionInfo[RegionNames.Test0] = CreateTest0Info();
            _regionInfo[RegionNames.Test1] = CreateTest1Info();
            _regionInfo[RegionNames.Test2] = CreateTest2Info();
        }

        private static RegionInfo CreateTest0Info()
        {
            var test0Info = new RegionInfo(RegionTheme.TestTheme0);
            test0Info.AdjacentRegions.Add(RegionNames.Test1);
            test0Info.ZoneTileSizes.Add(new Vector2(60, 60));
            test0Info.ZoneLevels.Add(1);
            test0Info.RegionOffsets.Add(new RegionOffset(RegionNames.Test0, RegionNames.Test1, 0, 0, new Vector2(1800, 0)));
            test0Info.Scenery.Add(SceneryNames.Test0Level0);
            return test0Info;
        }

        private static RegionInfo CreateTest1Info()
        {
            var test1Info = new RegionInfo(RegionTheme.TestTheme1);
            test1Info.AdjacentRegions.Add(RegionNames.Test0);
            test1Info.AdjacentRegions.Add(RegionNames.Test2);
            test1Info.ZoneTileSizes.Add(new Vector2(60, 120));
            test1Info.ZoneLevels.Add(2);
            test1Info.RegionOffsets.Add(new RegionOffset(RegionNames.Test1, RegionNames.Test0, 0, 0, new Vector2(-1800, 0)));
            test1Info.RegionOffsets.Add(new RegionOffset(RegionNames.Test1, RegionNames.Test2, 0, 0, new Vector2(1800, 1800)));
            test1Info.Scenery.Add(SceneryNames.Test1Level0Top);
            test1Info.Scenery.Add(SceneryNames.Test1Level0Bottom);
            return test1Info;
        }

        private static RegionInfo CreateTest2Info()
        {
            var test2Info = new RegionInfo(RegionTheme.TestTheme0);
            test2Info.AdjacentRegions.Add(RegionNames.Test1);
            test2Info.ZoneTileSizes.Add(new Vector2(120, 60));
            test2Info.ZoneLevels.Add(0);
            test2Info.RegionOffsets.Add(new RegionOffset(RegionNames.Test2, RegionNames.Test1, 0, 0, new Vector2(-1800, -1800)));
            test2Info.Scenery.Add(SceneryNames.Test2Level0Left);
            test2Info.Scenery.Add(SceneryNames.Test2Level0Right);
            return test2Info;
        }

        public static Dictionary<RegionNames, RegionInfo> RegionInfo
        {
            get { return _regionInfo; }
        }
    }
}
