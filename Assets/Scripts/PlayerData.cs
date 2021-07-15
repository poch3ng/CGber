using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CGber
{
    public struct PlayerData
    {
        public string PlayerName { get; private set; }

        public PlayerData(string playerName)
        {
            PlayerName = playerName;
        }
    }

}
