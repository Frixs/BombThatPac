using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuBackground : MonoBehaviour
    {
        [SerializeField] private ImageAnimation _idleAnimation;
        [SerializeField] private ImageAnimation _startAnimation;
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Start play the game animation.
        /// </summary>
        public void StartAnimation()
        {
            _startAnimation.enabled = true;
            _idleAnimation.enabled = false;
        }
    }
}