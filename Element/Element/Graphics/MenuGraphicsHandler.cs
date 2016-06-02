using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;
using Element.ResourceManagement;
using Element.Logic;

namespace Element.Graphics
{
    public class MenuGraphicsHandler
    {

        public void DrawStart(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {

        }

        public void DrawStartMenu(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            var screenRatio = DataHelper.GetScreenRatioFromResolution();

        }

        public void DrawExitMenu(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            // draw the background for each of the menus
            var screenRatio = DataHelper.GetScreenRatioFromResolution();
        }
    }
}
