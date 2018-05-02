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
        
        public void EventOnMouseEnter()
        {
            LabelText.colorGradientPreset = _hoverState;
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
        }
    }
}