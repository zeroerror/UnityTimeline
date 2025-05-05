
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    [System.Serializable]
    public class GameTimeline
    {
        [SerializeReference]
        public List<GameTimelineTrack> tracks;
        public float length;
        public float time;
        public int frameRate = 30;
    }
}
