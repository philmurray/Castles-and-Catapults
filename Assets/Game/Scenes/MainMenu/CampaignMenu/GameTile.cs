using cac.Game.Common.UI.Scripts;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.MainMenu.CampaignMenu
{
    public class GameTile : MonoBehaviour
    {
        public Button GoButton;
        public Button CancelButton;

        public BoolReactiveProperty Selected = new BoolReactiveProperty(false);
        public BoolReactiveProperty Selectable = new BoolReactiveProperty(true);

        private TransitionRx transition;

        public virtual void Start()
        {
            transition = GetComponent<TransitionRx>();

            Selected.Subscribe(s =>
            {
                GoButton.gameObject.SetActive(!s);
                CancelButton.gameObject.SetActive(s);
            }).AddTo(this);
            Selectable.Subscribe(s =>
            {
                transition.Shown.Value = s;
                GoButton.interactable = s;
                CancelButton.interactable = s;
            }).AddTo(this);
        }
    }
}
