
using System.Collections.Generic;

namespace Game.GameEditor
{
    [System.Serializable]
    public class GameTimeline
    {
        public List<GameTimelineTrack> tracks;
        public float length;
        public float time;
        public int frameRate = 30;
    }
}
