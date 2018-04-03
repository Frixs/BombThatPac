using System;
using Characters;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;

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
        /// Reference to player panel.
        /// </summary>
        [HideInInspector] public PlayerPanel PlayerPanelReference;
        
        /// <summary>
        /// The number of wins this player has so far.
        /// </summary>
        [HideInInspector] public int Wins;

        /// <summary>
        /// Pass all necessary parameters to Player scripts (like hotkeys etc.).
        /// </summary>
        /// <param name="initOrderNumber">Initialized order number of a player. 0 = The first player has been initialized. 1 = the 2nd player has been initialized. etc.</param>
        public void Setup(int initOrderNumber)
        {
            // Assign player a new player object.
            Player.PlayerManagerReference = this;
            
            // Setup player's controls according to type of the game.
            switch (GameManager.Instance.CurrentGameType)
            {
                case GameType.LOCAL:
                    Player.InputPlayerSection = "Player"+ Player.Identifier;
                    break;
            }

            // Setup player UI.
            PlayerPanelReference = UserInterfaceGameplayManager.Instance.InstantiatePlayerPanel();
            PlayerPanelReference.PlayerManagerReference = this;
            // Set player colors.
            PlayerPanelReference.PlayerInventory.GetComponent<Image>().color = PlayerColor;
            // Set default scale & position.
            PlayerPanelReference.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            PlayerPanelReference.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                10f + initOrderNumber * (10f + PlayerPanelReference.GetComponent<RectTransform>().rect.width),
                10f,
                0f
            );
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
