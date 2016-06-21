using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Element.Common.Animations
{
    public class Animator
    {
        private Dictionary<int, Animation> _animations;
        private Animation _currentAnimation;

        private bool _animationFinished;
        private int _animtionCounter;
        private int _frameCounter;

        private Vector2 _imageSize;
        private Vector2 _drawOffset;

        public Animator(Dictionary<int, Animation> animations, int startingAnimation, Vector2 imageSize) : this(animations, startingAnimation, imageSize, new Vector2(0, 0)) { }

        public Animator(Dictionary<int, Animation> animations, int startingAnimation, Vector2 imageSize, Vector2 drawOffset)
        {
            _animations = animations;
            _currentAnimation = _animations[startingAnimation];
            _imageSize = imageSize;
            _drawOffset = drawOffset;
        }

        public void AdvanceAnimation()
        {
            if (_animationFinished) // maybe shouldn't put this here to be safe?
            {
                return;
            }

            if (_frameCounter == _currentAnimation.ImageTime - 1)
            {
                if (_animtionCounter == _currentAnimation.NumImages - 1)
                {
                    _animationFinished = true;
                }
                else
                {
                    _animtionCounter++;
                    _frameCounter = 0;
                }
            }
            else
            {
                _frameCounter++;
            }
        }

        public void AdvanceAnimationReplay()
        {
            if (_animationFinished) // yes we should leave this here, functions as a replay of the current animation
            {
                _animtionCounter = 0;
                _frameCounter = 0;
                _animationFinished = _currentAnimation.NumImages == 1 ? true : false;
            }

            if (_frameCounter == _currentAnimation.ImageTime - 1)
            {
                if (_animtionCounter == _currentAnimation.NumImages - 1)
                {
                    _animationFinished = true;
                }
                else
                {
                    _animtionCounter++;
                    _frameCounter = 0;
                }
            }
            else
            {
                _frameCounter++;
            }
        }

        public void SetNewAnimation(int animation)
        {
            _currentAnimation = _animations[animation];
            _animtionCounter = 0;
            _frameCounter = 0;
            _animationFinished = _currentAnimation.NumImages == 1 ? true : false;
        }

        public void SetAnimationIfNotCurrent(int animation)
        {
            if (_currentAnimation.Row != animation)
                SetNewAnimation(animation);
        }

        public Dictionary<int, Animation> Animations
        {
            get { return _animations; }
        }

        public Animation CurrentAnimation
        {
            get { return _currentAnimation; }
        }

        public int AnimationCounter
        {
            get { return _animtionCounter; }
        }

        public bool AnimationFinished
        {
            get { return _animationFinished; }
        }

        public Vector2 ImageSize
        {
            get { return _imageSize; }
        }

        public Vector2 DrawOffset
        {
            get { return _drawOffset; }
        }
    }
}
