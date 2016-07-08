using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class LiquidTile : Tile
    {
        public override bool? CanMoveInto(Directions direction)
        {
            if (_reserved)
                return false;
            if (_npc != null)
                return false;
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.WalkOn, direction);

            return null;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecute(TileObjectActions.WalkOnTop, direction);
        }

        public override bool? CanPushInto(Directions direction)
        {
            if (_reserved)
                return false;
            if (_npc != null)
                return false;
            if (_standardObject != null)
                return false;

            return null;
        }

        public override bool? CanPushOnTop(Directions direction)
        {
            if (_standardObject == null)
                return null;

            return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction);
        }

        public override bool? CanLandOn(bool pushed)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.LandOnTop, Directions.Up); // direction doesn't matter here

            return null;
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }

        public override NpcAction GetMoveActionFromTile(Directions direction)
        {
            if (_reserved)
                return NpcAction.None;

            if (_standardObject != null)
                return _standardObject.GetNpcActionFromMove(direction);

            return NpcAction.Jump;
        }
    }
}
