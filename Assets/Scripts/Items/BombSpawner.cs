using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
	[SerializeField]
	private Tilemap _tilemap;

	[SerializeField]
	private GameObject _bombPrefab;
	
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int cell = _tilemap.WorldToCell(worldPos);
			Vector3 cellCenterPos = _tilemap.GetCellCenterWorld(cell);

			Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity);
		}
	}
}
