using System.Collections.Generic;
namespace Game.Config
{
    [System.Serializable]
    public class GameTimelineTrack
    {
        public string trackName = "未定义";
        public int trackIndex;
        public float length;
        public float time;
        public float lastTime;

        public List<GameTimelineTrackFragment> fragments;

        public GameTimelineTrack()
        {
        }
    }
}