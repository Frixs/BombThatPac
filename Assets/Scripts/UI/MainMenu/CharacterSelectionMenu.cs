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
        /// All character skins for players.
        /// </summary>
        [SerializeField] private RuntimeAnimatorController[] _characterSkinList;
        
        /// <summary>
        /// All character preview skins for player selection.
        /// </summary>
        [SerializeField] private RuntimeAnimatorController[] _characterPreviewSkinList;

        private void Start()
        {
            PlayerManager.PlayerCharacterSelection[0] = _characterSkinList[0];
            PlayerManager.PlayerCharacterSelection[1] = _characterSkinList[1];
        }

        /// <summary>
        /// Button event method.
        /// </summary>
        public void StartButtonEvent()
        {
            SceneLoadingManager.Instance.LoadScene(1);
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