using UnityEditor;
using UnityEngine;
using ZeroError.Config;

namespace ZeroError.EditorTool
{
    [CustomPropertyDrawer(typeof(GameTimeline), true)]
    public class GameTimelinePropertyDrawer : GamePropertyDrawer
    {
        float trackH => GameTimelineGUIConfig.trackH;
        float trackW => GameTimelineGUIConfig.trackW;
        float labelW => GameTimelineGUIConfig.labelW;
        float labelLeftPadding => GameTimelineGUIConfig.labelLeftPadding;
        float rowPadding => GameTimelineGUIConfig.rowHeight;
        float bgPadding_Hor => GameTimelineGUIConfig.bgPadding_Hor;
        float bgPadding_Ver => GameTimelineGUIConfig.bgPadding_Ver;
        Color bgColor => GameTimelineGUIConfig.bgColor;
        Color trackColor => GameTimelineGUIConfig.trackColor;
        Color trackLineColor => GameTimelineGUIConfig.trackLineColor;
        Color textColor => GameTimelineGUIConfig.textColor;
        Color transparentColor => GameTimelineGUIConfig.transparentColor;
        float bgWidth = GameTimelineGUIConfig.bgWidth;

        float GetBgHeight(int trackCount) => (trackCount + 2) * rowPadding + bgPadding_Ver * 2;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return trackH * 2 + bgPadding_Ver;
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
            this._editorLayout.AddAnchorOffset(labelW, 0);
            // 绘制按钮(上一帧、播放/暂停、下一帧)
            this._editorLayout.BeginOffset();
            this._editorLayout.AddAnchorOffset(-120, trackH * 0.3f);
            const float buttonW = 10;
            const float buttonPadding = 5;
            var verlineRect = this._editorLayout.GetRect(1, buttonW);
            // 上一帧按钮 图标
            var prevFrameRect = this._editorLayout.GetRect(buttonW, buttonW);
            this._editorLayout.DrawArrow(prevFrameRect, Color.white, 90);
            verlineRect.x = prevFrameRect.x;
            // 上一帧按钮点击事件
            var hasClickPrevFrame = prevFrameRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown;
            if (hasClickPrevFrame)
            {
                var curFrame = Mathf.FloorToInt(time_p.floatValue * frameRate);
                var newFrame = curFrame - 1;
                newFrame = Mathf.Clamp(newFrame, 0, Mathf.FloorToInt(length * frameRate));
                time_p.floatValue = newFrame / (float)frameRate;
                this._OnTimelineUpdate(property, time_p.floatValue);
            }
            this._editorLayout.DrawTextureRect(verlineRect, hasClickPrevFrame ? Color.yellow : Color.white);
            // 播放/暂停按钮 图标
            this._editorLayout.AddAnchorOffset(buttonW + buttonPadding, 0);
            var playRect = this._editorLayout.GetRect(buttonW, buttonW);
            // 播放/暂停按钮点击事件
            var hasClickPlay = playRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown;
            var isPlaying_p = property.FindPropertyRelative("isPlaying");
            if (hasClickPlay) isPlaying_p.boolValue = !isPlaying_p.boolValue;
            if (isPlaying_p.boolValue)
            {
                this._editorLayout.DrawArrow(playRect, GameTimelineGUIConfig.textColor, -90);
                var deltaTime = this.deltaTime * property.FindPropertyRelative("simulateSpeed").floatValue;
                var newTime = time_p.floatValue + deltaTime;
                newTime = newTime % length;
                time_p.floatValue = newTime;
                this._OnTimelineUpdate(property, time_p.floatValue);
            }
            else
            {
                this._editorLayout.DrawArrow(playRect, Color.white, -90);
            }
            // 下一帧按钮 图标
            this._editorLayout.AddAnchorOffset(buttonW + buttonPadding, 0);
            var nextFrameRect = this._editorLayout.GetRect(buttonW, buttonW);
            this._editorLayout.DrawArrow(nextFrameRect, Color.white, -90);
            verlineRect.x = nextFrameRect.x + buttonW;
            this._editorLayout.DrawTextureRect(verlineRect, Color.white);
            // 下一帧按钮点击事件
            var hasClickNextFrame = nextFrameRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown;
            if (hasClickNextFrame)
            {
                var curFrame = Mathf.FloorToInt(time_p.floatValue * frameRate);
                var newFrame = curFrame + 1;
                newFrame = Mathf.Clamp(newFrame, 0, Mathf.FloorToInt(length * frameRate));
                time_p.floatValue = newFrame / (float)frameRate;
                this._OnTimelineUpdate(property, time_p.floatValue);
            }
            this._editorLayout.EndOffset();
            // 绘制时间
            var totalFrame = length * frameRate;
            var frame = Mathf.FloorToInt(time_p.floatValue * frameRate);
            var timeStr = $"{frame} / {totalFrame}";
            this._editorLayout.AddAnchorOffset(-50, 0);
            this._editorLayout.LabelField(new GUIContent(timeStr), 50, trackH, Color.white, transparentColor);
            this._editorLayout.AddAnchorOffset(50, 0);
            // 绘制箭头轨道
            this._editorLayout.AddRow(0, trackW, trackH, trackColor);
            // 绘制箭头轨道分割线
            _DrawTrackLine(property, trackW, trackH, trackLineColor);
            // 绘制箭头
            var arrowRect = this._editorLayout.GetRect(15, 15);
            arrowRect.x -= 7.5f;
            arrowRect.y -= 7.5f;
            arrowRect.x += time_p.floatValue / length * trackW;
            this._editorLayout.DrawArrow(arrowRect, Color.white, 180);
            // 箭头拖拽
            OnDragArrowTrack(property);
            // 绘制模拟速度
            this._editorLayout.AddAnchorOffset(-labelW, 0);
            this._editorLayout.AddAnchorOffset(0, rowPadding);
            this._editorLayout.LabelField(new GUIContent("模拟速度"), labelW, trackH, textColor, trackColor, labelLeftPadding);
            this._editorLayout.AddAnchorOffset(70, 0);
            var simulateSpeed_p = property.FindPropertyRelative("simulateSpeed");
            var simulateSpeedRect = this._editorLayout.GetRect(50, trackH);
            simulateSpeedRect.y -= 2;
            simulateSpeedRect.height -= 2;
            simulateSpeed_p.floatValue = Mathf.Clamp(simulateSpeed_p.floatValue, 0.1f, 10f);
            simulateSpeed_p.floatValue = EditorGUI.FloatField(simulateSpeedRect, simulateSpeed_p.floatValue);
            // 绘制轨道
            this._editorLayout.AddAnchorOffset(0, rowPadding);
            for (int i = 0; i < tracks_p.arraySize; i++)
            {
                var track_p = tracks_p.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(track_p);
            }

            // 垂直时间线 
            var x = bgRect.x + bgPadding_Hor + labelW + time_p.floatValue / length * trackW;
            var lineRect = new Rect(x, bgRect.y + bgPadding_Ver, 1, bgHeight - bgPadding_Ver * 2);
            this._editorLayout.DrawTextureRect(lineRect, Color.white);
        }

        private void _DrawTrackLine(SerializedProperty property, float trackW, float trackH, Color color)
        {
            var length = property.FindPropertyRelative("length").floatValue;
            var frameRate = property.FindPropertyRelative("frameRate").intValue;
            var totalFrame = Mathf.FloorToInt(length * frameRate);
            for (int i = 0; i < totalFrame; i++)
            {
                var x = i / (float)totalFrame * trackW;
                var height = i % 5 == 0 && i > 0 ? trackH * 0.7f : trackH * 0.25f;
                var lineRect = new Rect(x + this._editorLayout.anchorPos.x, this._editorLayout.anchorPos.y, 1, height);
                this._editorLayout.DrawTextureRect(lineRect, color);
            }
        }

        private void OnDragArrowTrack(SerializedProperty property)
        {
            Event currentEvent = Event.current;
            var isMouseUp = currentEvent.type == EventType.MouseUp;
            if (this._isDragging && isMouseUp)
            {
                this._isDragging = false;
                return;
            }

            var time_p = property.FindPropertyRelative("time");
            var length_p = property.FindPropertyRelative("length");
            var length = length_p.floatValue;
            var trackRect = this._editorLayout.GetRect(trackW, trackH);
            var isMouseDown = currentEvent.type == EventType.MouseDown;
            var isMouseDrag = currentEvent.type == EventType.MouseDrag;
            var isContains = trackRect.Contains(currentEvent.mousePosition);
            if (isMouseDown && isContains)
            {
                this._isDragging = true;
            }

            if ((isMouseDown || isMouseDrag) && this._isDragging)
            {
                var offsetx = currentEvent.mousePosition.x - trackRect.x;
                var newTime = offsetx / trackW * length;
                newTime = Mathf.Clamp(newTime, 0, length);
                time_p.floatValue = newTime;
                // 更新轨道当前时间
                var tracks_p = property.FindPropertyRelative("tracks");
                for (var i = 0; i < tracks_p.arraySize; i++)
                {
                    var track_p = tracks_p.GetArrayElementAtIndex(i);
                    var trackTime_p = track_p.FindPropertyRelative("time");
                    var trackTime = trackTime_p.floatValue;
                    if (trackTime != newTime)
                    {
                        trackTime_p.floatValue = newTime;
                        this._OnTimelineUpdate(property, newTime);
                    }
                }
                Event.current.Use();
            }
        }
        protected virtual void _OnTimelineUpdate(SerializedProperty property, float time)
        {
            // Override this method to handle timeline time updates
        }
        private bool _isDragging = false;
    }
}