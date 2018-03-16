using UnityEngine;

namespace Items
{
	public class Bomb : MonoBehaviour
	{
		private float _countdown = 2.0f;
	
		// Use this for initialization
		void Start ()
		{	
		}
	
		// Update is called once per frame
		void Update ()
		{
			_countdown -= Time.deltaTime;

			if (_countdown <= 0.0f)
			{
				Debug.unityLogger.logHandler.LogFormat(LogType.Log, null, "Bomb exploded!");
				FindObjectOfType<MapDestroyer>().Explode(transform.position);
				Destroy(gameObject);
			}
		}
	}
}
