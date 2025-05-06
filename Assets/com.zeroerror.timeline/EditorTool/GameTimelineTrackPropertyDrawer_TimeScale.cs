using UnityEditor;
using UnityEngine;
using ZeroError.Config;

namespace ZeroError.EditorTool
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_TimeScale))]
    public class GameTimelineTrackPropertyDrawer_TimeScale : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(1f, 1f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
        }
    }
}