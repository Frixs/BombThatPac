using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ButtonAction : MonoBehaviour
    {
        /// <summary>
        /// Reference to text label.
        /// </summary>
        public TMP_Text LabelText;

        /// <summary>
        /// Normal state gradient.
        /// </summary>
        [SerializeField] private TMP_ColorGradient _normalState;
        
        /// <summary>
        /// Hover state gradient.
        /// </summary>
        [SerializeField] private TMP_ColorGradient _hoverState;
        
        /// <summary>
        /// Active state gradient.
        /// </summary>
        [SerializeField] private TMP_ColorGradient _activeState;
        
        /// <summary>
        /// Sound on button click.
        /// </summary>
        [Header("Music Settings")] public AudioClip ButtonClickSfx;
        
        /// <summary>
        /// Sound on button click.
        /// </summary>
        public AudioClip ButtonHoverSfx;
        
        public void EventOnMouseEnter()
        {
            LabelText.colorGradientPreset = _hoverState;
            
            SoundManager.Instance.PlaySingleSfx(ButtonHoverSfx);
        }

        public void EventOnMouseExit()
        {
            LabelText.colorGradientPreset = _normalState;
        }
        
        public void EventOnMouseKeyUp()
        {
            LabelText.colorGradientPreset = _normalState;
        }
        
        public void EventOnMouseKeyDown()
        {
            LabelText.colorGradientPreset = _activeState;
            
            SoundManager.Instance.PlaySingleSfx(ButtonClickSfx);
        }
    }
}