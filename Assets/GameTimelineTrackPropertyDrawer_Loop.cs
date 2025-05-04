using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_Loop))]
    public class GameTimelineTrackPropertyDrawer_Loop : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(0.5f, 1f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
            Debug.Log($"轨道【循环】时间更新：{time}");
        }
    }
}