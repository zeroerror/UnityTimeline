using UnityEngine;

namespace Game.GameEditor
{
    public static class GameTimelineGUIConfig
    {
        public static readonly float bgPadding_Hor = 10;
        public static readonly float bgPadding_Ver = 10;
        public static float bgWidth => labelW + trackW + bgPadding_Hor * 2;

        public static readonly float trackW = 500;
        public static readonly float trackH = 25;
        public static readonly float rowHeight = 27;
        public static readonly Color trackColor = new Color(42 / 255.0f, 42 / 255.0f, 42 / 255.0f, 1);
        public static readonly Color trackLineColor = new Color(0.5f, 0.5f, 0.5f, 1);
        public static readonly Color trackLabelColor = new Color(0.2f, 0.2f, 0.2f, 1);

        public static readonly float labelW = 200;
        public static readonly float labelLeftPadding = 5;

        public const float fragmentH = 20;
        public const float fragmentBorderPadding = 4;
        public static readonly Color fragmentBorderColor = new Color(0.0f, 0.5f, 0.5f, 1);

        public static readonly Color bgColor = new Color(25 / 255.0f, 25 / 255.0f, 25 / 255.0f);
        public static readonly Color textColor = new Color(0.0f, 0.5f, 0.5f);
        public static readonly Color transparentColor = new Color(1.0f, 0.0f, 0.0f, 0.0f);
    }
}