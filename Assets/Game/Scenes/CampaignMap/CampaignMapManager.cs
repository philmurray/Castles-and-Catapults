using cac.Game.Common.UI.Scripts;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Screen;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.CampaignMap
{
    public class CampaignMapManager : SceneManager
    {
        public static CampaignMapManager Instance;

        public CampaignMapState CampaignMapState;
        public override SceneState State
        {
            get
            {
                return CampaignMapState;
            }
            set
            {
                CampaignMapState = (CampaignMapState)value;
            }
        }
        
        void Awake()
        {
            Instance = this;
        }

        public override void Start()
        {
            NextScene.Where(scene => !string.IsNullOrEmpty(scene)).Subscribe(s => GetComponent<FadeScreen>().OutConfig.SceneToLoad = s).AddTo(this);
            base.Start();
            
        }
    }
}
