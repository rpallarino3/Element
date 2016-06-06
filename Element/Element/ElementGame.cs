using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Element.Graphics;
using Element.Input;
using Element.Logic;
using Element.ResourceManagement;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;
using Element.Common.Menus;

namespace Element
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ElementGame : Microsoft.Xna.Framework.Game
    {
        private LogicHandler _logicHandler;
        private InputHandler _inputHandler;
        private GraphicsHandler _graphicsHandler;
        private ResourceManager _resourceManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public ElementGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _resourceManager = new ResourceManager(Content.ServiceProvider, Content.RootDirectory);
            _resourceManager.LoadFilesAndUpdatePreferenceData();
            ChangeResolution(new ResolutionChangeEventArgs(DataHelper.PreferenceData.Resolution));

            _inputHandler = new InputHandler(_resourceManager);

            _logicHandler = new LogicHandler(_resourceManager, _inputHandler);
            _logicHandler.StartAndExitMenuLogicHandler.ResolutionChanged += ChangeResolution;
            _logicHandler.StartAndExitMenuLogicHandler.BeginExit += ExitGame;

            _graphicsHandler = new GraphicsHandler();

            base.Initialize();
        }

        private void ChangeResolution(MenuPageEventArgs e)
        {
            var args = e as ResolutionChangeEventArgs;

            if (e == null)
                return;

            if (args.Resolution == Resolutions.r960x540)
            {
                if (graphics.PreferredBackBufferWidth != 960)
                    graphics.PreferredBackBufferWidth = 960;

                if (graphics.PreferredBackBufferHeight != 540)
                    graphics.PreferredBackBufferHeight = 540;
            }
            else if (args.Resolution == Resolutions.r1280x720)
            {
                if (graphics.PreferredBackBufferWidth != 1280)
                    graphics.PreferredBackBufferWidth = 1280;

                if (graphics.PreferredBackBufferHeight != 720)
                    graphics.PreferredBackBufferHeight = 720;
            }
            else if (args.Resolution == Resolutions.r1600x900)
            {
                if (graphics.PreferredBackBufferWidth != 1600)
                    graphics.PreferredBackBufferWidth = 1600;

                if (graphics.PreferredBackBufferHeight != 900)
                    graphics.PreferredBackBufferHeight = 900;
            }
            else if (args.Resolution == Resolutions.r1920x1080)
            {
                if (graphics.PreferredBackBufferWidth != 1920)
                    graphics.PreferredBackBufferWidth = 1920;

                if (graphics.PreferredBackBufferHeight != 1080)
                    graphics.PreferredBackBufferHeight = 1080;
            }

            graphics.ApplyChanges();
        }

        private void ExitGame(MenuPageEventArgs e)
        {
            var args = e as ExitGameEventArgs;

            if (args == null)
                return;

            _resourceManager.Shutdown();
            this.Exit();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _resourceManager.LoadStaticContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _inputHandler.UpdateInputs(GamePad.GetState(PlayerIndex.One), Keyboard.GetState());
            _logicHandler.UpdateGameLogic();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            _graphicsHandler.Draw(spriteBatch, _logicHandler, _resourceManager);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
