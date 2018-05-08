using System.Collections;
using Managers;
using UnityEngine;

namespace UI.MainMenu
{
    public class CharacterSelectionMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to character animation preview of player 1.
        /// </summary>
        [Header("Settings")] [SerializeField] private GameObject _previewP1;
        
        /// <summary>
        /// Reference to character animation preview of player 2.
        /// </summary>
        [SerializeField] private GameObject _previewP2;

        /// <summary>
        /// Background reference in main menu.
        /// </summary>
        [SerializeField] private MainMenuBackground _backgroundMainMenu;
        
        /// <summary>
        /// All character skins for players.
        /// </summary>
        [SerializeField] private RuntimeAnimatorController[] _characterSkinList;
        
        /// <summary>
        /// All character preview skins for player selection.
        /// </summary>
        [SerializeField] private RuntimeAnimatorController[] _characterPreviewSkinList;
        
        /// <summary>
        /// Sound on start click.
        /// </summary>
        [Header("Music Settings")] public AudioClip StartClickSfx;

        private void Start()
        {
            PlayerManager.PlayerCharacterSelection[0] = _characterSkinList[0];
            PlayerManager.PlayerCharacterSelection[1] = _characterSkinList[1];
        }

        private void Update()
        {
            if (Input.GetButtonDown(InputManager.Instance.GetButton("Player1", InputButtonType.MoveLeft)))
            {
                ChangeCharacterLeftButtonP1Event();
            }
            
            if (Input.GetButtonDown(InputManager.Instance.GetButton("Player1", InputButtonType.MoveRight)))
            {
                ChangeCharacterRightButtonP1Event();
            }
            
            if (Input.GetButtonDown(InputManager.Instance.GetButton("Player2", InputButtonType.MoveLeft)))
            {
                ChangeCharacterLeftButtonP2Event();
            }
            
            if (Input.GetButtonDown(InputManager.Instance.GetButton("Player2", InputButtonType.MoveRight)))
            {
                ChangeCharacterRightButtonP2Event();
            }
            
            if (Input.GetButtonDown(InputManager.Instance.GetButton("Player1", InputButtonType.Bomb)) || Input.GetButtonUp(InputManager.Instance.GetButton("Player2", InputButtonType.Bomb)))
            {
                StartButtonEvent();
            }
        }

        /// <summary>
        /// Button event method.
        /// </summary>
        public void StartButtonEvent()
        {
            _backgroundMainMenu.StartAnimation();

            Invoke("PlayStartSound", 0.8f);
            Invoke("StartTheGame", 1.5f);
            
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        private void StartTheGame()
        {
            SceneLoadingManager.Instance.LoadScene(1);
        }

        /// <summary>
        /// Play start sound.
        /// </summary>
        private void PlayStartSound()
        {
            SoundManager.Instance.PlaySingleSfx(StartClickSfx);
        }

        /// <summary>
        /// Button event method.
        /// </summary>
        public void ChangeCharacterLeftButtonP1Event()
        {
            int newIndex;
            for (int i = 0; i < _characterPreviewSkinList.Length; i++)
            {
                if (_characterPreviewSkinList[i] == _previewP1.GetComponent<Animator>().runtimeAnimatorController)
                {
                    newIndex = i - 1 >= 0 ? i - 1 : _characterPreviewSkinList.Length - 1;
                    if (_characterPreviewSkinList[newIndex] == _previewP2.GetComponent<Animator>().runtimeAnimatorController)
                    {
                        newIndex = newIndex - 1 >= 0 ? newIndex - 1 : _characterPreviewSkinList.Length - 1;
                    }
                    
                    _previewP1.GetComponent<Animator>().runtimeAnimatorController = _characterPreviewSkinList[newIndex];
                    PlayerManager.PlayerCharacterSelection[0] = _characterSkinList[newIndex];

                    break;
                }
            }
        }
        
        /// <summary>
        /// Button event method.
        /// </summary>
        public void ChangeCharacterRightButtonP1Event()
        {
            int newIndex;
            for (int i = 0; i < _characterPreviewSkinList.Length; i++)
            {
                if (_characterPreviewSkinList[i] == _previewP1.GetComponent<Animator>().runtimeAnimatorController)
                {
                    newIndex = i + 1 >= _characterPreviewSkinList.Length ? 0 : i + 1;
                    if (_characterPreviewSkinList[newIndex] == _previewP2.GetComponent<Animator>().runtimeAnimatorController)
                    {
                        newIndex = newIndex + 1 >= _characterPreviewSkinList.Length ? 0 : newIndex + 1;
                    }
                    
                    _previewP1.GetComponent<Animator>().runtimeAnimatorController = _characterPreviewSkinList[newIndex];
                    PlayerManager.PlayerCharacterSelection[0] = _characterSkinList[newIndex];

                    break;
                }
            }
        }
        
        /// <summary>
        /// Button event method.
        /// </summary>
        public void ChangeCharacterLeftButtonP2Event()
        {
            int newIndex;
            for (int i = 0; i < _characterPreviewSkinList.Length; i++)
            {
                if (_characterPreviewSkinList[i] == _previewP2.GetComponent<Animator>().runtimeAnimatorController)
                {
                    newIndex = i - 1 >= 0 ? i - 1 : _characterPreviewSkinList.Length - 1;
                    if (_characterPreviewSkinList[newIndex] == _previewP1.GetComponent<Animator>().runtimeAnimatorController)
                    {
                        newIndex = newIndex - 1 >= 0 ? newIndex - 1 : _characterPreviewSkinList.Length - 1;
                    }
                    
                    _previewP2.GetComponent<Animator>().runtimeAnimatorController = _characterPreviewSkinList[newIndex];
                    PlayerManager.PlayerCharacterSelection[1] = _characterSkinList[newIndex];

                    break;
                }
            }
        }
        
        /// <summary>
        /// Button event method.
        /// </summary>
        public void ChangeCharacterRightButtonP2Event()
        {
            int newIndex;
            for (int i = 0; i < _characterPreviewSkinList.Length; i++)
            {
                if (_characterPreviewSkinList[i] == _previewP2.GetComponent<Animator>().runtimeAnimatorController)
                {
                    newIndex = i + 1 >= _characterPreviewSkinList.Length ? 0 : i + 1;
                    if (_characterPreviewSkinList[newIndex] == _previewP1.GetComponent<Animator>().runtimeAnimatorController)
                    {
                        newIndex = newIndex + 1 >= _characterPreviewSkinList.Length ? 0 : newIndex + 1;
                    }
                    
                    _previewP2.GetComponent<Animator>().runtimeAnimatorController = _characterPreviewSkinList[newIndex];
                    PlayerManager.PlayerCharacterSelection[1] = _characterSkinList[newIndex];

                    break;
                }
            }
        }
    }
}