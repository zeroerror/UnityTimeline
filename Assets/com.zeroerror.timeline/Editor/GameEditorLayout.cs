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
            if (_isBeginOffset)
            {
                _offset += new Vector2(xOffset, yOffset);
            }
        }
        public void BeginOffset()
        {
            _isBeginOffset = true;
            _offset = Vector2.zero;
        }
        public void EndOffset()
        {
            anchorPos -= _offset;
            _isBeginOffset = false;
        }
        private bool _isBeginOffset = false;
        private Vector2 _offset = Vector2.zero;

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

        public Texture2D CreateArrowTexture(Color arrowColor, int width, int height, float angle = 0)
        {
            // 创建一个新的纹理
            Texture2D arrowTexture = new Texture2D(width, height);

            // 初始化纹理的所有像素为透明色
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.clear;
            }

            // 将角度转换为弧度
            float radAngle = angle * Mathf.Deg2Rad;

            // 中心点
            float centerX = width / 2f;
            float centerY = height / 2f;

            // 绘制箭头像素
            for (int y = 0; y < height; y++)
            {
                // 在箭头的高度范围内，动态计算宽度
                float normalizedY = y / (float)height; // 当前行的归一化高度 (0~1)
                float arrowWidth = (1 - normalizedY) * (width / 2f); // 顶部宽度逐渐收缩

                for (int x = 0; x < width; x++)
                {
                    // 计算箭头形状范围
                    float startX = centerX - arrowWidth;
                    float endX = centerX + arrowWidth;

                    if (x >= startX && x <= endX)
                    {
                        // 将原始坐标旋转
                        float rotatedX = Mathf.Cos(radAngle) * (x - centerX) - Mathf.Sin(radAngle) * (y - centerY) + centerX;
                        float rotatedY = Mathf.Sin(radAngle) * (x - centerX) + Mathf.Cos(radAngle) * (y - centerY) + centerY;

                        // 将旋转后的坐标映射回像素数组
                        int pixelX = Mathf.RoundToInt(rotatedX);
                        int pixelY = Mathf.RoundToInt(rotatedY);

                        // 检查像素是否在纹理范围内
                        if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height)
                        {
                            int index = pixelY * width + pixelX;
                            pixels[index] = arrowColor;
                        }
                    }
                }
            }

            // 设置纹理的像素
            arrowTexture.SetPixels(pixels);

            // 应用像素更改
            arrowTexture.Apply();
            return arrowTexture;
        }
        public void DrawArrow(Rect position, Color arrowColor, float angle = 0)
        {
            int width = (int)position.width;
            int height = (int)position.height;
            Texture2D arrowTexture = CreateArrowTexture(arrowColor, width, height, angle);
            GUI.DrawTexture(position, arrowTexture);
            Object.DestroyImmediate(arrowTexture);
        }
    }
}