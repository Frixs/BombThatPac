using System;
using StatusEffects.Scriptable;
using UnityEngine;

namespace Items.SpecialItems
{
	public abstract class SpecialItem : MonoBehaviour
	{
		/// <summary>
		/// Status effect of tzhe current item. It handles whole functionality of the item.
		/// </summary>
		[Header("Item Settings")] public ScriptableStatusEffect ItemStatusEffect;

		/// <summary>
		/// Drop chance of the current item.
		/// </summary>
		public float DropChance;

		/// <summary>
		/// Check if item is thrown.
		/// </summary>
		private bool _isThrown;

		/// <summary>
		/// Throwing position.
		/// </summary>
		private Vector3 _throwingPosition;
		
		/// <summary>
		/// Throwing speed.
		/// </summary>
		private float _throwingSpeed;

		/// <summary>
		/// Throwing distance.
		/// </summary>
		private float _throwingDistance;
		
		// Use this for initialization
		protected virtual void Start()
		{
		}
	
		// Update is called once per frame
		protected virtual void Update()
		{
			if (_isThrown)
				Throw();
		}

		/// <summary>
		/// Start throwing the item to the position.
		/// </summary>
		/// <param name="position">Final position.</param>
		/// <param name="speed">Throwing speed.</param>
		public void StartThrowing(Vector3 position, float speed)
		{
			if (position == transform.position || speed == 0f)
				return;
			
			_isThrown = true;
			GetComponent<ItemLevitation>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;
			_throwingPosition = position;
			_throwingSpeed = speed;
			_throwingDistance = Vector3.Distance(transform.position, _throwingPosition);
			transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		}

		/// <summary>
		/// Throw the item to the position.
		/// </summary>
		private void Throw()
		{
			transform.position = Vector3.MoveTowards(transform.position, _throwingPosition, Time.deltaTime * _throwingSpeed);
			
			// Current distance to the final position.
			float distance = Vector3.Distance(transform.position, _throwingPosition);

			// Percentage, how many prct from whole path left to reach the target.
			float pctLeft = distance / (_throwingDistance / 100f);
			// 1 percentage of sinusoid.
			float onePctOfHalfSin = Mathf.PI / 100f;
			// Final progress scale of whole distance.
			float scale = Mathf.Sin(pctLeft * onePctOfHalfSin) + 1f; // +1 because of we need to have 1 as the base scale.

			transform.localScale = new Vector3(scale, scale, scale);
			transform.GetComponent<SpriteRenderer>().color = new Color(
				transform.GetComponent<SpriteRenderer>().color.r,
				transform.GetComponent<SpriteRenderer>().color.g,
				transform.GetComponent<SpriteRenderer>().color.b,
				scale > 1f ? 2.75f - scale : scale // Scale is number between 1f and 2f. Thanks to this the alpha will be 0.75 at the highest point of sinusoid animation.
			);

			if (transform.position == _throwingPosition)
			{
				_isThrown = false;
				transform.localScale = new Vector3(1f, 1f, 1f);
				transform.GetComponent<SpriteRenderer>().color = new Color(
					transform.GetComponent<SpriteRenderer>().color.r,
					transform.GetComponent<SpriteRenderer>().color.g,
					transform.GetComponent<SpriteRenderer>().color.b,
					1f
				);
				GetComponent<ItemLevitation>().enabled = true;
				GetComponent<CircleCollider2D>().enabled = true;
			}
		}
	}
}
