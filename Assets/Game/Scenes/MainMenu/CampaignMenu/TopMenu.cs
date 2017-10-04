using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.MainMenu.TopMenu
{
    public class TopMenu : MonoBehaviour
    {
        public Button ContinueButton;
        public Button CampaignButton;
        public Button MatchesButton;
        public Button OptionsButton;
        public Button ExitButton;

        // Use this for initialization
        void Start()
        {
            var matchesClick = MatchesButton.onClick.AsObservable();
            var optionsClick = OptionsButton.onClick.AsObservable();
            var exitClick = ExitButton.onClick.AsObservable();

            CampaignButton.onClick.AsObservable().Subscribe(_ => MainMenuManager.Instance.ActiveMenu.Value = 1).AddTo(this);

            if (GameManager.GameState.SavedCampaigns[GameManager.GameState.CampaignIndex] == null)
            {
                ContinueButton.gameObject.SetActive(false);
            }
            ContinueButton.onClick.AsObservable().Subscribe(_ => GameManager.Instance.IsCampaignRunning = true).AddTo(this);
        }
    }
}
