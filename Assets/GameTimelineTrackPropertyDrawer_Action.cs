using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack_Action))]
    public class GameTimelineTrackPropertyDrawer_Action : GameTimelineTrackPropertyDrawer
    {
        protected override Color fragmentColor => new Color(1f, 0.5f, 0.5f, 0.5f);
        protected override void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
            Debug.Log($"轨道【行为】时间更新：{time}");
        }
    }
}