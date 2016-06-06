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
    public class MenuResourceManager
    {
        private IServiceProvider _serviceProvider;
        private string _rootDirectory;

        private ContentManager _contentManager;

        private Texture2D _splashScreen;
        private Texture2D _titleBackground;
        private Texture2D _startBackground;
        private Texture2D _dialogBackgroundSmall;
        private Texture2D _dialogBackgroundLarge;
        private Texture2D _fileMenuBackground;
        private Texture2D _exitMenuBackground;
        private Texture2D _optionsMenuBackground;
        private Texture2D _fileImageHighlight;

        private SpriteFont _menuFont; // make my own font

        private Dictionary<RegionNames, Texture2D> _filePictures;
        private Dictionary<ButtonStyles, Texture2D> _buttonTextures;

        public MenuResourceManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;

            _contentManager = new ContentManager(_serviceProvider, _rootDirectory);

            _filePictures = new Dictionary<RegionNames, Texture2D>();
            _buttonTextures = new Dictionary<ButtonStyles, Texture2D>();
        }

        public void LoadContent()
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

        public void UnloadContent()
        {
            _contentManager.Unload();
        }

        public Texture2D SlashScreen { get { return _splashScreen; } }
        public Texture2D TitleBackground { get { return _titleBackground; } }
        public Texture2D StartBackground { get { return _startBackground; } }
        public Texture2D DialogBackgroundSmall { get { return _dialogBackgroundSmall; } }
        public Texture2D DialogBackgroundLarge { get { return _dialogBackgroundLarge; } }
        public Texture2D FileMenuBackground { get { return _fileMenuBackground; } }
        public Texture2D ExitMenuBackground { get { return _exitMenuBackground; } }
        public Texture2D OptionsMenuBackground { get { return _optionsMenuBackground; } }
        public Texture2D FileImageHighlight { get { return _fileImageHighlight; } }
        public SpriteFont MenuFont { get { return _menuFont; } }
        public Dictionary<RegionNames, Texture2D> FilePictures { get { return _filePictures; } }
        public Dictionary<ButtonStyles, Texture2D> ButtonTextures { get { return _buttonTextures; } }
    }
}
