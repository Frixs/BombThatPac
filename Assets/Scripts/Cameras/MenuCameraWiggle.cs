using Managers;
using UnityEngine;

namespace Cameras
{
    public class MenuCameraWiggle : MonoBehaviour
    {
        Vector3 _origin;
        Vector3 _target;
        float ratio = 0.001f;
        
        /// <summary>
        /// This is the default background music on the game.
        /// </summary>
        [Header("Music Settings")] public AudioClip MainMenuModeMusic;

        void Start()
        {
            _origin = transform.position;
            InvokeRepeating("ChangeTarget", 0.01f, 2.0f);
            
            // Start playing game music loop.
            SoundManager.Instance.PlayNewBackgroundMusic(MainMenuModeMusic);
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _target, ratio);
        }

        void ChangeTarget()
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            _target = new Vector3(_origin.x + x, _origin.y + y, _origin.z);
        }
    }
}