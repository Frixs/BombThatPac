using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;

namespace Items
{
    public class BananaPeel : MonoBehaviour
    {
        /// <summary>
        /// Caster of the Banana peel.
        /// </summary>
        [HideInInspector] public Player Caster;

        /// <summary>
        /// Scriptable status effect for slide debuff.
        /// </summary>
        [SerializeField] private ScriptableStatusEffect _slideStatusEffect;

        /// <summary>
        /// Spawn game object BananaPeel.
        /// </summary>
        /// <param name="prefab">Prefab representing this object.</param>
        /// <param name="p">Position to spawn.</param>
        /// <param name="r">ROtation on spawn.</param>
        /// <param name="caster">Caster of the object.</param>
        public static void Spawn(GameObject prefab, Vector3 p, Quaternion r, Player caster)
        {
            if (prefab == null || caster == null)
            {
                Debug.unityLogger.Log(LogType.Error, "Missing parameter!");
                return;
            }
            
            BananaPeel obj = Instantiate(prefab, p, r).GetComponent<BananaPeel>();
            obj.Caster = caster;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Component component = null;

            if ((component = other.gameObject.GetComponent<Player>()) != null && ((Player) component).Identifier != Caster.Identifier)
            {
                StatusEffectManager.Instance.ApplyStatusEffect((Player) component, Caster, _slideStatusEffect);
                Destroy(gameObject);
            }
        }
    }
}