using UnityEngine;
using System.Collections;
using System;
using UniRx;

namespace cac.Game.Player
{
    [Serializable]
    public class PlayerState
    {
        public ReactiveProperty<string> AvatarProperty = new ReactiveProperty<string>();
        public string Avatar
        {
            get { return AvatarProperty.Value; }
            set { AvatarProperty.Value = value; }
        }
        
        public ReactiveProperty<int> TotalManaProperty = new ReactiveProperty<int>();
        public int TotalMana
        {
            get { return TotalManaProperty.Value; }
            set { TotalManaProperty.Value = value; }
        }
    }
}