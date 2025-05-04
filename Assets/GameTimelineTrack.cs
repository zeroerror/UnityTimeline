using System.Collections.Generic;
namespace Game.GameEditor
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

        protected virtual void _OnUpdate(float newTime)
        {
        }
    }
}