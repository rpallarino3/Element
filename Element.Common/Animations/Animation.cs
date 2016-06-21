using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Element.Common.Animations
{
    public class Animation
    {
        private int _row;
        private int _numImages;
        private int _imageTime;

        public Animation(int row, int numImages, int imageTime)
        {
            _row = row;
            _numImages = numImages;
            _imageTime = imageTime;
        }
        
        public int Row
        {
            get { return _row; }
        }

        public int NumImages
        {
            get { return _numImages; }
        }

        public int ImageTime
        {
            get { return _imageTime; }
        }        
    }
}
