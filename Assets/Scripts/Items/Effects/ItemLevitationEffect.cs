using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items.Effects
{
	/// <summary>
	/// This functionallity will let the gameobject levitate in 2D.
	/// </summary>
	public class ItemLevitationEffect : MonoBehaviour
	{
		/// <summary>
		/// New Levitation scale, possible to set from inspector.
		/// </summary>
		[Range(1, 100)] public float NewLevitationScale;
		
		/// <summary>
		/// Levitation scale. Default value.
		/// </summary>
		private float LevitationScaleDefault = 40f;
		
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
			float levitationScale = NewLevitationScale > 0f ? NewLevitationScale : LevitationScaleDefault;
			
			_originalX = transform.position.x;
			_originalY = transform.position.y;
			_floatStrength = MapManager.Instance.TilemapCellSize / levitationScale;
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
