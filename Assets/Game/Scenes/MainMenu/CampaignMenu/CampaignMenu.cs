using cac.Game.Campaign;
using cac.Game.Campaign.CampaignOptions;
using cac.Game.Common.UI.Scripts;
using cac.Game.Player;
using cac.Game.Scenes.CampaignMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.MainMenu.CampaignMenu
{
    public class CampaignMenu : MonoBehaviour
    {
        public Button ExitButton;
        public List<RectTransform> SaveGames;
        private Subject<int> SelectedIndex = new Subject<int>();

        public GameTile NewGameTile;
        public ContinueGameTile ContinueGameTile;

        public CampaignOptions CampaignOptions;
        public Button StartNewCampaignButton;

        private List<GameTile> gameTiles = new List<GameTile>();

        void Start()
        {
            ExitButton.onClick.AsObservable().Subscribe(_ => MainMenuManager.Instance.ActiveMenu.Value = 0).AddTo(this);
            GetComponent<TransitionRx>().Shown.Where(active => !active).Skip(1).Delay(new TimeSpan(0,0,2)).Subscribe(_ => {
                CampaignOptions.GetComponent<TransitionRx>().Shown.Value = false;
                gameTiles.ForEach(t => {
                    t.Selectable.Value = true;
                    t.Selected.Value = false;
                    var g = t as ContinueGameTile;
                    if (g != null)
                    {
                        g.HideConfirm();
                    }
                });
            }).AddTo(this);

            SelectedIndex.OnNext(-1);
            
            SelectedIndex.CombineLatest(StartNewCampaignButton.onClick.AsObservable(), (i, _) => i).Where(i => i >= 0).Subscribe(index =>
            {
                GameManager.GameState.CampaignIndex = index;
                GameManager.GameState.SavedCampaigns[index] = CampaignOptions.CampaignState.Value;
                GameManager.Instance.IsCampaignRunning = true;
                GameManager.Instance.SaveState();
            }).AddTo(this);

            for (int i = 0, l = SaveGames.Count; i < l; i++)
            {
                _createGameTile(i);
            }

        }

        private void _createGameTile(int i)
        {
            var spot = SaveGames[i];

            if (GameManager.GameState.SavedCampaigns[i] == null)
            {
                var gameTile = Instantiate(NewGameTile, spot);
                gameTiles.Add(gameTile);

                var sd1 = gameTile.GoButton.onClick.AsObservable().Select(_ => true)
                    .Merge(gameTile.CancelButton.onClick.AsObservable().Select(_ => false));

                sd1.Subscribe(active =>
                {
                    gameTile.Selected.Value = active;
                    gameTiles.ForEach(t => t.Selectable.Value = (t == gameTile) || !active);
                    CampaignOptions.GetComponent<TransitionRx>().Shown.Value = active;
                    if (active)
                    {
                        SelectedIndex.OnNext(i);
                        CampaignOptions.CampaignState.Value = new CampaignState
                        {
                            DifficultyLevel = GameManager.GameConfig.CampaignConfig.DefaultDifficulty,
                            Permadeath = GameManager.GameConfig.CampaignConfig.DefaultPermadeath,
                            PlayerState = new PlayerState
                            {
                                TotalMana = GameManager.GameConfig.CampaignConfig.PlayerStartingMana,
                                Avatar = GameManager.GameConfig.PlayerConfig.DefaultAvatar
                            },
                            Tutorials = GameManager.GameConfig.CampaignConfig.DefaultTutorials,
                            CurrentScene = new CampaignMapState()
                        };
                    }
                    else
                    {
                        SelectedIndex.OnNext(-1);
                        CampaignOptions.CampaignState.Value = null;
                    }
                }).AddTo(gameTile);
            }
            else
            {
                var gameTile = Instantiate(ContinueGameTile, spot);
                gameTiles.Add(gameTile);
                gameTile.CampaignState.Value = GameManager.GameState.SavedCampaigns[i];

                var sd1 = gameTile.DeleteButton.onClick.AsObservable().Select(_ => true)
                    .Merge(gameTile.DeleteConfirmCancel.onClick.AsObservable().Select(_ => false));

                sd1.Subscribe(active =>
                {
                    gameTile.Selected.Value = active;
                    gameTiles.ForEach(t => t.Selectable.Value = (t == gameTile) || !active);
                }).AddTo(gameTile);

                gameTile.DeleteConfirmOk.onClick.AsObservable().SubscribeWithState(i, (_, index) =>
                {
                    GameManager.GameState.SavedCampaigns[index] = null;
                    GameManager.Instance.SaveState();
                    Destroy(gameTiles[index].gameObject);
                    gameTiles.ForEach(t => t.Selectable.Value = true);
                    _createGameTile(index);
                }).AddTo(gameTile);

                gameTile.GoButton.onClick.AsObservable().SubscribeWithState(i, (_, index) =>
                {
                    GameManager.GameState.CampaignIndex = index;
                    GameManager.Instance.IsCampaignRunning = true;
                    
                }).AddTo(gameTile);
            }
        }
    }
}
