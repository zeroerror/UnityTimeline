using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.GameEditor
{
    public class GameEditorLayout
    {
        public Vector2 anchorPos { get; private set; }
        public void AddAnchorOffset(float xOffset, float yOffset)
        {
            anchorPos += new Vector2(xOffset, yOffset);
        }
        public void SetAnchorPos(Vector2 pos)
        {
            anchorPos = pos;
        }

        public Rect GetRect(float width, float height)
        {
            var rect = new Rect(anchorPos.x, anchorPos.y, width, height);
            return rect;
        }

        public void AddRow(int rowId, float width, float height, Color bgColor)
        {
            var rect = GetRect(width, height);
            DrawTextureRect(rect, bgColor);
            _rowRects[rowId] = rect;
        }

        public void AddColumn(int rowId, float xOffset, float width, float height, Color bgColor)
        {
            var rowRect = _rowRects[rowId];
            var columX = rowRect.x + xOffset;
            var columY = rowRect.y + (rowRect.height - height) / 2;
            var columWidth = width;
            var columHeight = height;
            var columRect = new Rect(columX, columY, columWidth, columHeight);
            DrawTextureRect(columRect, bgColor);
        }
        private Dictionary<int, Rect> _rowRects = new Dictionary<int, Rect>();

        public void LabelField(GUIContent label, float width, float height, Color textColor, Color bgColor, float lPadding = 0f)
        {
            var rect = GetRect(width, height);
            // 绘制背景
            DrawTextureRect(rect, bgColor);
            // 绘制文本
            var style = new GUIStyle(LabelStyle)
            {
                normal = { textColor = textColor }
            };
            rect.x += lPadding;
            EditorGUI.LabelField(rect, label, style);
        }
        private GUIStyle _labelStyle;
        private GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(GUI.skin.label)
                    {
                        normal = { textColor = Color.white },
                        alignment = TextAnchor.MiddleLeft,
                        fontSize = 12,
                        padding = new RectOffset(0, 0, 0, 0)
                    };
                }
                return _labelStyle;
            }
        }

        public void DrawTextureRect(Rect rect, Color bgColor, Texture2D texture = null)
        {
            if (bgColor.a == 0)
            {
                return;
            }
            texture = texture ?? new Texture2D(1, 1);
            texture.SetPixel(0, 0, bgColor);
            texture.Apply();
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
            Object.DestroyImmediate(texture);
        }

        public Texture2D CreateArrowTexture(Color arrowColor, int width, int height)
        {
            // 创建一个新的纹理
            Texture2D arrowTexture = new Texture2D(width, height);
            // 初始化纹理的所有像素为透明色
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.clear;
            }
            // 绘制朝下的三角箭头
            for (int y = 0; y < height; y++)
            {
                // 计算箭头的宽度范围
                int startX = (width / 2) - (y * width / height / 2);
                int endX = (width / 2) + (y * width / height / 2);

                for (int x = 0; x < width; x++)
                {
                    if (x >= startX && x <= endX)
                    {
                        // 将箭头区域的像素设置为箭头颜色
                        pixels[y * width + x] = arrowColor;
                    }
                }
            }
            // 设置纹理的像素
            arrowTexture.SetPixels(pixels);
            // 应用像素更改
            arrowTexture.Apply();
            return arrowTexture;
        }
        public void DrawArrow(Rect position, Color arrowColor)
        {
            int width = (int)position.width;
            int height = (int)position.height;
            Texture2D arrowTexture = this._arrowTexture ?? (this._arrowTexture = CreateArrowTexture(arrowColor, width, height));
            GUI.DrawTexture(position, arrowTexture);
        }
        private Texture2D _arrowTexture;
    }
}