using System;
using Characters;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// This class is to manage various settings on a player.
    /// 
    /// It works with the GameManager class to control how the tanks behave
    /// and whether or not players have control of their tank in the 
    /// different phases of the game.
    /// </summary>
    [Serializable]
    public class PlayerManager
    {
        /// <summary>
        /// A reference to the instance of the tank when it is created.
        /// </summary>
        [HideInInspector] public GameObject Instance;
        
        /// <summary>
        /// Name of the player.
        /// </summary>
        public string PlayerName;
        
        /// <summary>
        /// This is the color this tank will be tinted.
        /// </summary>
        public Color PlayerColor;
        
        /// <summary>
        /// Reference to player's movement script.
        /// </summary>
        [HideInInspector] public Player Player;
        
        /// <summary>
        /// The number of wins this player has so far.
        /// </summary>
        [HideInInspector] public int Wins;
    
        /// <summary>
        /// Pass all necessary parameters to Player scripts (like hotkeys etc.).
        /// </summary>
        public void Setup()
        {
            // Assign player a new player object.
            Player.PlayerManagerReference = this;
            
            // Setup player's controls according to type of the game.
            switch (GameManager.Instance.GameType)
            {
                case "LOCAL":
                    Player.InputPlayerSection = "Player"+ Player.Identifier;
                    break;
            }
        }
    
        /// <summary>
        /// Used at the start of each round to put the character into it's default state.
        /// </summary>
        public void Reset()
        {
        /*
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;
        
            m_Instance.SetActive (false);
            m_Instance.SetActive (true);
            */
        }
    }
}
