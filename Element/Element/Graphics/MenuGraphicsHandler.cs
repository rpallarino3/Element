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
    public class MenuGraphicsHandler
    {
        private readonly Vector2 SPLASH_SCREEN_DRAW_LOCATION = new Vector2(0, 0);
        private readonly Vector2 TITLE_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private readonly Vector2 START_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private readonly Vector2 FILE_SELECT_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private readonly Vector2 OPTIONS_BACKGROUND_DRAW_LOCATION = new Vector2(0, 0);
        private readonly Vector2 EXIT_BACKGROUND_DRAW_LOCATION = new Vector2(495, 185);

        private readonly Vector2 FILE_IMAGE_0_LOCATION = new Vector2(110, 60);
        private readonly Vector2 FILE_IMAGE_1_LOCATION = new Vector2(490, 60);
        private readonly Vector2 FILE_IMAGE_2_LOCATION = new Vector2(870, 60);
        private readonly Vector2 FILE_IMAGE_SIZE = new Vector2(300, 400);

        private readonly Vector2 DIALOG_DRAW_LOCATION = new Vector2(525, 300);
        private readonly Vector2 DIALOG_SIZE_SMALL = new Vector2(230, 60);
        private readonly Vector2 DIALOG_SIZE_LARGE = new Vector2(230, 120);

        public void DrawStart(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {

        }

        public void DrawStartMenu(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            var screenRatio = DataHelper.GetScreenRatioFromResolution();

            DrawTitleBackground(sb, logic, resourceManager, screenRatio);

            var activeMenuPage = logic.StartAndExitMenuLogicHandler.ActiveMenuPage;

            if (activeMenuPage.Name == MenuPageNames.Start)
                DrawStartBackground(sb, logic, resourceManager, screenRatio);
            else if (activeMenuPage.Name == MenuPageNames.FileSelect)
            {
                DrawFileSelectBackground(sb, logic, resourceManager, screenRatio);
                DrawFileSelectImages(sb, logic, resourceManager, screenRatio, ((FileSelectMenuPage)activeMenuPage).GetFileSelectedIndex());
            }
            else if (activeMenuPage.Name == MenuPageNames.Options)
                DrawOptionsBackground(sb, logic, resourceManager, screenRatio);

            foreach (var button in activeMenuPage.Buttons)
            {
                var texture = resourceManager.MenuResourceManager.ButtonTextures[button.Style];
                var imageSize = button.Animator.CurrentAnimation.ImageSize;
                var rectangle = new Rectangle((int)(button.Animator.AnimationCounter * imageSize.X),
                    (int)(button.Animator.CurrentAnimation.Row * imageSize.Y),
                    (int)imageSize.X, (int)imageSize.Y); // this could be different if button is too big?

                sb.Draw(
                    texture,
                    button.Location * screenRatio,
                    rectangle,
                    logic.DrawColor,
                    GameConstants.DEFAULT_ROTATION,
                    GameConstants.DEFAULT_IMAGE_ORIGIN,
                    screenRatio,
                    SpriteEffects.None,
                    GameConstants.DEFAULT_LAYER);

                GraphicsHandler.DrawCenteredText(sb, resourceManager.MenuResourceManager.MenuFont, button.Text, button.Location, imageSize, screenRatio, logic.DrawColor);
            }

            if (activeMenuPage.DialogOpen)
            {
                if (activeMenuPage.CurrentDialog.Buttons.Count == 0)
                    DrawSmallDialog(sb, logic, resourceManager, screenRatio);
                else
                    DrawLargeDialog(sb, logic, resourceManager, screenRatio);

                foreach (var button in activeMenuPage.CurrentDialog.Buttons)
                {
                    var texture = resourceManager.MenuResourceManager.ButtonTextures[button.Style];
                    var imageSize = button.Animator.CurrentAnimation.ImageSize;
                    var rectangle = new Rectangle((int)(button.Animator.AnimationCounter * imageSize.X),
                        (int)(button.Animator.CurrentAnimation.Row * imageSize.Y),
                        (int)imageSize.X, (int)imageSize.Y); // this could be different if button is too big?

                    sb.Draw(
                        texture,
                        (button.Location + DIALOG_DRAW_LOCATION) * screenRatio,
                        rectangle,
                        logic.DrawColor,
                        GameConstants.DEFAULT_ROTATION,
                        GameConstants.DEFAULT_IMAGE_ORIGIN,
                        screenRatio,
                        SpriteEffects.None,
                        GameConstants.DEFAULT_LAYER);

                    GraphicsHandler.DrawCenteredText(sb, resourceManager.MenuResourceManager.MenuFont, button.Text, button.Location + DIALOG_DRAW_LOCATION, imageSize, screenRatio, logic.DrawColor);
                }
            }
        }

        public void DrawExitMenu(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            // draw the background for each of the menus
            var screenRatio = DataHelper.GetScreenRatioFromResolution();
            // draw everything but the title background
        }

        private void DrawSmallDialog(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.DialogBackgroundSmall,
                DIALOG_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)DIALOG_SIZE_SMALL.X, (int)DIALOG_SIZE_SMALL.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawLargeDialog(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.DialogBackgroundLarge,
                DIALOG_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)DIALOG_SIZE_LARGE.X, (int)DIALOG_SIZE_LARGE.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawTitleBackground(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.TitleBackground,
                TITLE_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawStartBackground(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.StartBackground,
                START_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawFileSelectBackground(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.FileMenuBackground,
                FILE_SELECT_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawFileSelectImages(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio, int index)
        {
            RegionNames file0Region = DataHelper.File0SaveData != null ? DataHelper.File0SaveData.FileInfo.LastRegion : RegionNames.None;
            RegionNames file1Region = DataHelper.File1SaveData != null ? DataHelper.File1SaveData.FileInfo.LastRegion : RegionNames.None;
            RegionNames file2Region = DataHelper.File2SaveData != null ? DataHelper.File2SaveData.FileInfo.LastRegion : RegionNames.None;

            var imageRectange = new Rectangle(0, 0, (int)FILE_IMAGE_SIZE.X, (int)FILE_IMAGE_SIZE.Y);

            sb.Draw(
                resourceManager.MenuResourceManager.FilePictures[file0Region],
                FILE_IMAGE_0_LOCATION * screenRatio,
                imageRectange,
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);

            sb.Draw(
                resourceManager.MenuResourceManager.FilePictures[file1Region],
                FILE_IMAGE_1_LOCATION * screenRatio,
                imageRectange,
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);

            sb.Draw(
                resourceManager.MenuResourceManager.FilePictures[file2Region],
                FILE_IMAGE_2_LOCATION * screenRatio,
                imageRectange,
                logic.DrawColor,
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
                resourceManager.MenuResourceManager.FileImageHighlight,
                highlightLocation * screenRatio,
                imageRectange,
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }

        private void DrawOptionsBackground(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager, Vector2 screenRatio)
        {
            sb.Draw(
                resourceManager.MenuResourceManager.OptionsMenuBackground,
                OPTIONS_BACKGROUND_DRAW_LOCATION * screenRatio,
                new Rectangle(0, 0, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X, (int)GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y),
                logic.DrawColor,
                GameConstants.DEFAULT_ROTATION,
                GameConstants.DEFAULT_IMAGE_ORIGIN,
                screenRatio,
                SpriteEffects.None,
                GameConstants.DEFAULT_LAYER);
        }
    }
}
