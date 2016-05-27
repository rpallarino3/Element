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

        private static readonly Vector2 BASIC_SIZE = new Vector2(0, 0);
        private static readonly Vector2 EXITBASIC_SIZE = new Vector2(0, 0);
        private static readonly Vector2 TITLE_SIZE = new Vector2(0, 0);
        private static readonly Vector2 SMALLBACK_SIZE = new Vector2(0, 0);
        private static readonly Vector2 FILESELECT_SIZE = new Vector2(0, 0);
        private static readonly Vector2 RESOLUTION_SIZE = new Vector2(0, 0);
        private static readonly Vector2 VOLUME_SIZE = new Vector2(0, 0);
        private static readonly Vector2 MOVEKEYBIND_SIZE = new Vector2(0, 0);
        private static readonly Vector2 KEYBIND_SIZE = new Vector2(0, 0);
        private static readonly Vector2 DIALOGONE_SIZE = new Vector2(0, 0);
        private static readonly Vector2 DIALOGTWO_SIZE = new Vector2(0, 0);

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
            var moveKeybindDictionary = new Dictionary<int, Animation>();
            var keybindDictionary = new Dictionary<int, Animation>();
            var dialogOneDictionary = new Dictionary<int, Animation>();
            var dialogTwoDictionary = new Dictionary<int, Animation>();

            #region Basic
            basicDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, BASIC_SIZE));
            basicDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, BASIC_SIZE));
            basicDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, BASIC_SIZE));
            basicDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, BASIC_SIZE));
            basicDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, BASIC_SIZE));
            basicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, BASIC_SIZE));
            basicDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, BASIC_SIZE));
            basicDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, BASIC_SIZE));
            #endregion

            #region ExitBasic            
            exitBasicDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, EXITBASIC_SIZE));
            exitBasicDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, EXITBASIC_SIZE));
            #endregion

            #region Title
            titleDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, TITLE_SIZE));
            titleDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, TITLE_SIZE));
            titleDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, TITLE_SIZE));
            titleDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, TITLE_SIZE));
            titleDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, TITLE_SIZE));
            titleDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, TITLE_SIZE));
            titleDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, TITLE_SIZE));
            titleDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, TITLE_SIZE));
            #endregion

            #region SmallBack
            smallBackDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, SMALLBACK_SIZE));
            smallBackDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, SMALLBACK_SIZE));
            #endregion

            #region FileSelect
            fileSelectDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, FILESELECT_SIZE));
            fileSelectDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, FILESELECT_SIZE));
            #endregion

            #region Resolution
            resolutionDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, RESOLUTION_SIZE));
            resolutionDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, RESOLUTION_SIZE));
            #endregion

            #region Volume
            volumeDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, VOLUME_SIZE));
            volumeDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, VOLUME_SIZE));
            volumeDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, VOLUME_SIZE));
            volumeDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, VOLUME_SIZE));
            volumeDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, VOLUME_SIZE));
            volumeDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, VOLUME_SIZE));
            volumeDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, VOLUME_SIZE));
            volumeDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, VOLUME_SIZE));
            #endregion

            #region MoveKeybind
            moveKeybindDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, MOVEKEYBIND_SIZE));
            moveKeybindDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, MOVEKEYBIND_SIZE));
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
            dialogOneDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, DIALOGONE_SIZE));
            dialogOneDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, DIALOGONE_SIZE));
            #endregion

            #region DialogTwo
            dialogTwoDictionary.Add(DISABLED,             new Animation(DISABLED,             0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(ENABLED,              new Animation(ENABLED,              0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(SELECTED,             new Animation(SELECTED,             0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(SELECT,               new Animation(SELECT,               0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(DESELECT,             new Animation(DESELECT,             0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(HIGHLIGHT_ENABLED,    new Animation(HIGHLIGHT_ENABLED,    0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(HIGHLIGHT_DISABLED,   new Animation(HIGHLIGHT_DISABLED,   0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(HIGHLIGHT_SELECTED,   new Animation(HIGHLIGHT_SELECTED,   0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(FADE_OUT,             new Animation(FADE_OUT,             0, 0, DIALOGTWO_SIZE));
            dialogTwoDictionary.Add(FADE_IN,              new Animation(FADE_IN,              0, 0, DIALOGTWO_SIZE));
            #endregion

            _animations.Add(ButtonStyles.Basic, basicDictionary);
            _animations.Add(ButtonStyles.ExitBasic, exitBasicDictionary);
            _animations.Add(ButtonStyles.Title, titleDictionary);
            _animations.Add(ButtonStyles.SmallBack, smallBackDictionary);
            _animations.Add(ButtonStyles.FileSelect, fileSelectDictionary);
            _animations.Add(ButtonStyles.Resolution, resolutionDictionary);
            _animations.Add(ButtonStyles.Volume, volumeDictionary);
            _animations.Add(ButtonStyles.MoveKeybind, moveKeybindDictionary);
            _animations.Add(ButtonStyles.Keybind, keybindDictionary);
            _animations.Add(ButtonStyles.DialogOne, dialogOneDictionary);
            _animations.Add(ButtonStyles.DialogTwo, dialogTwoDictionary);
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
