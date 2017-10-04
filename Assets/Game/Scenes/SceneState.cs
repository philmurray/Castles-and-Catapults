using UnityEngine;
using System.Collections;
using System;

namespace cac.Game.Scenes
{
    [Serializable]
    public abstract class SceneState
    {
        public virtual string SceneName
        {
            get
            {
                throw new System.Exception("Not Implemented");
            }
        }
    }
}
