using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
        private readonly Vector2 _imageSize;
        private readonly Vector2 _imageOffset;

        public SceneryInfo(SceneryNames name, List<string> fileNames, Dictionary<int, Animation> animations, bool onFloor, Vector2 imageSize) :
            this(name, fileNames, animations, onFloor, imageSize, new Vector2(0, 0))
        { }

        public SceneryInfo(SceneryNames name, List<string> fileNames, Dictionary<int, Animation> animations, bool onFloor, Vector2 imageSize, Vector2 imageOffset)
        {
            _name = name;
            _fileNames = fileNames;
            _animations = animations;
            _onFloor = onFloor;
            _imageSize = imageSize;
            _imageOffset = imageOffset;
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

        public Vector2 ImageSize
        {
            get { return _imageSize; }
        }

        public Vector2 ImageOffset
        {
            get { return _imageOffset; }
        }
    }
}
