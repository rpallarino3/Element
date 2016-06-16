using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Animations;
using Element.Common.Enumerations.Environment;

namespace Element.ResourceManagement.Scenery
{
    public class SceneryInfo
    {
        public readonly SceneryNames _name;
        public readonly List<string> _fileNames;
        public readonly Dictionary<int, Animation> _animations;

        public SceneryInfo(SceneryNames name, List<string> fileNames, Dictionary<int, Animation> animations)
        {
            _name = name;
            _fileNames = fileNames;
            _animations = animations;
        }

        public SceneryNames Name
        {
            get { return _name; }
        }

        public List<string> FileNames
        {
            get { return _fileNames; }
        }

        public Dictionary<int, Animation> Animations
        {
            get { return _animations; }
        }
    }
}
