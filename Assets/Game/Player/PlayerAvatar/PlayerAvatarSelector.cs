using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Player.PlayerAvatar
{
    public class PlayerAvatarSelector : MonoBehaviour
    {
        public ReactiveProperty<PlayerState> PlayerState = new ReactiveProperty<PlayerState>();
        public CompositeDisposable PlayerStateClear = new CompositeDisposable();

        public Button AvatarRight;
        public Button AvatarLeft;

	    // Use this for initialization
	    void Start () {

            PlayerState.Do(ps => {
                PlayerStateClear.Dispose();
                PlayerStateClear = new CompositeDisposable();
                PlayerStateClear.AddTo(this);
            }).Where(ps => ps != null).Subscribe(ps => {

                AvatarRight.onClick.AsObservable().Select(p => 1)
                .Merge(AvatarLeft.onClick.AsObservable().Select(p => -1))
                .Subscribe(pos => {
                    var list = GameManager.GameConfig.CampaignConfig.PlayerAvatarList;
                    var currentIndex = list.IndexOf(GameManager.GameConfig.CampaignConfig.PlayerAvatars[PlayerState.Value.Avatar]);
                    var nextIndex = currentIndex + pos;

                    if (nextIndex < 0)
                    {
                        nextIndex = list.Count - 1;
                    }
                    if (nextIndex >= list.Count)
                    {
                        nextIndex = 0;
                    }

                    PlayerState.Value.Avatar = list[nextIndex].Name;
                }).AddTo(PlayerStateClear);

            }).AddTo(this);
        }
    }
}
