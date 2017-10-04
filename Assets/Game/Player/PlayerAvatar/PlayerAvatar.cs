using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Player.PlayerAvatar
{
    public class PlayerAvatar : MonoBehaviour
    {
        public ReactiveProperty<PlayerState> PlayerState = new ReactiveProperty<PlayerState>();
        private CompositeDisposable PlayerStateClear = new CompositeDisposable();

        public Image Icon;
	    // Use this for initialization
	    void Start () {
            PlayerState.Do(ps =>
            {
                PlayerStateClear.Dispose();
                PlayerStateClear = new CompositeDisposable();
                PlayerStateClear.AddTo(this);
            }).Where(ps => ps != null).Subscribe(ps =>
            {
                ps.AvatarProperty.StartWith(ps.Avatar).Subscribe(a =>
                {
                    Icon.sprite = GameManager.GameConfig.CampaignConfig.PlayerAvatars[a].Avatar;
                }).AddTo(PlayerStateClear);

            }).AddTo(this);
	    }
    }
}
