using cac.Game.Player.PlayerAvatar;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Campaign.CampaignOptions
{
    public class CampaignOptions : MonoBehaviour
    {
        public ReactiveProperty<CampaignState> CampaignState = new ReactiveProperty<CampaignState>();
        private CompositeDisposable CampaignStateClear = new CompositeDisposable();
        
        public PlayerAvatar Avatar;
        public PlayerAvatarSelector AvatarSelector;

        public Toggle EasyCampaign;
        public Toggle MediumCampaign;
        public Toggle HardCampaign;

        public Toggle Tutorials;
        public Toggle Permadeath;

        // Use this for initialization
        void Start()
        {
            CampaignState.Subscribe(cs => {
                CampaignStateClear.Dispose();
                CampaignStateClear = new CompositeDisposable();
                CampaignStateClear.AddTo(this);

                if (cs != null)
                {
                    Avatar.PlayerState.Value = cs.PlayerState;
                    AvatarSelector.PlayerState.Value = cs.PlayerState;

                    cs.DifficultyLevelProperty.Subscribe(d =>
                    {
                        EasyCampaign.isOn = d == Difficulty.Level.Easy;
                        MediumCampaign.isOn = d == Difficulty.Level.Medium;
                        HardCampaign.isOn = d == Difficulty.Level.Hard;
                    }).AddTo(CampaignStateClear);
                    cs.PermadeathProperty.Subscribe(p => Permadeath.isOn = p).AddTo(CampaignStateClear);
                    cs.TutorialsProperty.Subscribe(t => Tutorials.isOn = t).AddTo(CampaignStateClear);

                    EasyCampaign.onValueChanged.AsObservable().Where(c => c).Subscribe(_ => cs.DifficultyLevel = Difficulty.Level.Easy);
                    MediumCampaign.onValueChanged.AsObservable().Where(c => c).Subscribe(_ => cs.DifficultyLevel = Difficulty.Level.Medium);
                    HardCampaign.onValueChanged.AsObservable().Where(c => c).Subscribe(_ => cs.DifficultyLevel = Difficulty.Level.Hard);

                    Tutorials.onValueChanged.AsObservable().Subscribe(t => cs.Tutorials = t);
                    Permadeath.onValueChanged.AsObservable().Subscribe(t => cs.Permadeath = t);
                }
                else
                {
                    Avatar.PlayerState.Value = null;
                    AvatarSelector.PlayerState.Value = null;
                }
            }).AddTo(this);
        }
    }
}