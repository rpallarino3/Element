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
        private readonly SceneryNames _name;
        private readonly List<string> _fileNames;
        private readonly Dictionary<int, Animation> _animations;
        private readonly bool _onFloor;

        public SceneryInfo(SceneryNames name, List<string> fileNames, Dictionary<int, Animation> animations, bool onFloor)
        {
            _name = name;
            _fileNames = fileNames;
            _animations = animations;
            _onFloor = onFloor;
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

        public bool OnFloor
        {
            get { return _onFloor; }
        }
    }
}
