﻿using System.Collections;
using System.Collections.Generic;
using Characters;
using Items.SpecialItems;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// This class controlls all possible spawn on the map.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of SpawnManager which allows it to be accessed by any other script.
        /// </summary>
        public static SpawnManager Instance = null;

        /// <summary>
        /// Spawner which throw the items to its spawning positions.
        /// </summary>
        [HideInInspector] public GameObject ItemSpawner;

        /// <summary>
        /// Drop chance of special case - nothing.
        /// </summary>
        [Header("Item Spawning")] public float DropChanceOfNothing;

        /// <summary>
        /// Spawn item on each tick.
        /// </summary>
        [SerializeField] private float _spawnItemEachTick;

        /// <summary>
        /// Delay before launching the item.
        /// </summary>
        [SerializeField] private float _itemLaunchDelay;

        /// <summary>
        /// List of all possible items to be able to spawn.
        /// </summary>
        [SerializeField] private SpecialItem[] _spawnableItemList;

        /// <summary>
        /// Total weight of all spawnable items from the list.
        /// </summary>
        private float _totalItemWeight;

        /// <summary>
        /// Timer to spawn item.
        /// </summary>
        private float _itemSpawnTimer;

        // Awake is always called before any Start functions
        void Awake()
        {
            // Check if instance already exists.
            if (Instance == null)
            {
                // If not, set instance to this.
                Instance = this;
            }
            // If instance already exists and it's not this.
            else if (Instance != this)
            {
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a SpawnManager.
                Destroy(gameObject);
            }

            // Sets this to not be destroyed when reloading scene.
            //DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            ItemSpawner = GameManager.Instance.PacMan.gameObject;

            _totalItemWeight = CalculateItemWeight();
        }

        // Update is called once per frame
        void Update()
        {
            _itemSpawnTimer += Time.deltaTime;

            if (_itemSpawnTimer >= _spawnItemEachTick)
            {
                _itemSpawnTimer = 0f;
                SpawnRandomItem();
            }
        }

        /// <summary>
        /// Respawn the character.
        /// </summary>
        /// <param name="toRespawn">GameObject reference to respawn.</param>
        /// <param name="delay">Time to respawn.</param>
        /// <param name="positions">Spawnpoint positions, whre the object can possibly spawn.</param>
        /// <param name="spawnAnimation">Spawn animation reference.</param>
        public void RespawnCharacterInit(Character toRespawn, float delay, Transform[] positions, GameObject spawnAnimation)
        {
            if (toRespawn == null || positions.Length == 0)
            {
                Debug.unityLogger.Log(LogType.Error, "There is missing parameter to be able to respawn!");
                return;
            }

            StartCoroutine(RespawnCharacter(toRespawn, delay, positions, spawnAnimation));
        }

        /// <summary>
        /// Coroutine delayed respawn object.
        /// </summary>
        /// <param name="toRespawn">GameObject reference to respawn.</param>
        /// <param name="delay">Time to respawn.</param>
        /// <param name="positions">Spawnpoint positions, whre the object can possibly spawn.</param>
        /// <param name="spawnAnimation">Spawn animation reference.</param>
        /// <returns></returns>
        private IEnumerator RespawnCharacter(Character toRespawn, float delay, Transform[] positions, GameObject spawnAnimation)
        {
            yield return new WaitForSeconds(delay);

            toRespawn.transform.position = positions[Random.Range(0, positions.Length)].position;

            toRespawn.IsInvulnearable = true; // Make the target invulnerable despite receiving invulnerability as status effect. But we need to know it is invulnerable before set active.
            
            toRespawn.IsDeath = false;
            toRespawn.gameObject.SetActive(true);
            
            // If it is Player, set invulnerability on respawn.
            if (toRespawn is Player)
                StatusEffectManager.Instance.ApplyStatusEffect(toRespawn, null, ((Player) toRespawn).RespawnInvulStatusEffect);

            // Spawn animation.
            SpawnAnimationAtPositionWithExpiry(spawnAnimation, toRespawn.transform.position, Quaternion.identity);

            SoundManager.Instance.PlayRandomizeSfx(toRespawn.GetComponent<Character>().SpawnSfx);

            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Character has been respawned!", toRespawn.GetComponent<Character>().Name);
        }

        /// <summary>
        /// Spawn animation prefab at the current position and delete it after expiration.
        /// </summary>
        /// <param name="prefab">Prefab with animation to spawn.</param>
        /// <param name="animPosition">Position where to spawn the animation prefab.</param>
        /// <param name="animRotation">Rotation of the animation.</param>
        public void SpawnAnimationAtPositionWithExpiry(GameObject prefab, Vector3 animPosition, Quaternion animRotation)
        {
            GameObject anim = SpawnAnimationAtPosition(prefab, animPosition, animRotation);
            // Destroy the animation after expiration.
            Destroy(anim, prefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);
        }
        
        /// <summary>
        /// Spawn animation prefab at the current position.
        /// </summary>
        /// <param name="prefab">Prefab with animation to spawn.</param>
        /// <param name="animPosition">Position where to spawn the animation prefab.</param>
        /// <param name="animRotation">Rotation of the animation.</param>
        /// <returns>Return animation reference.</returns>
        public GameObject SpawnAnimationAtPosition(GameObject prefab, Vector3 animPosition, Quaternion animRotation)
        {
            if (prefab == null)
            {
                Debug.unityLogger.Log(LogType.Error, "There is missing prefab to be able to spawn that animation!");
                return null;
            }

            // Create an animation.
            return Instantiate(prefab, animPosition, animRotation);
        }

        /// <summary>
        /// Spawn animation following target object.
        /// </summary>
        /// <param name="prefab">Prefab with animation to spawn.</param>
        /// <param name="target">Target where to spawn the animation.</param>
        /// <param name="additionalPositioning">Additional positiong. You can move relative from the target.</param>
        /// <param name="animRotation">Rotation of the animation.</param>
        /// <returns>Spawned animation gameobject reference.</returns>
        public GameObject SpawnFollowingAnimationLoop(GameObject prefab, GameObject target, Vector3 additionalPositioning, Quaternion animRotation)
        {
            if (prefab == null || target == null)
            {
                Debug.unityLogger.Log(LogType.Error, "Missing parameters!");
                return null;
            }

            // Create an animation.
            GameObject anim = (GameObject) Instantiate(prefab, target.transform.position + additionalPositioning, animRotation);
            anim.transform.parent = target.transform;

            return anim;
        }

        /// <summary>
        /// Despawn (Destroy) animation.
        /// </summary>
        /// <param name="anim">Reference to gameobject animation.</param>
        /// <param name="delay">Delay after which the animation will be destroyed.</param>
        public void DespawnAnimation(GameObject anim, float delay)
        {
            if (anim == null)
                return;
            
            if (delay > 0f)
                Destroy(anim, delay);
            else
                Destroy(anim);
        }

        /// <summary>
        ///	Spawns random item from the list.
        /// </summary>
        public void SpawnRandomItem()
        {
            float rand = Random.Range(0f, _totalItemWeight);
            float top = 0f;

            for (int i = 0; i < _spawnableItemList.Length; i++)
            {
                top += _spawnableItemList[i].DropChance;
                if (rand < top)
                {
                    StartCoroutine(SpawnItem(_spawnableItemList[i]));
                    return;
                }
            }

            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] No item to spawn.", ItemSpawner.name);
        }

        /// <summary>
        /// Spawn item.
        /// </summary>
        /// <param name="item">Item to be spawned.</param>
        private IEnumerator SpawnItem(SpecialItem item)
        {
            List<Transform> possibleSpawns = new List<Transform>();

            // Find a free spawn point for the item.
            for (int i = 0; i < MapManager.Instance.ItemSpawnPoints.Length; i++)
            {
                Collider2D[] collectables = Physics2D.OverlapCircleAll(
                    new Vector2(
                        MapManager.Instance.ItemSpawnPoints[i].transform.position.x,
                        MapManager.Instance.ItemSpawnPoints[i].transform.position.y
                    ),
                    MapManager.Instance.TilemapCellHalfSize,
                    1 << LayerMask.NameToLayer(Constants.UserLayerNameCollectable)
                );

                if (collectables.Length == 0)
                    possibleSpawns.Add(MapManager.Instance.ItemSpawnPoints[i]);
            }

            if (possibleSpawns.Count == 0)
            {
                Debug.unityLogger.LogFormat(LogType.Log, "[Spawner: {0}] There is no free spawn point to spawn new item.", ItemSpawner.name);
                yield break;
            }

            // Animate throwing item.
            Component component = null;
            if ((component = ItemSpawner.GetComponent<PacMan>()) != null)
            {
                ((PacMan) component).StartEventAnimation(1);
                yield return new WaitForSeconds(0.4f); // Wait for the correct animation.
            }

            // Get random spawn point.
            int spawnPointIndex = Random.Range(0, possibleSpawns.Count - 1);

            SpecialItem spawnedItem = Instantiate(item, ItemSpawner.transform.position, item.transform.rotation);
            spawnedItem.StartThrowing(possibleSpawns[spawnPointIndex].position, 3f);
            Debug.unityLogger.LogFormat(LogType.Log, "[Spawner: {0}] Spawning item: {1}.", ItemSpawner.name, item);

            yield return null;
        }

        /// <summary>
        /// Calculate item weight to be able to recognize spawn chances.
        /// </summary>
        /// <returns>Weight of all spawnable items from the list.</returns>
        private float CalculateItemWeight()
        {
            float total = 0f;

            for (int i = 0; i < _spawnableItemList.Length; i++)
            {
                total += _spawnableItemList[i].DropChance;
            }

            if (total > 0f)
                total += DropChanceOfNothing;

            return total;
        }
    }
}