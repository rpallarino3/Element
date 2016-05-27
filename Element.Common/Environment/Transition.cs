using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.Environment
{
    public abstract class Transition { }

    public class CombinationTransition : Transition
    {
        public GameStates DestinationState { get; set; }
        public RegionNames DestinationRegion { get; set; }
        public int DestinationZone { get; set; }
        public int DestinationLevel { get; set; }
        public Vector2 DestinationCoords { get; set; }
        public bool Fade { get; set; }
    }

    public class RoamTransition : Transition
    {
        public List<Directions> InitiationDirections { get; set; }
        public RegionNames DestinationRegion { get; set; }
        public int DestinationZone { get; set; }
        public int DestinationLevel { get; set; }
        public Vector2 DestinationCoords { get; set; }
        public bool Fade { get; set; }
    }

    public class StateTransition : Transition
    {
        public GameStates DestinationState { get; set; }
        public bool Dim
        {
            get
            {
                return Dim;
            }
            set
            {
                if (value)
                {
                    Fade = false;
                }
            }
        }
        public bool Fade
        {
            get { return Dim; }
            set
            {
                if (value)
                {
                    Dim = false;
                }
            }
        }
    }
}
