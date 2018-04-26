using System.Collections.Generic;
using Others.Transformation;
using UnityEngine;

namespace Managers
{
    public class TransformManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of TransformManager which allows it to be accessed by any other script.
        /// </summary>
        public static TransformManager Instance = null;
        
        /// <summary>
        /// All transformations which should be process.
        /// </summary>
        public List<Transformation> TransformationList = new List<Transformation>();
        
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
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a TransformManager.
                Destroy(gameObject);
            }

            // Sets this to not be destroyed when reloading scene.
            //DontDestroyOnLoad(gameObject);
        }
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            ProcessTransforms();
        }

        /// <summary>
        /// Process all transformations on the list.
        /// </summary>
        public void ProcessTransforms()
        {
            foreach (Transformation t in TransformationList.ToArray())
            {
                t.Tick(Time.deltaTime);
                if (t.IsFinished)
                {
                    TransformationList.Remove(t);
                }
            }
        }

        /// <summary>
        /// Add new tranformation MoveTowards.
        /// </summary>
        /// <param name="obj">Object to transform.</param>
        /// <param name="timeout">Timeout of the transformation.</param>
        /// <param name="targetPos">Target position of the move.</param>
        /// <param name="speed">Speed of the move.</param>
        /// <returns>Added transformation or null on error.</returns>
        public Transformation AddTransfMoveTowards(GameObject obj, float timeout, Vector3 targetPos, float speed)
        {
            if (obj == null || speed == 0f)
            {
                Debug.unityLogger.Log(LogType.Error, "Null reference!");
                return null;
            }

            Transformation t = new TransfMoveTowards(obj, timeout, targetPos, speed);
            TransformationList.Add(t);

            return t;
        }
    }
}