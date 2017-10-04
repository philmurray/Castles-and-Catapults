using UnityEngine;
using System.Collections;
using System;
using UniRx;
using cac.Game.Player;

namespace cac.Game.Campaign
{
    [Serializable]
    public class Difficulty
    {
        public enum Level {
            Easy,
            Medium,
            Hard
        }
        public Level DifficultyLevel;
    }
}