using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack), true)]
    public class GameTimelineTrackPropertyDrawer : GamePropertyDrawer
    {
        // 常量定义
        const float trackW = 500;
        const float trackH = 25;
        const float fragmentH = 20;
        const float labelW = 100;
        const float labelLeftPadding = 5;
        const float rowPadding = 27;
        protected virtual Color trackColor => new Color(42 / 255.0f, 42 / 255.0f, 42 / 255.0f, 1);
        protected virtual Color trackLabelColor => new Color(0.2f, 0.2f, 0.2f, 1);
        protected virtual Color textColor => new Color(0.0f, 0.5f, 0.5f);
        protected virtual Color fragmentColor => new Color(0.5f, 0.5f, 0.5f, 1);

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
            _ShowFragmentMenu(property);
            var fragments_p = property.FindPropertyRelative("fragments");
            for (int j = 0; j < fragments_p.arraySize; j++)
            {
                var fragment_p = fragments_p.GetArrayElementAtIndex(j);
                var startTime = fragment_p.FindPropertyRelative("startTime").floatValue;
                var endTime = fragment_p.FindPropertyRelative("endTime").floatValue;
                var length = property.FindPropertyRelative("length").floatValue;
                var x = startTime / length * trackW;
                var w = (endTime - startTime) / length * trackW;
                this._editorLayout.AddColumn(trackIndex, x, w, fragmentH, fragmentColor);
                _OnDragFragment(property, fragment_p, x, w);
            }
            this._editorLayout.AddAnchorOffset(-labelW, rowPadding);

            _CheckTimeChange(property);
        }

        private void _CheckTimeChange(SerializedProperty property)
        {
            var time = property.FindPropertyRelative("time").floatValue;
            var lastTime = property.FindPropertyRelative("lastTime").floatValue;
            if (time == lastTime) return;
            var lastTime_p = property.FindPropertyRelative("lastTime");
            lastTime_p.floatValue = time;
            OnTimeUpdate(property, time, lastTime);
        }

        protected virtual void OnTimeUpdate(SerializedProperty property, float time, float lastTime)
        {
        }

        private void _ShowFragmentMenu(SerializedProperty property)
        {
            var eventType = Event.current.type;
            var isRightClick = eventType == EventType.MouseDown && Event.current.button == 1;
            if (!isRightClick)
            {
                return;
            }

            var mousePos = Event.current.mousePosition;
            var rect = this._editorLayout.GetRect(trackW, trackH);
            if (!rect.Contains(mousePos))
            {
                return;
            }

            var fragments_p = property.FindPropertyRelative("fragments");
            var length = property.FindPropertyRelative("length").floatValue;

            SerializedProperty clickFragment = null;
            int clickIndex = -1;

            for (int j = 0; j < fragments_p.arraySize; j++)
            {
                var fragment_p = fragments_p.GetArrayElementAtIndex(j);
                var startTime = fragment_p.FindPropertyRelative("startTime").floatValue;
                var endTime = fragment_p.FindPropertyRelative("endTime").floatValue;
                var x = startTime / length * trackW + rect.x;
                var w = (endTime - startTime) / length * trackW;
                if (mousePos.x >= x && mousePos.x <= x + w)
                {
                    clickFragment = fragment_p;
                    clickIndex = j;
                    break;
                }
            }

            GenericMenu menu = new GenericMenu();
            if (clickFragment != null)
            {
                var trackName = property.FindPropertyRelative("trackName").stringValue;
                menu.AddItem(new GUIContent("删除片段 " + trackName), false, () =>
                {
                    fragments_p.DeleteArrayElementAtIndex(clickIndex);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            else
            {
                menu.AddItem(new GUIContent("添加片段"), false, () =>
                {
                    var disRatio = (mousePos.x - rect.x) / trackW;
                    var startTime = disRatio * length;
                    var endTime = startTime + 0.1f;
                    endTime = Mathf.Min(endTime, length);
                    fragments_p.InsertArrayElementAtIndex(fragments_p.arraySize);
                    var newFragment_p = fragments_p.GetArrayElementAtIndex(fragments_p.arraySize - 1);
                    newFragment_p.FindPropertyRelative("startTime").floatValue = startTime;
                    newFragment_p.FindPropertyRelative("endTime").floatValue = endTime;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
            Event.current.Use();
        }

        private void _OnDragFragment(SerializedProperty property, SerializedProperty fragment_p, float offsetX, float width)
        {
            var eventType = Event.current.type;

            var isMouseUp = eventType == EventType.MouseUp && Event.current.button == 0;
            if (isMouseUp && this._dragingFragment != null)
            {
                this._dragingFragment = null;
                return;
            }

            var isMouseDrag = eventType == EventType.MouseDrag && Event.current.button == 0;
            if (!isMouseDrag)
            {
                return;
            }

            // 锁定当前拖拽片段
            if (this._dragingFragment == null)
            {
                var fragmentRect = new Rect(offsetX + this._editorLayout.anchorPos.x, this._editorLayout.anchorPos.y, width, fragmentH);
                var mousePos = Event.current.mousePosition;
                if (fragmentRect.Contains(mousePos))
                {
                    this._dragingFragment = fragment_p;
                }
            }
            if (this._dragingFragment?.propertyPath != fragment_p.propertyPath)
            {
                return;
            }

            var length = property.FindPropertyRelative("length").floatValue;
            var deltaX = Event.current.delta.x;
            var deltaTime = deltaX / trackW * length;
            var startTime_p = fragment_p.FindPropertyRelative("startTime");
            var startTime = startTime_p.floatValue;
            var endTime_p = fragment_p.FindPropertyRelative("endTime");
            var endTime = endTime_p.floatValue;
            var newStartTime = startTime + deltaTime;
            var newEndTime = endTime + deltaTime;

            var isOverFlow = newStartTime < 0 || newEndTime > length;
            if (isOverFlow) return;

            startTime_p.floatValue = newStartTime;
            endTime_p.floatValue = newEndTime;
            property.serializedObject.ApplyModifiedProperties();
            Event.current.Use();
        }
        private SerializedProperty _dragingFragment = null;
    }
}