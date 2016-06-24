using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.Menu;

namespace Element.ResourceManagement
{
    public static class MenuResourceManager
    {
        private static IServiceProvider _serviceProvider;
        private static string _rootDirectory;

        private static ContentManager _contentManager;

        private static Texture2D _splashScreen;
        private static Texture2D _titleBackground;
        private static Texture2D _startBackground;
        private static Texture2D _dialogBackgroundSmall;
        private static Texture2D _dialogBackgroundLarge;
        private static Texture2D _fileMenuBackground;
        private static Texture2D _exitMenuBackground;
        private static Texture2D _optionsMenuBackground;
        private static Texture2D _fileImageHighlight;

        private static SpriteFont _menuFont; // make my own font

        private static Dictionary<RegionNames, Texture2D> _filePictures;
        private static Dictionary<ButtonStyles, Texture2D> _buttonTextures;

        static MenuResourceManager()
        {
            _filePictures = new Dictionary<RegionNames, Texture2D>();
            _buttonTextures = new Dictionary<ButtonStyles, Texture2D>();
        }

        public static void PassProviderAndRootDirectory(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;
            _contentManager = new ContentManager(_serviceProvider, _rootDirectory);
        }

        public static void LoadContent()
        {
            _filePictures.Add(RegionNames.None, _contentManager.Load<Texture2D>("Menus/FileImages/NewGame"));
            _filePictures.Add(RegionNames.Test0, _contentManager.Load<Texture2D>("Menus/FileImages/TestRegion0"));
            _filePictures.Add(RegionNames.Test1, _contentManager.Load<Texture2D>("Menus/FileImages/TestRegion1"));
            _filePictures.Add(RegionNames.Test2, _contentManager.Load<Texture2D>("Menus/FileImages/TestRegion2"));

            _buttonTextures.Add(ButtonStyles.Basic, _contentManager.Load<Texture2D>("Menus/Buttons/BasicButton"));
            _buttonTextures.Add(ButtonStyles.Dialog, _contentManager.Load<Texture2D>("Menus/Buttons/DialogOneButton"));
            _buttonTextures.Add(ButtonStyles.ExitBasic, _contentManager.Load<Texture2D>("Menus/Buttons/ExitBasicButton"));
            _buttonTextures.Add(ButtonStyles.FileSelect, _contentManager.Load<Texture2D>("Menus/Buttons/FileSelectButton"));
            _buttonTextures.Add(ButtonStyles.Keybind, _contentManager.Load<Texture2D>("Menus/Buttons/KeybindButton"));
            _buttonTextures.Add(ButtonStyles.Resolution, _contentManager.Load<Texture2D>("Menus/Buttons/ResolutionButton"));
            _buttonTextures.Add(ButtonStyles.SmallBack, _contentManager.Load<Texture2D>("Menus/Buttons/SmallBackButton"));
            _buttonTextures.Add(ButtonStyles.Title, _contentManager.Load<Texture2D>("Menus/Buttons/TitleButton"));
            _buttonTextures.Add(ButtonStyles.Volume, _contentManager.Load<Texture2D>("Menus/Buttons/VolumeButton"));

            _splashScreen = _contentManager.Load<Texture2D>("Menus/SplashScreen");
            _titleBackground = _contentManager.Load<Texture2D>("Menus/TitleBackground");
            _startBackground = _contentManager.Load<Texture2D>("Menus/StartBackground");
            _dialogBackgroundSmall = _contentManager.Load<Texture2D>("Menus/DialogTextOnly");
            _dialogBackgroundLarge = _contentManager.Load<Texture2D>("Menus/DialogButtons");
            _fileMenuBackground = _contentManager.Load<Texture2D>("Menus/FileSelectMenuBackground");
            _exitMenuBackground = _contentManager.Load<Texture2D>("Menus/ExitBackground");
            _optionsMenuBackground = _contentManager.Load<Texture2D>("Menus/OptionsMenuBackground");
            _fileImageHighlight = _contentManager.Load<Texture2D>("Menus/FileImageHighlight");

            _menuFont = _contentManager.Load<SpriteFont>("Menus/MenuFont");
        }

        public static void UnloadContent()
        {
            _contentManager.Unload();
            _contentManager.Dispose();
        }

        public static Texture2D SlashScreen { get { return _splashScreen; } }
        public static Texture2D TitleBackground { get { return _titleBackground; } }
        public static Texture2D StartBackground { get { return _startBackground; } }
        public static Texture2D DialogBackgroundSmall { get { return _dialogBackgroundSmall; } }
        public static Texture2D DialogBackgroundLarge { get { return _dialogBackgroundLarge; } }
        public static Texture2D FileMenuBackground { get { return _fileMenuBackground; } }
        public static Texture2D ExitMenuBackground { get { return _exitMenuBackground; } }
        public static Texture2D OptionsMenuBackground { get { return _optionsMenuBackground; } }
        public static Texture2D FileImageHighlight { get { return _fileImageHighlight; } }
        public static SpriteFont MenuFont { get { return _menuFont; } }
        public static Dictionary<RegionNames, Texture2D> FilePictures { get { return _filePictures; } }
        public static Dictionary<ButtonStyles, Texture2D> ButtonTextures { get { return _buttonTextures; } }
    }
}
