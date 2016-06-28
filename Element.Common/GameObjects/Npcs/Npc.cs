using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.HelperClasses;

namespace Element.Common.GameObjects.Npcs
{
    public class Npc : GameObject
    {
        private List<RegionNames> _possibleRegions;
        private NpcNames _name;
        private NpcTypes _type;
        private CharacterStates _state;
        private Directions _facingDirection;

        public Npc(Vector2 location, int level) : base(location, level)
        {
            _animator = NpcAnimator.GetNpcAnimatorFromType(_type);
        }

        public bool CanExecute(NpcAction action, Directions direction)
        {
            return false;
        }

        public bool CanExecuteInFacingDirection(NpcAction action)
        {
            return CanExecute(action, _facingDirection);
        }

        public void ExecuteAction(NpcAction action, Directions direction)
        {

        }

        public void ExecuteActionInFacingDirection(NpcAction action)
        {
            ExecuteAction(action, _facingDirection);
        }

        public void ReleaseGrab()
        {

        }

        public void SetRun(bool run)
        {

        }

        // make sure to get this value from npc mapper on npc creation
        public List<RegionNames> PossibleRegions 
        {
            get { return _possibleRegions; }
        }

        public NpcNames Name
        {
            get { return _name; }
        }

        public NpcTypes Type
        {
            get { return _type; }
        }

        public Directions FacingDirection
        {
            get { return _facingDirection; }
        }
    }
}
