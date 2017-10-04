using cac.Game.Campaign;
using cac.Game.Common.UI.Scripts;
using cac.Game.Player.PlayerAvatar;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.MainMenu.CampaignMenu
{
    public class ContinueGameTile : GameTile
    {
        public ReactiveProperty<CampaignState> CampaignState = new ReactiveProperty<CampaignState>();

        public Text ManaText;
        public Text LastPlayed;
        public Text TimeSpentText;

        public Button DeleteButton;

        public GameObject DeleteConfirm;
        public Button DeleteConfirmOk;
        public Button DeleteConfirmCancel;

        public override void Start()
        {
            base.Start();

            CampaignState.Subscribe(cs =>
            {
                if (cs != null)
                {
                    GetComponent<PlayerAvatar>().PlayerState.Value = cs.PlayerState;
                    ManaText.text = cs.PlayerState.TotalMana.ToString();
                    LastPlayed.text = cs.LastPlayed.ToShortDateString();
                    TimeSpentText.text = string.Format("{0}h {1}m {2}s", cs.PlayTime.Days * 24 + cs.PlayTime.Hours, cs.PlayTime.Minutes, cs.PlayTime.Seconds);
                }
                else
                {
                    GetComponent<PlayerAvatar>().PlayerState.Value = null;
                }
            }).AddTo(this);

            DeleteButton.onClick.AsObservable()
                .Subscribe(_ => DeleteConfirm.SetActive(true))
                .AddTo(this);
            DeleteConfirmCancel.onClick.AsObservable()
                .Subscribe(_ => HideConfirm())
                .AddTo(this);
        }

        public void HideConfirm()
        {
            if (DeleteConfirm.activeSelf)
            {
                TransitionHelper.TransitionOut(DeleteConfirm, () => DeleteConfirm.SetActive(false));
            }
        }
    }
}
