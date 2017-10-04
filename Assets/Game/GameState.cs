using UnityEngine;
using System.Collections;
using System;
using cac.Game.Campaign;
using System.Collections.Generic;
using UniRx;

namespace cac.Game
{
    [Serializable]
    public class GameState
    {

        public ReactiveProperty<int> CampaignIndexProperty = new ReactiveProperty<int>();
        public int CampaignIndex
        {
            get { return CampaignIndexProperty.Value; }
            set { CampaignIndexProperty.Value = value; }
        }

        public List<CampaignState> SavedCampaigns = new List<CampaignState> { null, null, null };
    }
}
