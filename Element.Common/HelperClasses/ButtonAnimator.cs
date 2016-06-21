using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;
using Element.Common.Enumerations.Menu;

namespace Element.Common.HelperClasses
{
    public static class ButtonAnimator
    {
        private static readonly int DISABLED = 0;
        private static readonly int ENABLED = 1;
        private static readonly int SELECTED = 2;
        private static readonly int SELECT = 3;
        private static readonly int DESELECT = 4;
        private static readonly int HIGHLIGHT_ENABLED = 5;
        private static readonly int HIGHLIGHT_DISABLED = 6;
        private static readonly int HIGHLIGHT_SELECTED = 7;
        private static readonly int FADE_OUT = 8;
        private static readonly int FADE_IN = 9;

        private static readonly Vector2 BASIC_SIZE = new Vector2(500, 80);
        private static readonly Vector2 EXITBASIC_SIZE = new Vector2(250, 50);
        private static readonly Vector2 TITLE_SIZE = new Vector2(600, 150);
        private static readonly Vector2 SMALLBACK_SIZE = new Vector2(150, 60);
        private static readonly Vector2 FILESELECT_SIZE = new Vector2(300, 100);
        private static readonly Vector2 RESOLUTION_SIZE = new Vector2(30, 30);
        private static readonly Vector2 VOLUME_SIZE = new Vector2(30, 30);
        private static readonly Vector2 KEYBIND_SIZE = new Vector2(150, 50);
        private static readonly Vector2 DIALOG_SIZE = new Vector2(100, 40);

        private static Dictionary<ButtonStyles, Dictionary<int, Animation>> _animations;
        private static Dictionary<ButtonStyles, Vector2> _buttonSizes;
        
        static ButtonAnimator()
        {
            _animations = new Dictionary<ButtonStyles, Dictionary<int, Animation>>();
            _buttonSizes = new Dictionary<ButtonStyles, Vector2>();

            _buttonSizes.Add(ButtonStyles.Basic, BASIC_SIZE);
            _buttonSizes.Add(ButtonStyles.ExitBasic, EXITBASIC_SIZE);
            _buttonSizes.Add(ButtonStyles.Title, TITLE_SIZE);
            _buttonSizes.Add(ButtonStyles.SmallBack, SMALLBACK_SIZE);
            _buttonSizes.Add(ButtonStyles.FileSelect, FILESELECT_SIZE);
            _buttonSizes.Add(ButtonStyles.Resolution, RESOLUTION_SIZE);
            _buttonSizes.Add(ButtonStyles.Volume, VOLUME_SIZE);
            _buttonSizes.Add(ButtonStyles.Keybind, KEYBIND_SIZE);
            _buttonSizes.Add(ButtonStyles.Dialog, DIALOG_SIZE);

            // can reuse the same animation dictionary multiple times for different button styles? maybe not because of sizes
            var basicDictionary = new Dictionary<int, Animation>();
            var exitBasicDictionary = new Dictionary<int, Animation>();
            var titleDictionary = new Dictionary<int, Animation>();
            var smallBackDictionary = new Dictionary<int, Animation>();
            var fileSelectDictionary = new Dictionary<int, Animation>();
            var resolutionDictionary = new Dictionary<int, Animation>();
            var volumeDictionary = new Dictionary<int, Animation>();
            var keybindDictionary = new Dictionary<int, Animation>();
            var dialogDictionary = new Dictionary<int, Animation>();

            #region Basic
            basicDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            basicDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            basicDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            basicDictionary.Add(SELECT,               new Animation(1, 1, 1));
            basicDictionary.Add(DESELECT,             new Animation(1, 1, 1));
            basicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 4, 3));
            basicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 4, 3));
            basicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 4, 3));
            basicDictionary.Add(FADE_OUT,             new Animation(4, 4, 10));
            basicDictionary.Add(FADE_IN,              new Animation(3, 4, 10));
            #endregion

            #region ExitBasic            
            exitBasicDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            exitBasicDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            exitBasicDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            exitBasicDictionary.Add(SELECT,               new Animation(1, 0, 0));
            exitBasicDictionary.Add(DESELECT,             new Animation(1, 0, 0));
            exitBasicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 0, 0));
            exitBasicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 0, 0));
            exitBasicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 0, 0));
            exitBasicDictionary.Add(FADE_OUT,             new Animation(3, 0, 0));
            exitBasicDictionary.Add(FADE_IN,              new Animation(4, 0, 0));
            #endregion

            #region Title
            titleDictionary.Add(DISABLED,             new Animation(0, 3, 3));
            titleDictionary.Add(ENABLED,              new Animation(0, 3, 3));
            titleDictionary.Add(SELECTED,             new Animation(0, 3, 3));
            titleDictionary.Add(SELECT,               new Animation(0, 3, 3));
            titleDictionary.Add(DESELECT,             new Animation(0, 3, 3));
            titleDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(0, 3, 3));
            titleDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(0, 3, 3));
            titleDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(0, 3, 3));
            titleDictionary.Add(FADE_OUT,             new Animation(1, 3, 10));
            titleDictionary.Add(FADE_IN,              new Animation(2, 3, 10));
            #endregion

            #region SmallBack
            smallBackDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            smallBackDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            smallBackDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            smallBackDictionary.Add(SELECT,               new Animation(1, 3, 3));
            smallBackDictionary.Add(DESELECT,             new Animation(1, 3, 3));
            smallBackDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4));
            smallBackDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4));
            smallBackDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4));
            smallBackDictionary.Add(FADE_OUT,             new Animation(3, 4, 10));
            smallBackDictionary.Add(FADE_IN,              new Animation(4, 4, 10));
            #endregion

            #region FileSelect
            fileSelectDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            fileSelectDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            fileSelectDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            fileSelectDictionary.Add(SELECT,               new Animation(1, 3, 3));
            fileSelectDictionary.Add(DESELECT,             new Animation(1, 3, 3));
            fileSelectDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4));
            fileSelectDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4));
            fileSelectDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4));
            fileSelectDictionary.Add(FADE_OUT,             new Animation(3, 4, 10));
            fileSelectDictionary.Add(FADE_IN,              new Animation(4, 4, 10));
            #endregion

            #region Resolution
            resolutionDictionary.Add(DISABLED,             new Animation(DISABLED,             1, 1));
            resolutionDictionary.Add(ENABLED,              new Animation(ENABLED,              1, 1));
            resolutionDictionary.Add(SELECTED,             new Animation(SELECTED,             1, 1));
            resolutionDictionary.Add(SELECT,               new Animation(SELECT,               2, 3));
            resolutionDictionary.Add(DESELECT,             new Animation(DESELECT,             2, 3));
            resolutionDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    1, 1));
            resolutionDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   1, 1));
            resolutionDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   1, 1));
            resolutionDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             4, 10));
            resolutionDictionary.Add(FADE_IN,              new Animation(FADE_IN,              4, 10));
            #endregion

            #region Volume
            volumeDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            volumeDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            volumeDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            volumeDictionary.Add(SELECT,               new Animation(1, 2, 3));
            volumeDictionary.Add(DESELECT,             new Animation(1, 2, 3));
            volumeDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 1, 1));
            volumeDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 1, 1));
            volumeDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 1, 1));
            volumeDictionary.Add(FADE_OUT,             new Animation(3, 4, 10));
            volumeDictionary.Add(FADE_IN,              new Animation(4, 4, 10));
            #endregion

            #region Keybind
            keybindDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            keybindDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            keybindDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            keybindDictionary.Add(SELECT,               new Animation(1, 2, 3));
            keybindDictionary.Add(DESELECT,             new Animation(1, 2, 3));
            keybindDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 1, 1));
            keybindDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 1, 1));
            keybindDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 1, 1));
            keybindDictionary.Add(FADE_OUT,             new Animation(3, 4, 10));
            keybindDictionary.Add(FADE_IN,              new Animation(4, 4, 10));
            #endregion

            #region DialogOne
            dialogDictionary.Add(DISABLED,             new Animation(0, 1, 1));
            dialogDictionary.Add(ENABLED,              new Animation(0, 1, 1));
            dialogDictionary.Add(SELECTED,             new Animation(0, 1, 1));
            dialogDictionary.Add(SELECT,               new Animation(1, 3, 3));
            dialogDictionary.Add(DESELECT,             new Animation(1, 3, 3));
            dialogDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4));
            dialogDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4));
            dialogDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4));
            dialogDictionary.Add(FADE_OUT,             new Animation(3, 4, 10));
            dialogDictionary.Add(FADE_IN,              new Animation(4, 4, 10));
            #endregion

            _animations.Add(ButtonStyles.Basic, basicDictionary);
            _animations.Add(ButtonStyles.ExitBasic, exitBasicDictionary);
            _animations.Add(ButtonStyles.Title, titleDictionary);
            _animations.Add(ButtonStyles.SmallBack, smallBackDictionary);
            _animations.Add(ButtonStyles.FileSelect, fileSelectDictionary);
            _animations.Add(ButtonStyles.Resolution, resolutionDictionary);
            _animations.Add(ButtonStyles.Volume, volumeDictionary);
            _animations.Add(ButtonStyles.Keybind, keybindDictionary);
            _animations.Add(ButtonStyles.Dialog, dialogDictionary);
        }

        public static Dictionary<int, Animation> GetAnimationsFromStyle(ButtonStyles style)
        {
            return _animations[style];
        }

        public static Vector2 GetImageSizeFromStyle(ButtonStyles style)
        {
            return _buttonSizes[style];
        }
    }
}
