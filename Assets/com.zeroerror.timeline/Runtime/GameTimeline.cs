#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace ZeroError.Config
{
    [System.Serializable]
    public class GameTimeline
    {
        [SerializeReference]
        public List<GameTimelineTrack> tracks;
        public float length;

        public float time;
        public int frameRate = 30;
        public bool isPlaying = false;
        public float simulateSpeed = 1.0f;
    }
}
#endif
