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
        private static readonly Vector2 FILESELECT_SIZE = new Vector2(400, 100);
        private static readonly Vector2 RESOLUTION_SIZE = new Vector2(30, 30);
        private static readonly Vector2 VOLUME_SIZE = new Vector2(30, 30);
        private static readonly Vector2 KEYBIND_SIZE = new Vector2(150, 50);
        private static readonly Vector2 DIALOG_SIZE = new Vector2(100, 40);

        private static Dictionary<ButtonStyles, Dictionary<int, Animation>> _animations;
        
        static ButtonAnimator()
        {
            _animations = new Dictionary<ButtonStyles, Dictionary<int, Animation>>();

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
            basicDictionary.Add(DISABLED,             new Animation(0, 1, 1, BASIC_SIZE));
            basicDictionary.Add(ENABLED,              new Animation(0, 1, 1, BASIC_SIZE));
            basicDictionary.Add(SELECTED,             new Animation(0, 1, 1, BASIC_SIZE));
            basicDictionary.Add(SELECT,               new Animation(1, 1, 1, BASIC_SIZE));
            basicDictionary.Add(DESELECT,             new Animation(1, 1, 1, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 4, 3, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 4, 3, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 4, 3, BASIC_SIZE));
            basicDictionary.Add(FADE_OUT,             new Animation(4, 4, 3, BASIC_SIZE));
            basicDictionary.Add(FADE_IN,              new Animation(3, 4, 3, BASIC_SIZE));
            #endregion

            #region ExitBasic            
            exitBasicDictionary.Add(DISABLED,             new Animation(0, 1, 1, EXITBASIC_SIZE));
            exitBasicDictionary.Add(ENABLED,              new Animation(0, 1, 1, EXITBASIC_SIZE));
            exitBasicDictionary.Add(SELECTED,             new Animation(0, 1, 1, EXITBASIC_SIZE));
            exitBasicDictionary.Add(SELECT,               new Animation(1, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(DESELECT,             new Animation(1, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(FADE_OUT,             new Animation(3, 0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(FADE_IN,              new Animation(4, 0, 0, EXITBASIC_SIZE));
            #endregion

            #region Title
            titleDictionary.Add(DISABLED,             new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(ENABLED,              new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(SELECTED,             new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(SELECT,               new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(DESELECT,             new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(0, 3, 3, TITLE_SIZE));
            titleDictionary.Add(FADE_OUT,             new Animation(1, 3, 3, TITLE_SIZE));
            titleDictionary.Add(FADE_IN,              new Animation(2, 3, 3, TITLE_SIZE));
            #endregion

            #region SmallBack
            smallBackDictionary.Add(DISABLED,             new Animation(0, 1, 1, SMALLBACK_SIZE));
            smallBackDictionary.Add(ENABLED,              new Animation(0, 1, 1, SMALLBACK_SIZE));
            smallBackDictionary.Add(SELECTED,             new Animation(0, 1, 1, SMALLBACK_SIZE));
            smallBackDictionary.Add(SELECT,               new Animation(1, 3, 3, SMALLBACK_SIZE));
            smallBackDictionary.Add(DESELECT,             new Animation(1, 3, 3, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4, SMALLBACK_SIZE));
            smallBackDictionary.Add(FADE_OUT,             new Animation(3, 4, 3, SMALLBACK_SIZE));
            smallBackDictionary.Add(FADE_IN,              new Animation(4, 4, 3, SMALLBACK_SIZE));
            #endregion

            #region FileSelect
            fileSelectDictionary.Add(DISABLED,             new Animation(0, 1, 1, FILESELECT_SIZE));
            fileSelectDictionary.Add(ENABLED,              new Animation(0, 1, 1, FILESELECT_SIZE));
            fileSelectDictionary.Add(SELECTED,             new Animation(0, 1, 1, FILESELECT_SIZE));
            fileSelectDictionary.Add(SELECT,               new Animation(1, 3, 3, FILESELECT_SIZE));
            fileSelectDictionary.Add(DESELECT,             new Animation(1, 3, 3, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4, FILESELECT_SIZE));
            fileSelectDictionary.Add(FADE_OUT,             new Animation(3, 4, 3, FILESELECT_SIZE));
            fileSelectDictionary.Add(FADE_IN,              new Animation(4, 4, 3, FILESELECT_SIZE));
            #endregion

            #region Resolution
            resolutionDictionary.Add(DISABLED,             new Animation(DISABLED,             1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(ENABLED,              new Animation(ENABLED,              1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(SELECTED,             new Animation(SELECTED,             1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(SELECT,               new Animation(SELECT,               2, 3, RESOLUTION_SIZE));
            resolutionDictionary.Add(DESELECT,             new Animation(DESELECT,             2, 3, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   1, 1, RESOLUTION_SIZE));
            resolutionDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             4, 3, RESOLUTION_SIZE));
            resolutionDictionary.Add(FADE_IN,              new Animation(FADE_IN,              4, 3, RESOLUTION_SIZE));
            #endregion

            #region Volume
            volumeDictionary.Add(DISABLED,             new Animation(0, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(ENABLED,              new Animation(0, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(SELECTED,             new Animation(0, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(SELECT,               new Animation(1, 2, 3, VOLUME_SIZE));
            volumeDictionary.Add(DESELECT,             new Animation(1, 2, 3, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 1, 1, VOLUME_SIZE));
            volumeDictionary.Add(FADE_OUT,             new Animation(3, 4, 3, VOLUME_SIZE));
            volumeDictionary.Add(FADE_IN,              new Animation(4, 4, 3, VOLUME_SIZE));
            #endregion

            #region Keybind
            keybindDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, KEYBIND_SIZE));
            keybindDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, KEYBIND_SIZE));
            #endregion

            #region DialogOne
            dialogDictionary.Add(DISABLED,             new Animation(0, 1, 1, DIALOG_SIZE));
            dialogDictionary.Add(ENABLED,              new Animation(0, 1, 1, DIALOG_SIZE));
            dialogDictionary.Add(SELECTED,             new Animation(0, 1, 1, DIALOG_SIZE));
            dialogDictionary.Add(SELECT,               new Animation(1, 3, 3, DIALOG_SIZE));
            dialogDictionary.Add(DESELECT,             new Animation(1, 3, 3, DIALOG_SIZE));
            dialogDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(2, 2, 4, DIALOG_SIZE));
            dialogDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(2, 2, 4, DIALOG_SIZE));
            dialogDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(2, 2, 4, DIALOG_SIZE));
            dialogDictionary.Add(FADE_OUT,             new Animation(3, 4, 3, DIALOG_SIZE));
            dialogDictionary.Add(FADE_IN,              new Animation(4, 4, 3, DIALOG_SIZE));
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

        public static int ConvertEnumToIndex(ButtonStates state, ButtonStyles style)
        {
            if (style == ButtonStyles.MoveKeybind || style == ButtonStyles.Resolution) // this means that there are extra animations, selected, deselect, highlight_selected, dehighlight_selected
            {

            }

            return 0;
        }
    }
}
