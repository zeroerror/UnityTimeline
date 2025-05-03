using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack))]
    public class GameTimelineTrackPropertyDrawer : GamePropertyDrawer
    {
        // 常量定义
        const float trackW = 500;
        const float trackH = 25;
        const float fragmentH = 20;
        const float labelW = 100;
        const float labelLeftPadding = 5;
        const float rowPadding = 27;
        Color trackColor = new Color(42 / 255.0f, 42 / 255.0f, 42 / 255.0f, 1);
        Color trackLabelColor = new Color(0.2f, 0.2f, 0.2f, 1);
        Color textColor = new Color(0.0f, 0.5f, 0.5f);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return trackH;
        }

        protected override void _OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            this._editorLayout.AddAnchorOffset(GameTimelinePropertyDrawer.bgPadding_Hor, 0);

            var trackName = property.FindPropertyRelative("trackName").stringValue;
            this._editorLayout.LabelField(new GUIContent(trackName), labelW, trackH, textColor, trackLabelColor, labelLeftPadding);
            this._editorLayout.AddAnchorOffset(labelW, 0);
            var trackIndex = property.FindPropertyRelative("trackIndex").intValue;
            this._editorLayout.AddRow(trackIndex, trackW, trackH, trackColor);
            this._editorLayout.AddAnchorOffset(-labelW, rowPadding);
            var fragments_p = property.FindPropertyRelative("fragments");
            for (int j = 0; j < fragments_p.arraySize; j++)
            {
                var fragment_p = fragments_p.GetArrayElementAtIndex(j);
                var startTime = fragment_p.FindPropertyRelative("startTime").floatValue;
                var endTime = fragment_p.FindPropertyRelative("endTime").floatValue;
                var length = property.FindPropertyRelative("length").floatValue;
                var x = startTime / length * trackW;
                var w = (endTime - startTime) / length * trackW;
                this._editorLayout.AddColumn(trackIndex, x, w, fragmentH, Color.green);
            }

            var time = property.FindPropertyRelative("time").floatValue;
            var lastTime = property.FindPropertyRelative("lastTime").floatValue;
            if (time != lastTime)
            {
                _OnUpdate(property, time);
            }
        }

        protected virtual void _OnUpdate(SerializedProperty property, float newTime)
        {
            var trackName = property.FindPropertyRelative("trackName").stringValue;
            var lastTime_p = property.FindPropertyRelative("lastTime");
            lastTime_p.floatValue = newTime;
        }
    }
}