using ZeroError.Config;
using UnityEngine;

namespace ZeroError.Config
{
    [CreateAssetMenu(fileName = "GameSampleSO", menuName = "游戏模板SO", order = 1)]
    public class GameSampleSO : ScriptableObject
    {
#if UNITY_EDITOR
    public AnimationClip animClip;
    public GameTimeline_Skill timeline = new GameTimeline_Skill();
#endif
    }
}

