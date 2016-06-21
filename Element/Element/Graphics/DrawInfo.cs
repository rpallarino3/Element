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
                if (_level < other.Level)
                    return -1;
                else if (_level > other.Level)
                    return 1;
                else
                    return -1;
            }
            else if (!_onFloor && other.OnFloor)
            {
                if (_level < other.Level)
                    return -1;
                else if (_level > other.Level)
                    return 1;
                else
                    return 1;
            }
            else
            {
                if (_drawLocation.Y < other.DrawLocation.Y)
                    return -1;
                else if (_drawLocation.Y > other.DrawLocation.Y)
                    return 1;
                else
                {
                    if (_level < other.Level)
                        return -1;
                    else if (_level > other.Level)
                        return 1;
                    else
                    {
                        // i guess this means they could be on the same level at the same y but next to each other
                        return 0;
                    }
                }
            }
        }

        public bool OnFloor { get { return _onFloor; } }
        public Texture2D Texture {  get { return _texture; } }
        public Vector2 DrawLocation { get { return _drawLocation; } }
        public Rectangle DrawRectangle { get { return _drawRectangle; } }
        public int Level { get { return _level; } }
    }
}
