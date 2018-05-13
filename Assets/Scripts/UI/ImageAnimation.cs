using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Animation image UI component.
    /// </summary>
    public class ImageAnimation : MonoBehaviour
    {
        public Sprite[] Sprites;
        public int SpritePerFrame = 6;
        public bool Loop = true;
        public bool DestroyOnEnd = false;

        private int _index = 0;
        private Image _image;
        private int _frame = 0;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void Update()
        {
            if (!Loop && _index == Sprites.Length)
                return;
            
            _frame++;
            
            if (_frame < SpritePerFrame)
                return;
            
            _image.sprite = Sprites[_index];
            _frame = 0;
            _index++;
            
            if (_index >= Sprites.Length)
            {
                if (Loop) _index = 0;
                if (DestroyOnEnd) Destroy(gameObject);
            }
        }
    }
}