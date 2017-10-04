using UnityEngine;
using System.Collections;
using System;

namespace cac.Game.Scenes.CampaignMap
{
    [Serializable]
    public class CampaignMapState : SceneState
    {
        public override string SceneName
        {
            get
            {
                return "CampaignMap";
            }
        }
    }
}