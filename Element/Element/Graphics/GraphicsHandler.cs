using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;
using Element.ResourceManagement;
using Element.Logic;

namespace Element.Graphics
{
    public class GraphicsHandler
    {
        private MenuGraphicsHandler _menuGraphicsHandler;

        public GraphicsHandler()
        {
            _menuGraphicsHandler = new MenuGraphicsHandler();
        }

        public void Draw(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            resourceManager.CheckLoad();
            resourceManager.CheckUnload();

            var state = GameStateHelper.CurrentState;

            if (state == GameStates.Start)
                _menuGraphicsHandler.DrawStart(sb, logic, resourceManager);
            else if (state == GameStates.StartMenu)
                _menuGraphicsHandler.DrawStartMenu(sb, logic, resourceManager);
            else if (state == GameStates.ExitMenu)
            {
                // draw roam here too
                _menuGraphicsHandler.DrawExitMenu(sb, logic, resourceManager);
            }
        }

        public static void DrawCenteredText(SpriteBatch sb, SpriteFont font, string text, Vector2 drawLocation, Vector2 areaSize, Vector2 screenRatio, Color color)
        {
            var split = text.Split(Environment.NewLine.ToCharArray());
            var totalHeight = font.MeasureString(text).Y;
            var topPadding = (areaSize.Y - totalHeight) / 2;

            var start = drawLocation.Y + topPadding;

            foreach (var item in split)
            {
                var measurement = font.MeasureString(item);
                var xLocation = drawLocation.X + (areaSize.X - measurement.X) / 2;

                sb.DrawString(
                    font, 
                    item, 
                    new Vector2(xLocation, start) * screenRatio, 
                    color,
                    GameConstants.DEFAULT_ROTATION,
                    GameConstants.DEFAULT_IMAGE_ORIGIN, 
                    screenRatio, 
                    SpriteEffects.None, 
                    GameConstants.DEFAULT_LAYER);

                start += measurement.Y;
            }
        }
    }
}
