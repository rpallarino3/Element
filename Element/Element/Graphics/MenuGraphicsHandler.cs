using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.HelperClasses;
using Element.Common.Menus.MenuPages;
using Element.ResourceManagement;
using Element.Logic;

namespace Element.Graphics
{
    public static class MenuGraphicsHandler
    {
        private static readonly Vector2 SPLASH_SCREEN_DRAW_LOCATION = new Vector2(0, 0);
        private static readonly Vector2 TITLE_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private static readonly Vector2 START_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private static readonly Vector2 FILE_SELECT_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private static readonly Vector2 OPTIONS_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private static readonly Vector2 EXIT_BACKGROUND_DRAW_LOCATION = new Vector2(495, 185);

        private static readonly Vector2 FILE_IMAGE_0_LOCATION = new Vector2(110, 60);
        private static readonly Vector2 FILE_IMAGE_1_LOCATION = new Vector2(490, 60);
        private static readonly Vector2 FILE_IMAGE_2_LOCATION = new Vector2(870, 60);
        private static readonly Vector2 FILE_IMAGE_SIZE = new Vector2(300, 400);

        private static readonly Vector2 DIALOG_DRAW_LOCATION = new Vector2(525, 300);
        private static readonly Vector2 DIALOG_SIZE_SMALL = new Vector2(230, 60);
        private static readonly Vector2 DIALOG_SIZE_LARGE = new Vector2(230, 120);

        public static void DrawStart(SpriteBatch sb)
        {

        }

        public static void DrawStartMenu(SpriteBatch sb)
        {
            var screenRatio = DataHelper.GetScreenRatioFromResolution();

            DrawTitleBackground(sb, screenRatio);

            var activeMenuPage = StartAndExitMenuLogicHandler.ActiveMenuPage;

            if (activeMenuPage.Name == MenuPageNames.Start)
                DrawStartBackground(sb, screenRatio);
            else if (activeMenuPage.Name == MenuPageNames.FileSelect)
            {
                DrawFileSelectBackground(sb, screenRatio);
                DrawFileSelectImages(sb, screenRatio, ((FileSelectMenuPage)activeMenuPage).GetFileSelectedIndex());
            }
            else if (activeMenuPage.Name == MenuPageNames.Options)
                DrawOptionsBackground(sb, screenRatio);

            foreach (var button in activeMenuPage.Buttons)
            {
                var texture = MenuResourceManager.ButtonTextures[button.Style];
                var imageSize = button.Animator.ImageSize;
                var rectangle = new Rectangle((int)(button.Animator.AnimationCounter * imageSize.X),
                    (int)(button.Animator.CurrentAnimation.Row * imageSize.Y),
                    (int)imageSize.X, (int)imageSize.Y); // this could be different if button is too big?

                sb.Draw(
                    texture,
                    button.Location * screenRatio,
                    rectangle,
                    LogicHandler.DrawColor,
                    GameConstants.DEFAULT_ROTATION,
                    GameConstants.DEFAULT_IMAGE_ORIGIN,
                    screenRatio,
                    SpriteEffects.None,
                    GameConstants.DEFAULT_LAYER);

                GraphicsHandler.DrawCenteredText(sb, MenuResourceManager.MenuFont, button.Text, button.Location, imageSize, screenRatio, LogicHandler.DrawColor);
            }

            if (activeMenuPage.DialogOpen)
            {
                if (activeMenuPage.CurrentDialog.Buttons.Count == 0)
                    DrawSmallDialog(sb, screenRatio);
                else
                    DrawLargeDialog(sb, screenRatio);

                foreach (var button in activeMenuPage.CurrentDialog.Buttons)
                {
                    var texture = MenuResourceManager.ButtonTextures[button.Style];
                    var imageSize = button.Animator.ImageSize;
                    var rectangle = new Rectangle((int)(button.Animator.AnimationCounter * imageSize.X),
                        (int)(button.Animator.CurrentAnimation.Row * imageSize.Y),
                        (int)imageSize.X, (int)imageSize.Y); // this could be different if button is too big?

                    sb.Draw(
                        texture,
                        (button.Location + DIALOG_DRAW_LOCATION) * screenRatio,
                        rectangle,
                        LogicHandler.DrawColor,
                        GameConstants.DEFAULT_ROTATION,
                        GameConstants.DEFAULT_IMAGE_ORIGIN,
                        screenRatio,
                        SpriteEffects.None,
                        GameConstants.DEFAULT_LAYER);

                    GraphicsHandler.DrawCenteredText(sb, MenuResourceManager.MenuFont, button.Text, button.Location + DIALOG_DRAW_LOCATION, imageSize, screenRatio, LogicHandler.DrawColor);
                }
            }
        }

        public static void DrawExitMenu(SpriteBatch sb)
        {
            // draw the background for each of the menus
            var screenRatio = DataHelper.GetScreenRatioFromResolution();
            // draw everything but the title background
        }

        private static void DrawSmallDialog(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.DialogBackgroundSmall,
                DIALOG_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)DIALOG_SIZE_SMALL.X, (int)DIALOG_SIZE_SMALL.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawLargeDialog(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.DialogBackgroundLarge,
                DIALOG_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)DIALOG_SIZE_LARGE.X, (int)DIALOG_SIZE_LARGE.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawTitleBackground(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.TitleBackground,
                TITLE_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawStartBackground(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.StartBackground,
                START_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawFileSelectBackground(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.FileMenuBackground,
                FILE_SELECT_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawFileSelectImages(SpriteBatch sb, Vector2 screenRatio, int index)
        {
            RegionNames file0Region = DataHelper.File0SaveData != null ? DataHelper.File0SaveData.FileInfo.LastRegion : RegionNames.None;
            RegionNames file1Region = DataHelper.File1SaveData != null ? DataHelper.File1SaveData.FileInfo.LastRegion : RegionNames.None;
            RegionNames file2Region = DataHelper.File2SaveData != null ? DataHelper.File2SaveData.FileInfo.LastRegion : RegionNames.None;

            var imageRectange = new Rectangle(0, 0, (int)FILE_IMAGE_SIZE.X, (int)FILE_IMAGE_SIZE.Y);

            sb.Draw(
                MenuResourceManager.FilePictures[file0Region],
                FILE_IMAGE_0_LOCATION * screenRatio,
                imageRectange,
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);

            sb.Draw(
                MenuResourceManager.FilePictures[file1Region],
                FILE_IMAGE_1_LOCATION * screenRatio,
                imageRectange,
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);

            sb.Draw(
                MenuResourceManager.FilePictures[file2Region],
                FILE_IMAGE_2_LOCATION * screenRatio,
                imageRectange,
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);

            if (index == -1)
                return;

            var highlightLocation = FILE_IMAGE_0_LOCATION;

            if (index == 0)
                highlightLocation = FILE_IMAGE_0_LOCATION;
            else if (index == 1)
                highlightLocation = FILE_IMAGE_1_LOCATION;
            else if (index == 2)
                highlightLocation = FILE_IMAGE_2_LOCATION;

            sb.Draw(
                MenuResourceManager.FileImageHighlight,
                highlightLocation * screenRatio,
                imageRectange,
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private static void DrawOptionsBackground(SpriteBatch sb, Vector2 screenRatio)
        {
            sb.Draw(
                MenuResourceManager.OptionsMenuBackground,
                OPTIONS_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                LogicHandler.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }
    }
}
