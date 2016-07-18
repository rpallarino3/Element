using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class GapTile : Tile
    {
        public override bool? CanMoveOn(Directions direction)
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
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.WalkOnTop, direction);

            return false;
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
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction); // i think this is the action we want

            return null; // i think this is what we want to return here
        }

        public override bool? CanPushOver(Directions direction)
        {
            // i think if the tile is reserved by anything we don't want to push into
            // we could do more investigation as to what the reserving object is
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction);
            
            return null;
        }

        public override bool CanMoveVerticallyThrough()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            return true;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.LandOnTop, direction);

            return false;
        }

        public override bool? CanLandOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            // i think if there is a standard object inside we don't allow jumping into
            if (_standardObject != null)
                return false;
            
            return null;
        }

        public override bool? CanBePlacedIn()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            return null;
        }

        public override bool? CanDropInto()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null) // might need to come up with new actions for this stuff
                return _standardObject.CanExecute(TileObjectActions.FallOnTop, Directions.Up);
            
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
