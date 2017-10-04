using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace cac.Game.Common.UI.Scripts
{
    public class TransitionRx : MonoBehaviour
    {
        public BoolReactiveProperty Shown;
        // Use this for initialization
        void Start()
        {
            var activeChanged = Shown.Skip(1);
            activeChanged.Where(active => active).Subscribe(active => 
            {
                TransitionHelper.TransitionIn(gameObject);
            }).AddTo(this);

            activeChanged.Where(active => !active).TakeUntilDestroy(this).Subscribe(active =>
            {
                TransitionHelper.TransitionOut(gameObject);
            }).AddTo(this);

            if (Shown.Value) {
                TransitionHelper.TransitionIn(gameObject);
            }
        }
    }
}
