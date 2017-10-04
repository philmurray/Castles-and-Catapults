using UnityEngine;
using System.Collections;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components;
using UniRx;

namespace cac.Game.Scenes
{
    public abstract class SceneManager : MonoBehaviour
    {
        public abstract SceneState State { get; set; }

        private TransitionManager _transitionManager;
        public TransitionManager TransitionManager
        {
            get
            {
                if (_transitionManager == null)
                {
                    _transitionManager = GetComponent<TransitionManager>();
                }
                return _transitionManager;
            }
        }

        public StringReactiveProperty NextScene = new StringReactiveProperty();

        public virtual void Start()
        {
            GameManager.Instance.SceneManager = this;
            if (GameManager.Instance.CurrentSceneState != null)
            {
                State = GameManager.Instance.CurrentSceneState;
            }
            NextScene.Where(scene => !string.IsNullOrEmpty(scene)).Subscribe(_ => TransitionManager.TransitionOut()).AddTo(this);
        }
    }
}
