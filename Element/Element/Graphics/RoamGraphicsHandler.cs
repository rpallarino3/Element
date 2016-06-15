using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.HelperClasses;
using Element.ResourceManagement;
using Element.Logic;

namespace Element.Graphics
{
    public class RoamGraphicsHandler
    {

        public void DrawRoam(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            var cameraTopLeft = Camera.Location - GameConstants.SCREEN_SIZE_IN_GAME_UNITS / 2 + new Vector2(1, 1);
        }

        private bool IsOnScreen(Vector2 cameraTopLeft, Vector2 objLocation, Vector2 objSize)
        {
            if (objLocation.Y + objSize.Y <= cameraTopLeft.Y)
                return false;
            if (objLocation.Y >= cameraTopLeft.Y + GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y)
                return false;
            if (objLocation.X + objSize.X <= cameraTopLeft.X)
                return false;
            if (objLocation.X >= cameraTopLeft.X + GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X)
                return false;

            return true;
        }
    }
}
