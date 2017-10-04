using UnityEngine;
using System.Collections;
using System;
using UniRx;
using cac.Game.Player;
using System.Runtime.Serialization;
using cac.Game.Scenes;

namespace cac.Game.Campaign
{
    [Serializable]
    public class CampaignState
    {
        public ReactiveProperty<PlayerState> PlayerStateProperty = new ReactiveProperty<PlayerState>();
        public PlayerState PlayerState
        {
            get { return PlayerStateProperty.Value; }
            set { PlayerStateProperty.Value = value; }
        }

        public ReactiveProperty<Difficulty.Level> DifficultyLevelProperty = new ReactiveProperty<Difficulty.Level>();
        public Difficulty.Level DifficultyLevel
        {
            get { return DifficultyLevelProperty.Value; }
            set { DifficultyLevelProperty.Value = value; }
        }
        
        public ReactiveProperty<bool> TutorialsProperty = new ReactiveProperty<bool>();
        public bool Tutorials
        {
            get { return TutorialsProperty.Value; }
            set { TutorialsProperty.Value = value; }
        }

        public ReactiveProperty<bool> PermadeathProperty = new ReactiveProperty<bool>();
        public bool Permadeath
        {
            get { return PermadeathProperty.Value; }
            set { PermadeathProperty.Value = value; }
        }

        public DateTime CampaignStarted = DateTime.Now;
        [NonSerialized]
        public DateTime SessionStarted = DateTime.MinValue;
        public DateTime LastPlayed = DateTime.MinValue;
        public TimeSpan PlayTime = new TimeSpan();

        public SceneState CurrentScene;

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (SessionStarted != DateTime.MinValue)
            {
                PlayTime = PlayTime + DateTime.Now.Subtract(SessionStarted);
                LastPlayed = DateTime.Now;
            }
        }

        [OnSerialized()]
        internal void OnSerializedMethod(StreamingContext context)
        {
            SessionStarted = DateTime.MinValue;
        }
    }
}