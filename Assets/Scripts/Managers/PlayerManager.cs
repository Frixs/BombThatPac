using System;
using Characters;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
        /// Data from character selection.
        /// TODO: It is better to remove this static in the future and make better solution how to handle these data.
        /// </summary>
        public static RuntimeAnimatorController[] PlayerCharacterSelection = new RuntimeAnimatorController[2];
        
        /// <summary>
        /// A reference to the instance of the player if it is created.
        /// </summary>
        [HideInInspector] public GameObject CharacterInstance;
        
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
        [HideInInspector] public Player PlayerComponent;
        
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
            PlayerComponent.PlayerManagerReference = this;
            
            // Setup player's controls according to type of the game.
            switch (GameManager.Instance.CurrentGameType)
            {
                case GameType.Local:
                    PlayerComponent.InputPlayerSection = "Player"+ (initOrderNumber + 1);
                    break;
            }

            InitializePlayerPanel(initOrderNumber);
            
            // Set player skin.
            if (PlayerCharacterSelection[initOrderNumber] != null)
            {
                PlayerComponent.SetDefaultAnimationController(PlayerCharacterSelection[initOrderNumber]);
                PlayerComponent.GetComponent<Animator>().runtimeAnimatorController = PlayerCharacterSelection[initOrderNumber];
            }
        }

        /// <summary>
        /// Completly deinitialize player game references.
        /// </summary>
        public void Deinitialize()
        {
            Object.Destroy(CharacterInstance);
                
            PlayerComponent = null;
            CharacterInstance = null;
            
            DestroyPlayerPanel();
        }

        /// <summary>
        /// Initialize player panel to UI.
        /// </summary>
        /// <param name="initOrderNumber">Initialized order number of a player. 0 = The first player has been initialized. 1 = the 2nd player has been initialized. etc.</param>
        private void InitializePlayerPanel(int initOrderNumber)
        {
            if (initOrderNumber == 0)
            {
                // Setup player UI.
                PlayerPanelReference = UserInterfaceGameplayManager.Instance.InstantiatePlayerPanel(false);
                PlayerPanelReference.PlayerManagerReference = this;
                // Set player colors.
                //PlayerPanelReference.PlayerInventory.GetComponent<Image>().color = PlayerColor;

                // Player 1.
                // Set default scale & position.
                PlayerPanelReference.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                PlayerPanelReference.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                    0f,
                    0f,
                    0f
                );
            }
            else if (initOrderNumber == 1)
            {
                // Setup player UI.
                PlayerPanelReference = UserInterfaceGameplayManager.Instance.InstantiatePlayerPanel(true);
                PlayerPanelReference.PlayerManagerReference = this;
                // Set player colors.
                //PlayerPanelReference.PlayerInventory.GetComponent<Image>().color = PlayerColor;

                // Player 2.
                // Set default scale & position.
                PlayerPanelReference.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                PlayerPanelReference.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                    -(PlayerPanelReference.GetComponent<RectTransform>().rect.width),
                    0f,
                    0f
                );
                
                PlayerPanelReference.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
                PlayerPanelReference.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            }
        }
        
        /// <summary>
        /// Destroy player panel from UI.
        /// </summary>
        private void DestroyPlayerPanel()
        {
            Object.Destroy(PlayerPanelReference.gameObject);
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
