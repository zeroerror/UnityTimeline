using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimeline))]
    public class GameTimelinePropertyDrawer : GamePropertyDrawer
    {
        // 常量定义
        public const float trackW = 500;
        public const float trackH = 25;
        public const float labelW = 100;
        public const float labelLeftPadding = 5;
        public const float rowPadding = 27;
        public const float bgPadding_Hor = 10;
        public const float bgPadding_Ver = 10;
        Color bgColor = new Color(25 / 255.0f, 25 / 255.0f, 25 / 255.0f);
        Color trackColor = new Color(42 / 255.0f, 42 / 255.0f, 42 / 255.0f, 1);
        Color textColor = new Color(0.0f, 0.5f, 0.5f);
        Color transparentColor = new Color(1.0f, 0.0f, 0.0f, 0.0f);

        float bgWidth = labelW + trackW + bgPadding_Hor * 2;
        float GetBgHeight(int trackCount) => (trackCount + 1) * rowPadding + bgPadding_Ver * 2;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return trackH + bgPadding_Ver;
        }

        protected override void _OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var length_p = property.FindPropertyRelative("length");
            var length = length_p.floatValue;
            if (length <= 0)
            {
                this._editorLayout.LabelField(new GUIContent("时间轴长度必须大于0"), 500, trackH, Color.yellow, Color.gray, labelLeftPadding);
                return;
            }

            var frameRate = property.FindPropertyRelative("frameRate").intValue;
            var time_p = property.FindPropertyRelative("time");
            var tracks_p = property.FindPropertyRelative("tracks");

            var bgHeight = GetBgHeight(property.FindPropertyRelative("tracks").arraySize);
            var bgRect = new Rect(rect.x, rect.y, bgWidth, bgHeight);
            this._editorLayout.DrawTextureRect(bgRect, bgColor);
            this._editorLayout.AddAnchorOffset(bgPadding_Hor, bgPadding_Ver);

            // 绘制标签
            this._editorLayout.LabelField(new GUIContent("时间轴"), labelW, trackH, textColor, trackColor, labelLeftPadding);
            // 绘制时间
            var totalFrame = length * frameRate;
            var time = time_p.floatValue;
            var frame = Mathf.RoundToInt(time * frameRate);
            var timeStr = $"{frame} / {totalFrame}";
            this._editorLayout.AddAnchorOffset(labelW, 0);
            this._editorLayout.AddAnchorOffset(-50, 0);
            this._editorLayout.LabelField(new GUIContent(timeStr), 50, trackH, Color.white, transparentColor);
            // 绘制箭头轨道
            this._editorLayout.AddAnchorOffset(50, 0);
            this._editorLayout.AddRow(0, trackW, trackH, trackColor);
            _OnDragArrowTrack(property);
            var arrowRect = this._editorLayout.GetRect(15, 15);
            arrowRect.x -= 7.5f;
            arrowRect.y -= 7.5f;
            arrowRect.x += time / length * trackW;
            this._editorLayout.DrawArrow(arrowRect, Color.white);
            this._editorLayout.AddAnchorOffset(-labelW, rowPadding);

            // 绘制轨道
            for (int i = 0; i < tracks_p.arraySize; i++)
            {
                var track_p = tracks_p.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(track_p);
            }
        }

        private void _OnDragArrowTrack(SerializedProperty property)
        {
            var time_p = property.FindPropertyRelative("time");
            var length_p = property.FindPropertyRelative("length");
            var length = length_p.floatValue;
            var trackRect = this._editorLayout.GetRect(trackW, trackH);
            Event currentEvent = Event.current;
            if (new[] { EventType.MouseDrag, EventType.MouseDown }.Contains(currentEvent.type) && trackRect.Contains(currentEvent.mousePosition))
            {
                var offsetx = currentEvent.mousePosition.x - trackRect.x;
                var newTime = offsetx / trackW * length;
                time_p.floatValue = newTime;
                // 更新轨道当前时间
                var tracks_p = property.FindPropertyRelative("tracks");
                for (var i = 0; i < tracks_p.arraySize; i++)
                {
                    var track_p = tracks_p.GetArrayElementAtIndex(i);
                    track_p.FindPropertyRelative("time").floatValue = newTime;
                }
            }
        }
    }
}