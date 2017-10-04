using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace cac.Game.Campaign
{
    [Serializable]
    public class CampaignConfig
    {
        public List<AvatarObject> PlayerAvatarList;

        private Dictionary<string, AvatarObject> _playerAvatars;
        public Dictionary<string, AvatarObject> PlayerAvatars
        {
            get
            {
                if (_playerAvatars == null)
                {
                    _playerAvatars = new Dictionary<string, AvatarObject>();
                    PlayerAvatarList.ForEach(a => _playerAvatars.Add(a.Name, a));
                }
                return _playerAvatars;
            }
        }

        [Serializable]
        public class AvatarObject
        {
            public string Name;
            public Sprite Avatar;
        }

        public bool DefaultTutorials;
        public bool DefaultPermadeath;
        public Difficulty.Level DefaultDifficulty;
        public List<Difficulty> DifficultyLevelsList;

        private Dictionary<Difficulty.Level, Difficulty> _difficultyLevels;
        public Dictionary<Difficulty.Level, Difficulty> DifficultyLevels
        {
            get
            {
                if (_difficultyLevels == null)
                {
                    _difficultyLevels = new Dictionary<Difficulty.Level, Difficulty>();
                    DifficultyLevelsList.ForEach(a => _difficultyLevels.Add(a.DifficultyLevel, a));
                }
                return _difficultyLevels;
            }
        }

        public int PlayerStartingMana;

    }
}