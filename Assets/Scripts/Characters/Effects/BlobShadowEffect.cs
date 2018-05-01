using UnityEngine;

namespace Characters.Effects
{
    public class BlobShadowEffect : MonoBehaviour
    {
        /// <summary>
        /// New offset of the shadow.
        /// </summary>
        [Header("Settings")] public Vector3 Offset;
        
        /// <summary>
        /// Default offset.
        /// </summary>
        private Vector3 _offsetDefault = new Vector3(0f, -0.4f, 0f);

        /// <summary>
        /// Final offset which is applied.
        /// </summary>
        private Vector3 _appliedOffset;

        /// <summary>
        /// Sprite of the shadow.
        /// </summary>
        public Sprite ShadowSprite;
        
        /// <summary>
        /// Material of the shadow.
        /// </summary>
        public Material ShadowMaterial;

        /// <summary>
        /// Shadow object reference.
        /// </summary>
        private GameObject _shadow;

        private void Start()
        {
            _appliedOffset = Offset != Vector3.zero ? Offset : _offsetDefault;
            
            _shadow = new GameObject("Shadow");
            _shadow.transform.parent = transform;

            _shadow.transform.localPosition = _appliedOffset;
            _shadow.transform.localRotation = new Quaternion(0.8f, 0f, 0f, Quaternion.identity.w);

            SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer sr = _shadow.AddComponent<SpriteRenderer>();
            sr.sprite = ShadowSprite == null ? myRenderer.sprite : ShadowSprite;
            sr.material = ShadowMaterial;
            
            // Make sure that shadow is always rendered behind the object.
            sr.sortingLayerName = myRenderer.sortingLayerName;
            sr.sortingOrder = myRenderer.sortingOrder - 1;
        }

        private void LateUpdate()
        {
            _appliedOffset = Offset != Vector3.zero ? Offset : _offsetDefault;
            _shadow.transform.localPosition = _appliedOffset;
        }
    }
}