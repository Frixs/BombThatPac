using Items;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO make this class as singletion.
namespace Managers
{
    public class MapManager : MonoBehaviour
    {
        /// <summary>
        /// Reference of wall tile.
        /// </summary>
        [SerializeField] private TileBase _wallTile = null;

        /// <summary>
        /// Reference of detructable tile.
        /// </summary>
        [SerializeField] private Tile _destructibleTile = null;
        
        /// <summary>
        /// Reference to the gameplay tilemap.
        /// </summary>
        [SerializeField] private Tilemap _tilemapGameplay;
        
        public Tilemap TilemapGameplay
        {
            get { return _tilemapGameplay; }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool ExplodeInCell(Vector3Int cell)
        {
            TileBase tile = _tilemapGameplay.GetTile<TileBase>(cell);

            if (tile == _wallTile)
                return false;

            if (tile == _destructibleTile)
            {
                // Remove the tile.
                _tilemapGameplay.SetTile(cell, null);
            }

            // Create an explosion.
            Vector3 pos = _tilemapGameplay.GetCellCenterWorld(cell);
            GameObject explosion = (GameObject) Instantiate(FindObjectOfType<Bomb>().ExplosionPrefab, pos, Quaternion.identity);
        
            // Destroy the explosion after animation.
            Destroy(explosion, FindObjectOfType<Bomb>().ExplosionPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);
        
            return true;
        }
    }
}