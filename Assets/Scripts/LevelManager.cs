using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
	/// <summary>
	/// Map data fall all the map layers, grounds, walls, etc.
	/// </summary>
	[SerializeField]
	private Texture2D[] _mapData;

	/// <summary>
	/// A map element represents a tile that we can create in our game.
	/// </summary>
	[SerializeField]
	private MapElement[] _mapElements;
	
	/// <summary>
	/// This tile is used for measuring the distance between tiles.
	/// </summary>
	[SerializeField]
	private Sprite _defaultTile;

	/// <summary>
	/// Dictionary for all wall tiles.
	/// </summary>
	private Dictionary<Point, GameObject> wallTiles = new Dictionary<Point, GameObject>();

	/// <summary>
	/// TODO
	/// </summary>
	[SerializeField]
	private SpriteAtlas _wallAtlas;
	
	/// <summary>
	/// TODO
	/// </summary>
	private string[] _mazeTileSetTypes = new string[]
	{
		"GeneralMaze",
	};
	
	/// <summary>
	/// A Parent transform for our map, this will prevent our hierarchy to be flooded with tiles.
	/// </summary>
	[SerializeField]
	private Transform _map;
	
	/// <summary>
	/// The position of the bottom left corner of the screen.
	/// </summary>
	private Vector3 WorldStartPos
	{
		get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }
	}
	
	// Use this for initialization
	void Start()
	{
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// Generates our map.
	/// </summary>
	private void GenerateMap()
	{
		int i = 0, x = 0, y = 0;
		
		for (i = 0; i < _mapData.Length; i++) // Looks through all our map layers.
		{
			for (x = 0; x < _mapData[i].width; x++) // Runs through all pixels on the layer.
			{
				for (y = 0; y < _mapData[i].height; y++)
				{
					Color c = _mapData[i].GetPixel(x, y); // Gets the color of the current pixel.

					// Checks if we have a tile that suits the color of the pixel on the map.
					MapElement newElement = Array.Find(_mapElements, e => e.MyColor == c);

					if (newElement != null) // If we found an element with correct color.
					{
						// Calculate x and y position of the tile.
						float xPos = WorldStartPos.x + (_defaultTile.bounds.size.x * x);
						float yPos = WorldStartPos.y + (_defaultTile.bounds.size.y * y);
					
						// Create the tile.
						GameObject go = Instantiate(newElement.MyElementPrefab);
						
						// Set the tile's position.
						go.transform.position = new Vector2(xPos, yPos);
						
						// Check which element we are placing.
						switch (newElement.MyTileTag)
						{
							case "Wall":
								wallTiles.Add(new Point(x, y), go);
								break;
						}
						
						// Make the tile a child of map.
						go.transform.parent = _map;
					}
					
				}
			}
		}
		
		CheckWall();

		// Set camera to the center of a map.
		/*
		var camHeight = 2 * Camera.main.orthographicSize;
		var camWidth  = camHeight * Camera.main.aspect;
		Camera.main.transform.parent.SetPositionAndRotation(
			new Vector3(
				x / 2.0f - camWidth / 2.0f,
				y / 2.0f - camHeight / 2.0f
			),
			Camera.main.transform.parent.rotation
		);
		*/
	}

	/// <summary>
	/// Checks all tiles around each wall tile, so than we can swap the sprite to the correct one.
	/// </summary>
	public void CheckWall()
	{
		const int mazeTileSetType = 0;
		const string tileType = "Wall";
		// - - - - - - -
		// | 2 | 4 | 7 |
		// - - - - - - -
		// | 1 |   | 6 |
		// - - - - - - -
		// | 0 | 3 | 5 |
		// - - - - - - -
		const int
			left 	= 1,
			down 	= 3,
			up 		= 4,
			right 	= 6;

		foreach (KeyValuePair<Point, GameObject> tile in wallTiles)
		{
			string composition = TileCheck(tile.Key);
			
			if (composition[left] 	== 'G' &&
			    composition[down] 	== 'W' &&
			    composition[up]    	== 'G' &&
			    composition[right] 	== 'W')
			{
				Debug.Log(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "001");
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "001");
            }
            else if (composition[left] 	== 'W' &&
                	composition[down]	== 'W' &&
                	composition[up]    	== 'G' &&
                	composition[right] 	== 'W')
            {
	            int randomTile = UnityEngine.Random.Range(0, 100);
	            if (randomTile < 40)
		            tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "015");
	            else if (randomTile < 20)
		            tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "014");
	            else
                	tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "002");
		            
            }
            else if (composition[left] 	== 'W' &&
                	composition[down]	== 'W' &&
                	composition[up]    	== 'G' &&
                	composition[right] 	== 'G')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "003");
            }
            else if (composition[left]	== 'G' &&
            		composition[down]	== 'W' &&
            		composition[up] 	== 'W' &&
            		composition[right] 	== 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "004");
            }
            else if (composition[left] 	== 'W' &&
            		composition[down]	== 'W' &&
            		composition[up] 	== 'W' &&
            		composition[right]	== 'G')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "006");
            }
            else if (composition[left] 	== 'G' &&
            		composition[down]	== 'G' &&
            		composition[up] 	== 'W' &&
            		composition[right] 	== 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "007");
            }
            else if (composition[left] 	== 'W' &&
            		composition[down]	== 'G' &&
            		composition[up] 	== 'W' &&
            		composition[right]	== 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "008");
            }
            else if (composition[left] 	== 'W' &&
            		composition[down]	== 'G' &&
            		composition[up] 	== 'W' &&
            		composition[right]	== 'G')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "009");
            }
			else if (composition[left] 	== 'W' &&
			         composition[down] 	== 'W' &&
			         composition[up] 	== 'W' &&
			         composition[5] 	== 'G' &&
			         composition[right] == 'W')
			{
				tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "010");
			}
			else if (composition[0] 	== 'G' &&
			         composition[left] 	== 'W' &&
			         composition[down] 	== 'W' &&
			         composition[up] 	== 'W' &&
			         composition[right] == 'W')
			{
				tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "011");
			}
			else if (composition[left] 	== 'W' &&
			         composition[down] 	== 'W' &&
			         composition[up] 	== 'W' &&
			         composition[right] == 'W' &&
			         composition[7] 	== 'G')
			{
				tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "012");
			}
			else if (composition[left] 	== 'W' &&
			         composition[2] 	== 'G' &&
			         composition[down] 	== 'W' &&
			         composition[up] 	== 'W' &&
			         composition[right] == 'W')
			{
				tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "013");
			}
			else if (composition[0] 	== 'W' &&
			         composition[left] 	== 'W' &&
			         composition[2] 	== 'W' &&
			         composition[down] 	== 'W' &&
			         composition[up] 	== 'W' &&
			         composition[5] 	== 'W' &&
			         composition[right] == 'W' &&
			         composition[7] 	== 'W')
			{
				tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "005");
			}
            else if (composition[left] 	== 'W' &&
            		composition[down] 	== 'G' &&
            		composition[up] 	== 'G' &&
            		composition[right] 	== 'G')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "008");
            }
            else if (composition[left] 	== 'G' &&
            		composition[down] 	== 'G' &&
            		composition[up] 	== 'G' &&
            		composition[right] 	== 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "009");
            }
            else if (composition[left] 	== 'W' &&
            		composition[down] 	== 'G' &&
            		composition[up] 	== 'G' &&
            		composition[right] 	== 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "010");
            }
            else if (composition[left] 	== 'G' &&
            		composition[down] 	== 'W' &&
            		composition[up] 	== 'W' &&
            		composition[right] 	== 'G')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "011");
            }
            else if (composition[left] 	== 'G' &&
            		composition[down] 	== 'G' &&
            		composition[up] 	== 'W' &&
            		composition[right] 	== 'G')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "012");
            }
            else if (composition[left] 	== 'G' &&
            		composition[down] 	== 'W' &&
            		composition[up] 	== 'G' &&
            		composition[right] 	== 'G')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = _wallAtlas.GetSprite(_mazeTileSetTypes[mazeTileSetType] +"_"+ tileType +"_Tile_" + "013");
            }
		}
		
	}
	
	/// <summary>
	/// Checks all neighbors of each tile.
	/// </summary>
	/// <param name="currentPoint">The position of the tile we are checking.</param>
	/// <returns></returns>
	public string TileCheck(Point currentPoint)
	{
		string composition = string.Empty;

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;
				
				if (wallTiles.ContainsKey(new Point(currentPoint.MyX + x, currentPoint.MyY + y)))
				{
					composition += 'W';
				}
				else
				{
					composition += 'G';
				}
			}
		}

		return composition;
	}
}

[Serializable]
public class MapElement
{
	/// <summary>
	/// This tile tag, this is used to check what tile we are placing.
	/// </summary>
	[SerializeField]
	private string _tileTag;

	/// <summary>
	/// The color of the tile, this is used to compare the tile with colors on the map layers.
	/// </summary>
	[SerializeField]
	private Color _color;

	/// <summary>
	/// Prefab that we used to spawn the tile in our world.
	/// </summary>
	[SerializeField]
	private GameObject _elementPrefab;

	/// <summary>
	/// Property for accessing the prefab.
	/// </summary>
	public GameObject MyElementPrefab
	{
		get { return _elementPrefab; }
	}

	/// <summary>
	/// Property for accessing the color.
	/// </summary>
	public Color MyColor
	{
		get { return _color; }
	}
	
	/// <summary>
	/// Property for accessing the tag.
	/// </summary>
	public string MyTileTag
	{
		get { return _tileTag; }
	}
}

public struct Point
{
	public int MyX { get; set; }
	public int MyY { get; set; }

	public Point(int x, int y) : this()
	{
		MyX = x;
		MyY = y;
	}
}