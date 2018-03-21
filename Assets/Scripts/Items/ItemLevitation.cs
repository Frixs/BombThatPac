using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
	/// <summary>
	/// This functionallity will let the gameobject levitate in 2D.
	/// </summary>
	public class ItemLevitation : MonoBehaviour
	{
		/// <summary>
		/// Original X coord of the object.
		/// </summary>
		private float _originalX;
		
		/// <summary>
		/// Original Y coord of the object.
		/// </summary>
		private float _originalY;

		/// <summary>
		/// Range of possible change positions.
		/// </summary>
		private float _floatStrength;

		/// <summary>
		/// Start levitate with random position.
		/// </summary>
		private float _randomSeed;

		// Use this for initialization
		void Start()
		{
			_originalX = transform.position.x;
			_originalY = transform.position.y;
			_floatStrength = MapManager.Instance.TilemapCellSize / 48f;
			_randomSeed = Random.Range(0f, 360f);
		}

		// Update is called once per frame
		void Update()
		{
			transform.position = new Vector3(
				_originalX + (float)Math.Cos(Time.time + _randomSeed) * _floatStrength,
				_originalY + (float)Math.Sin(Time.time + _randomSeed) * _floatStrength,
				transform.position.z
			);
		}
	}
}
