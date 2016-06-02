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
    public class GraphicsHandler
    {
        private MenuGraphicsHandler _menuGraphicsHandler;

        public GraphicsHandler()
        {
            _menuGraphicsHandler = new MenuGraphicsHandler();
        }

        public void Draw(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            var state = GameStateHelper.CurrentState;

            if (state == GameStates.Start)
                _menuGraphicsHandler.DrawStart(sb, logic, resourceManager);
            else if (state == GameStates.StartMenu)
                _menuGraphicsHandler.DrawStartMenu(sb, logic, resourceManager);
            else if (state == GameStates.ExitMenu)
                _menuGraphicsHandler.DrawExitMenu(sb, logic, resourceManager);
        }
    }
}
