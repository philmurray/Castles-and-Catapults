using UnityEngine;
using System.Collections;
using System;

namespace cac.Game.Scenes.MainMenu
{
    [Serializable]
    public class MainMenuState : SceneState
    {
        public override string SceneName
        {
            get
            {
                return "MainMenu";
            }
        }
    }
}