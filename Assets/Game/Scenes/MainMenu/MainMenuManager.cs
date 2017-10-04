using cac.Game.Common.UI.Scripts;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Screen;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cac.Game.Scenes.MainMenu
{
    public class MainMenuManager : SceneManager
    {
        public static MainMenuManager Instance;

        public MainMenuState MainMenuState;
        public override SceneState State
        {
            get
            {
                return MainMenuState;
            }
            set
            {
                MainMenuState = (MainMenuState)value;
            }
        }

        public GameObject Wheels;

        public List<TransitionRx> Menus;
        public IntReactiveProperty ActiveMenu;

        void Awake()
        {
            Instance = this;
        }
        
        public override void Start()
        {
            NextScene.Where(scene => !string.IsNullOrEmpty(scene)).Subscribe(s => GetComponent<FadeScreen>().OutConfig.SceneToLoad = s).AddTo(this);

            base.Start();

            ActiveMenu.Subscribe(active => 
            {
                if (active == 0)
                {
                    TransitionHelper.TransitionIn(Wheels);
                }
                else
                {
                    TransitionHelper.TransitionOut(Wheels);
                }
                Menus.ForEach(m => m.Shown.Value = false);
                Menus[active].Shown.Value = true;
            }).AddTo(this);
        }
    }
}
