﻿using System;
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
    public static class GraphicsHandler
    {
        public static void Draw(SpriteBatch sb)
        {
            ResourceManager.CheckLoad();
            ResourceManager.CheckUnload();

            var state = GameStateHelper.CurrentState;

            if (state == GameStates.Start)
                MenuGraphicsHandler.DrawStart(sb);
            else if (state == GameStates.StartMenu)
                MenuGraphicsHandler.DrawStartMenu(sb);
            else if (state == GameStates.Roam)
                RoamGraphicsHandler.DrawRoam(sb);
            else if (state == GameStates.ExitMenu)
            {
                RoamGraphicsHandler.DrawRoam(sb);
                MenuGraphicsHandler.DrawExitMenu(sb);
            }
        }

        public static void DrawBlackScreen(SpriteBatch sb)
        {
            var screenRatio = DataHelper.GetScreenRatioFromResolution();
            // draw a black rectangle
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
