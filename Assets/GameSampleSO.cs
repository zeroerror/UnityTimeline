#if UNITY_EDITOR
using Game.GameEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "GameSampleSO", menuName = "游戏模板SO", order = 1)]
public class GameSampleSO : ScriptableObject
{
#if UNITY_EDITOR
    public AnimationClip animClip;
    public GameTimeline timeline = new GameTimeline();
#endif
}

public class GameTimelineTrack_Effect : GameTimelineTrack
{
    public GameTimelineTrack_Effect()
    {
        this.trackName = "特效";
        this.trackIndex = 0;
    }
    protected override void _OnUpdate(float newTime)
    {
    }
}

public class GameTimelineTrack_Action : GameTimelineTrack
{
    public GameTimelineTrack_Action()
    {
        this.trackName = "行为";
        this.trackIndex = 1;
    }
    protected override void _OnUpdate(float newTime)
    {
    }
}

public class GameTimelineTrack_TimeScale : GameTimelineTrack
{
    public GameTimelineTrack_TimeScale()
    {
        this.trackName = "时间缩放";
        this.trackIndex = 2;
    }
    protected override void _OnUpdate(float newTime)
    {
    }
}

public class GameTimelineTrack_Loop : GameTimelineTrack
{
    public GameTimelineTrack_Loop()
    {
        this.trackName = "时间循环";
        this.trackIndex = 3;
    }
    protected override void _OnUpdate(float newTime)
    {
    }
}

