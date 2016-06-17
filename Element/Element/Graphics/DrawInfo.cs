using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Element.Graphics
{
    public class DrawInfo : IComparable<DrawInfo>
    {
        private bool _onFloor;
        private Texture2D _texture;
        private Vector2 _drawLocation;
        private Rectangle _drawRectangle;
        private int _level;

        public DrawInfo(bool onFloor, Texture2D texture, Vector2 drawLocation, Rectangle drawRectangle, int level)
        {
            _onFloor = onFloor;
            _texture = texture;
            _drawLocation = drawLocation;
            _drawRectangle = drawRectangle;
            _level = level;
        }

        public int CompareTo(DrawInfo other)
        {
            if (_onFloor && !other.OnFloor)
            {
                // this is a floor object
                // the other is not, this should go below?
            }
            else if (!_onFloor && other.OnFloor)
            {
                // the other is a floor object
                // this is not, this should go on top?
            }
            else
            {
                // do normal comparison
            }

            return 0;
        }

        public bool OnFloor { get { return _onFloor; } }
        public Texture2D Texture {  get { return _texture; } }
        public Vector2 DrawLocation { get { return _drawLocation; } }
        public Rectangle DrawRectangle { get { return _drawRectangle; } }
        public int Level { get { return _level; } }
    }
}
