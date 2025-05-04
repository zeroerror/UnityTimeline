using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_TimeScale))]
    public class GameTimelineTrackPropertyDrawer_TimeScale : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(1f, 1f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
            Debug.Log($"轨道【时间缩放】时间更新：{time}");
        }
    }
}