using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;
using Element.Common.Enumerations.GameObjects.NPCs;

namespace Element.Common.HelperClasses
{
    public static class NpcAnimator
    {
        private static readonly Vector2 PLAYER_SIZE = new Vector2(40, 65);
        private static readonly Vector2 BASIC_HUMANOID_SIZE = new Vector2(40, 65);

        private static Dictionary<NpcTypes, Animator> _npcAnimations;

        static NpcAnimator()
        {
            _npcAnimations = new Dictionary<NpcTypes, Animator>();
            CreatePlayerAnimator();
            CreateBasicHumanoidAnimator();
        }

        private static void CreatePlayerAnimator()
        {
            var animationDictionary = new Dictionary<int, Animation>();

            var animator = new Animator(animationDictionary, 0, PLAYER_SIZE);
            _npcAnimations.Add(NpcTypes.Player, animator);
        }

        private static void CreateBasicHumanoidAnimator()
        {
            var animationDictionary = new Dictionary<int, Animation>();
            
            // stand
            // walk
            // run
            // jump
            // push
            // pull
            // grab
            // stationary climb
            // climb
            // cast
            // fall
            // lay
            // dead
            // sit

            var animator = new Animator(animationDictionary, 0, BASIC_HUMANOID_SIZE);
            _npcAnimations.Add(NpcTypes.BasicHumanoid, animator);
        }

        public static Animator GetNpcAnimatorFromType(NpcTypes type)
        {
            return _npcAnimations[type];
        }
    }
}
