using cac.Game.Campaign;
using cac.Game.Scenes;
using cac.Game.Scenes.MainMenu;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UniRx;
using UnityEngine;


namespace cac.Game
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;
        public static GameConfig GameConfig
        {
            get
            {
                return Instance.Config;
            }
        }
        public static GameState GameState
        {
            get
            {
                return Instance.State;
            }
        }


        public string StateFileLocation;

        public SceneManager SceneManager { get; set; }


        public ReactiveProperty<SceneState> CurrentSceneStateProperty = new ReactiveProperty<SceneState>();
        public SceneState CurrentSceneState
        { 
            get { return CurrentSceneStateProperty.Value; }
            set { CurrentSceneStateProperty.Value = value; }
        }

        public CampaignState CurrentCampaign
        {
            get
            {
                return GameState.SavedCampaigns[GameState.CampaignIndex];
            }
        }

        private GameState _state;
        private GameState State
        {
            get
            {
                if (_state == null)
                {
                    _state = LoadState();
                    if (_state == null)
                    {
                        _state = new GameState() { };
                        SaveState();
                    }
                }
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private GameConfig _config;
        private GameConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = GetComponent<GameConfig>();
                }
                return _config;
            }
        }


        public ReactiveProperty<bool> IsCampaignRunningProperty = new ReactiveProperty<bool>();
        public bool IsCampaignRunning
        {
            get { return IsCampaignRunningProperty.Value; }
            set { IsCampaignRunningProperty.Value = value; }
        }


        void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            CurrentSceneStateProperty.Where(css => css != null).Subscribe(css =>
            {
                if (IsCampaignRunning)
                {
                    CurrentCampaign.CurrentScene = css;
                }

                if (SceneManager != null && SceneManager.TransitionManager != null)
                {
                    SceneManager.NextScene.Value = css.SceneName;
                }
                else
                {
                    TransitionHelper.LoadScene(css.SceneName);
                }
            }).AddTo(this);

            IsCampaignRunningProperty.Subscribe(running => {
                if (running)
                {
                    CurrentCampaign.SessionStarted = DateTime.Now;
                    CurrentSceneState = CurrentCampaign.CurrentScene;
                }
                else
                {
                    CurrentSceneState = new MainMenuState();
                }
            }).AddTo(this);
        }

        private GameState LoadState()
        {
            if (File.Exists(Application.persistentDataPath + "/" + StateFileLocation))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + StateFileLocation, FileMode.Open);
                var saveGame = (GameState)bf.Deserialize(file);
                file.Close();

                return saveGame;
            }
            return null;
        }
        void OnApplicationQuit() {
            SaveState();
        }
        public void SaveState()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + StateFileLocation);
            bf.Serialize(file, State);
            file.Close();
            if (IsCampaignRunning) {
                CurrentCampaign.SessionStarted = DateTime.Now;
            }
        }
    }
}