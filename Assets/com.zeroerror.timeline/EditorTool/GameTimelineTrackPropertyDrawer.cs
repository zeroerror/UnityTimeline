using UnityEditor;
using UnityEngine;
using ZeroError.Config;

namespace ZeroError.EditorTool
{
    [CustomPropertyDrawer(typeof(GameTimelineTrack), true)]
    public class GameTimelineTrackPropertyDrawer : GamePropertyDrawer
    {
        float labelW => GameTimelineGUIConfig.labelW;
        float trackW => GameTimelineGUIConfig.trackW;
        float trackH => GameTimelineGUIConfig.trackH;
        float rowPadding => GameTimelineGUIConfig.rowHeight;
        float fragmentH => GameTimelineGUIConfig.fragmentH;
        float fragmentBorderPadding => GameTimelineGUIConfig.fragmentBorderPadding;
        Color trackColor => GameTimelineGUIConfig.trackColor;
        Color fragmentBorderColor => GameTimelineGUIConfig.fragmentBorderColor;
        Color textColor => GameTimelineGUIConfig.textColor;
        float labelLeftPadding => GameTimelineGUIConfig.labelLeftPadding;
        Color trackLabelColor => GameTimelineGUIConfig.trackLabelColor;
        float bgPadding_Hor => GameTimelineGUIConfig.bgPadding_Hor;

        protected virtual Color fragmentColor => new Color(0.5f, 1f, 0.5f, 0.5f);
        protected virtual bool _canStretch => true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GameTimelineGUIConfig.trackH;
        }

        protected override void _OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            this._editorLayout.AddAnchorOffset(bgPadding_Hor, 0);
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
                var startX = startTime / length * trackW;
                var fragmentW = (endTime - startTime) / length * trackW;
                var fragmentRect = new Rect(startX + this._editorLayout.anchorPos.x, this._editorLayout.anchorPos.y, fragmentW, fragmentH);
                _OnDrawFragment(fragmentRect, property, fragment_p, j, trackIndex, startX, fragmentW);
                OnClickFragment(fragmentRect, property, fragment_p, j);
                _OnDragFragment(fragmentRect, property, fragment_p, j);
                _OnStretchFragment(property, fragment_p, j, startX, fragmentW);
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
                    this._OnFragmentDelete(property, clickIndex);
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
                    this._OnFragmentCreate(newFragment_p, clickIndex);
                });
            }

            menu.ShowAsContext();
            Event.current.Use();
        }

        protected virtual void _OnFragmentCreate(SerializedProperty newFragment_p, int fragmentIndex)
        {
        }

        protected virtual void _OnFragmentDelete(SerializedProperty property, int fragmentIndex)
        {
        }

        protected virtual void _OnDrawFragment(Rect fragmentRect, SerializedProperty property, SerializedProperty fragment_p, int fragmentIndex, int trackIndex, float startX, float fragmentW)
        {
            this._editorLayout.AddColumn(trackIndex, startX, fragmentW, fragmentH, fragmentColor);
        }

        protected virtual void OnClickFragment(Rect fragmentRect, SerializedProperty property, SerializedProperty fragment_p, int fragmentIndex)
        {
            // 基于边界居中片段内部
            fragmentRect.x += fragmentBorderPadding;
            fragmentRect.width -= fragmentBorderPadding * 2;
            fragmentRect.width = Mathf.Max(fragmentRect.width, 0.1f);
            var eventType = Event.current.type;
            var isMouseDown = eventType == EventType.MouseDown && Event.current.button == 0;
            if (!isMouseDown)
            {
                return;
            }
            var mousePos = Event.current.mousePosition;
            if (!fragmentRect.Contains(mousePos))
            {
                return;
            }

            var selectedTrackIndex = property.FindPropertyRelative("trackIndex").intValue;
            this._OnClickFragment(property, selectedTrackIndex, fragmentIndex);
        }
        protected virtual void _OnClickFragment(SerializedProperty property, int trackIndex, int fragmentIndex) { }

        private void _OnDragFragment(Rect fragmentRect, SerializedProperty property, SerializedProperty fragment_p, int fragmentIndex)
        {
            if (this._stretchingFragment != null) return;

            // 定义拖拽区域
            this._OnDragFragmentDefine(ref fragmentRect);

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

            var isOverflow = newStartTime < 0 || newEndTime > length;
            if (isOverflow)
            {
                var isOverflowDir = newStartTime < 0 && deltaTime < 0 || newEndTime > length && deltaTime > 0;
                if (isOverflowDir)
                    return;
            }

            startTime_p.floatValue = newStartTime;
            endTime_p.floatValue = newEndTime;
            property.serializedObject.ApplyModifiedProperties();
            Event.current.Use();

            if (startTime != newStartTime || endTime != newEndTime)
            {
                this._OnFragmentUpdate(fragment_p, fragmentIndex, startTime_p.floatValue, endTime_p.floatValue);
            }
        }
        protected virtual void _OnDragFragmentDefine(ref Rect fragmentRect)
        {
            fragmentRect.x += fragmentBorderPadding;
            fragmentRect.width -= fragmentBorderPadding * 2;
            fragmentRect.width = Mathf.Max(fragmentRect.width, 0.1f);
        }
        private SerializedProperty _dragingFragment = null;

        private void _OnStretchFragment(SerializedProperty property, SerializedProperty fragment_p, int fragmentIndex, float offsetX, float width)
        {
            if (!this._canStretch) return;
            var fragmentRect_l = new Rect(offsetX + this._editorLayout.anchorPos.x, this._editorLayout.anchorPos.y, fragmentBorderPadding, trackH);
            var fragmentRect_r = new Rect(offsetX + width + this._editorLayout.anchorPos.x - fragmentBorderPadding, this._editorLayout.anchorPos.y, fragmentBorderPadding, trackH);
            this._editorLayout.DrawTextureRect(fragmentRect_l, fragmentBorderColor);
            this._editorLayout.DrawTextureRect(fragmentRect_r, fragmentBorderColor);

            if (this._dragingFragment != null) return;
            var eventType = Event.current.type;
            var isMouseUp = eventType == EventType.MouseUp && Event.current.button == 0;
            if (isMouseUp && this._stretchingFragment != null)
            {
                this._stretchingFragment = null;
                this._stretchSide = 0;
                return;
            }

            // 锁定当前拉伸片段
            var isMouseDrag = eventType == EventType.MouseDrag && Event.current.button == 0;
            if (this._stretchingFragment == null)
            {
                if (fragmentRect_l.Contains(Event.current.mousePosition))
                {
                    if (isMouseDrag) this._stretchingFragment = fragment_p;
                    this._stretchSide = 1;
                }
                else if (fragmentRect_r.Contains(Event.current.mousePosition))
                {
                    if (isMouseDrag) this._stretchingFragment = fragment_p;
                    this._stretchSide = 2;
                }
            }

            var isTarget = this._stretchingFragment?.propertyPath == fragment_p.propertyPath;
            var isStretching = isMouseDrag && isTarget;
            var length = property.FindPropertyRelative("length").floatValue;
            var deltaTime = length * Event.current.delta.x / trackW;

            var startTime_p = fragment_p.FindPropertyRelative("startTime");
            var endTime_p = fragment_p.FindPropertyRelative("endTime");
            var oldStartTime = startTime_p.floatValue;
            var oldEndTime = endTime_p.floatValue;
            switch (this._stretchSide)
            {
                case 1:
                    if (isStretching)
                    {
                        var startTime = startTime_p.floatValue;
                        var newStartTime = startTime + deltaTime;
                        newStartTime = Mathf.Max(newStartTime, 0);
                        startTime_p.floatValue = newStartTime;
                    }
                    EditorGUIUtility.AddCursorRect(fragmentRect_l, MouseCursor.ResizeHorizontal);
                    break;
                case 2:
                    if (isStretching)
                    {
                        var endTime = endTime_p.floatValue;
                        var newEndTime = endTime + deltaTime;
                        newEndTime = Mathf.Min(newEndTime, length);
                        endTime_p.floatValue = newEndTime;
                    }
                    EditorGUIUtility.AddCursorRect(fragmentRect_r, MouseCursor.ResizeHorizontal);
                    break;
            }
            if (isStretching)
            {
                property.serializedObject.ApplyModifiedProperties();
                Event.current.Use();
            }
            if (oldStartTime != startTime_p.floatValue || oldEndTime != endTime_p.floatValue)
            {
                this._OnFragmentUpdate(fragment_p, fragmentIndex, startTime_p.floatValue, endTime_p.floatValue);
            }
        }
        private SerializedProperty _stretchingFragment;
        private int _stretchSide;

        protected virtual void _OnFragmentUpdate(SerializedProperty fragment_p, int fragmentIndex, float startTime, float endTime)
        {
        }
    }
}