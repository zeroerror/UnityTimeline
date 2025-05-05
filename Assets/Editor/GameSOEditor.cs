using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Config;

namespace Game.GameEditor
{
    [CustomEditor(typeof(GameSampleSO))]
    public class GameSOEditor : Editor
    {
        private SerializedObject _serializedObject;
        private SerializedProperty timeline_p;
        private SerializedProperty animClip_p;

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(target);
            timeline_p = _serializedObject.FindProperty("timeline");
            animClip_p = _serializedObject.FindProperty("animClip");
            EditorApplication.update += ForceInspectorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= ForceInspectorUpdate;
        }

        private void ForceInspectorUpdate()
        {
            if (this._serializedObject == null) return;
            const float deltaTime = 1.0f / 60;
            if (Time.realtimeSinceStartup - _cacheRealTime > deltaTime)
            {
                _cacheRealTime = Time.realtimeSinceStartup;
                Repaint();
            }
        }
        private float _cacheRealTime = 0;

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();
            EditorGUILayout.PropertyField(animClip_p);
            EditorGUILayout.PropertyField(timeline_p);
            if (GUILayout.Button("Create Default"))
            {
                _CreateDefault();
            }
            this._serializedObject.ApplyModifiedProperties();
        }

        private void _CreateDefault()
        {
            {
                var gameSO = (GameSampleSO)target;
                var animClip = gameSO.animClip;
                var animLength = animClip != null ? animClip.length : 0;
                var timeline = gameSO.timeline;
                timeline.animClip = animClip;
                timeline.length = animLength;
                var tracks = new List<GameTimelineTrack>();
                timeline.tracks = tracks;
                var track0 = new GameTimelineTrack_Effect(timeline)
                {
                    length = animLength,
                    fragments = new List<GameTimelineTrackFragment>{
                            new GameTimelineTrackFragment{
                                startTime = 0.1f,
                                endTime = 0.1666f,
                            },
                            new GameTimelineTrackFragment{
                                startTime = 0.3333f,
                                endTime =  0.5f,
                            },
                        }
                };
                var track1 = new GameTimelineTrack_Action(timeline)
                {
                    length = animLength,
                    fragments = new List<GameTimelineTrackFragment>{
                            new GameTimelineTrackFragment{
                                startTime = 0.2f,
                                endTime = 0.3f,
                            },
                            new GameTimelineTrackFragment{
                                startTime = 0.4f,
                                endTime = 0.5f,
                            },
                        }
                };
                var track2 = new GameTimelineTrack_TimeScale(timeline)
                {
                    length = animLength,
                    fragments = new List<GameTimelineTrackFragment>{
                            new GameTimelineTrackFragment{
                                startTime = 0.3f,
                                endTime = 1.5f,
                            },
                        }
                };
                var track3 = new GameTimelineTrack_Loop(timeline)
                {
                    length = animLength,
                    fragments = new List<GameTimelineTrackFragment>{
                            new GameTimelineTrackFragment{
                                startTime = 1.6f,
                                endTime = 1.9f,
                            },
                        }
                };
                tracks.Add(track0);
                tracks.Add(track1);
                tracks.Add(track2);
                tracks.Add(track3);
            }
        }
    }
}