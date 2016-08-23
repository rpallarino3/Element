using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects.NPCs;
using Element.Common.HelperClasses;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.GameObjects.Npcs
{
    public class Npc : GameObject
    {
        private List<RegionNames> _possibleRegions;
        private NpcNames _name;
        private NpcTypes _type;
        private CharacterStates _state;
        private Directions _facingDirection;

        private bool _grabbing;
        private bool _climbing;
        private bool _running;

        public Npc(Vector2 location, int level) : base(location, level)
        {
            _animator = NpcAnimator.GetNpcAnimatorFromType(_type);
        }

        public override void UpdateLogic()
        {
            throw new NotImplementedException();
        }

        public override bool CanExecute(GameObjectActionType action, Directions direction)
        {
            return base.CanExecute(action, direction);
        }

        public override bool CanExecuteOn(GameObjectActionType action, Directions direction)
        {
            return base.CanExecuteOn(action, direction);
        }

        public override void Execute(GameObjectActionType action, Directions direction)
        {
            base.Execute(action, direction);
        }

        public override void ExecuteOn(GameObjectActionType action, Directions direction)
        {
            base.ExecuteOn(action, direction);
        }

        public override ActionInFrontType GetInteractAction()
        {
            return base.GetInteractAction();
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

        public bool Grabbing
        {
            get { return _grabbing; }
        }

        public bool Climbing
        {
            get { return _climbing; }
        }
    }
}
