using UnityEngine;

namespace Others.Transformation
{
    /// <summary>
    /// Special storage for TransformManager with multiple transformation.
    /// </summary>
    public abstract class Transformation
    {
        /// <summary>
        /// Check if the transformation is already finished.
        /// </summary>
        public bool IsFinished;

        /// <summary>
        /// Timeout timer.
        /// </summary>
        protected float TimeoutTimer;
        
        /// <summary>
        /// Target object to tranform.
        /// </summary>
        protected GameObject Obj;
        
        /// <summary>
        /// Timeout.
        /// </summary>
        protected float Timeout;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="obj">Target object to transform.</param>
        /// <param name="timeout">Timeout of tthe transformation.</param>
        public Transformation(GameObject obj, float timeout)
        {
            Obj = obj;
            Timeout = timeout;
        }
        
        /// <summary>
        /// Tick to process the transformation.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public abstract void Tick(float deltaTime);
    }
}