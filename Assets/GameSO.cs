#if UNITY_EDITOR
using Game.GameEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "GameSO", menuName = "游戏模板/GameSO", order = 1)]
public class GameSO : ScriptableObject
{
#if UNITY_EDITOR
    public AnimationClip animClip;
    public GameTimeline timeline = new GameTimeline();
#endif
}