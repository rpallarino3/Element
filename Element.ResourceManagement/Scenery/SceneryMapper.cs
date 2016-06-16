using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Animations;
using Element.Common.Enumerations.Environment;
using Element.Common.GameObjects.Scenery;
using Element.ResourceManagement.RegionGeneration;

namespace Element.ResourceManagement.Scenery
{
    public static class SceneryMapper
    {
        public static readonly string TEST0_LEVEL0 = "Scenery/Regions/Test0/Level0";
        public static readonly string TEST1_LEVEL0_TOP = "Scenery/Regions/Test1/Level0Top";
        public static readonly string TEST1_LEVEL0_BOTTOM = "Scenery/Regions/Test1/Level0Bottom";
        public static readonly string TEST2_LEVEL0_LEFT = "scenery/Regions/Test2/Level0Left";
        public static readonly string TEST2_LEVEL0_RIGHT = "scenery/Regions/Test2/Level0Right";

        private static Dictionary<SceneryNames, SceneryInfo> _sceneryInfo;

        static SceneryMapper()
        {
            _sceneryInfo = new Dictionary<SceneryNames, SceneryInfo>();

            // should make these all private methods
            #region Test0

            _sceneryInfo[SceneryNames.Test0Level0] = new SceneryInfo(SceneryNames.Test0Level0, new List<string>() { "Scenery/Regions/Test0/Level0" },
                new Dictionary<int, Animation>() { { 0, new Animation(0, 1, 1, new Vector2(1800, 1800)) } });
            
            #endregion

            #region Test1

            _sceneryInfo[SceneryNames.Test1Level0Top] = new SceneryInfo(SceneryNames.Test1Level0Top, new List<string>() { "Scenery/Regions/Test1/Level0Top" },
                new Dictionary<int, Animation>() { { 0, new Animation(0, 1, 1, new Vector2(1800, 1800)) } });
            _sceneryInfo[SceneryNames.Test1Level0Bottom] = new SceneryInfo(SceneryNames.Test1Level0Bottom, new List<string>() { "Scenery/Regions/Test1/Level0Bottom" },
                new Dictionary<int, Animation>() { { 0, new Animation(0, 1, 1, new Vector2(1800, 1800)) } });
            
            #endregion

            #region Test2

            _sceneryInfo[SceneryNames.Test2Level0Left] = new SceneryInfo(SceneryNames.Test2Level0Left, new List<string>() { "Scenery/Regions/Test2/Level0Left" },
                new Dictionary<int, Animation>() { { 0, new Animation(0, 1, 1, new Vector2(1800, 1800)) } });
            _sceneryInfo[SceneryNames.Test2Level0Right] = new SceneryInfo(SceneryNames.Test2Level0Right, new List<string>() { "Scenery/Regions/Test2/Level0Right" },
                new Dictionary<int, Animation>() { { 0, new Animation(0, 1, 1, new Vector2(1800, 1800)) } });
            
            #endregion
        }

        public static SceneryObject CreateSceneryObject(SceneryNames name, Vector2 location, int level)
        {
            return new SceneryObject(name, location, level, new Animator(_sceneryInfo[name].Animations, 0));
        }

        public static Dictionary<SceneryNames, List<Texture2D>> LoadSceneryTextures(ContentManager contentManager, RegionNames region)
        {
            var textureDictionary = new Dictionary<SceneryNames, List<Texture2D>>();

            var sceneryToLoad = RegionFactory.GetInfoForRegion(region).Scenery;

            foreach (var item in sceneryToLoad)
            {
                var list = new List<Texture2D>();
                var fileNames = _sceneryInfo[item].FileNames;

                foreach (var name in fileNames)
                    list.Add(contentManager.Load<Texture2D>(name));

                textureDictionary[item] = list;
            }

            return textureDictionary;
        }
    }
}
