using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO make this class as singletion.
public class MapDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private Tile _wallTile;

    [SerializeField] private Tile _destructibleTile;

    [SerializeField] private GameObject _explosionPrefab;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Explode(Vector2 worldPos)
    {
        Vector3Int originCell = _tilemap.WorldToCell(worldPos);
        int explosionDistance = 3;
        bool[] isCollisionInDirection = new bool[4] {false, false, false, false};

        ExplodeCell(originCell);
        for (int i = 1; i <= explosionDistance; i++)
        {
            if (!isCollisionInDirection[0] && !ExplodeCell(originCell + new Vector3Int(i, 0, 0)))
                break;
            if (!isCollisionInDirection[1] && !ExplodeCell(originCell + new Vector3Int(0, i, 0)))
                break;
            if (!isCollisionInDirection[2] && !ExplodeCell(originCell + new Vector3Int(-i, 0, 0)))
                break;
            if (!isCollisionInDirection[3] && !ExplodeCell(originCell + new Vector3Int(0, -i, 0)))
                break;
        }
    }

    private bool ExplodeCell(Vector3Int cell)
    {
        Tile tile = _tilemap.GetTile<Tile>(cell);
/*
		if (tile == _wallTile)
			return false;
*/
        if (tile == _destructibleTile)
        {
            // Remove the tile.
            _tilemap.SetTile(cell, null);
        }

        // Create an explosion.
        Vector3 pos = _tilemap.GetCellCenterWorld(cell);
        GameObject explosion = (GameObject) Instantiate(_explosionPrefab, pos, Quaternion.identity);
        
        // Destroy the explosion after animation.
        Destroy(explosion, _explosionPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);
        
        return true;
    }
}