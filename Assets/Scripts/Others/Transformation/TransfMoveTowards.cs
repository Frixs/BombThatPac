using UnityEngine;

namespace Others.Transformation
{
    public class TransfMoveTowards : Transformation
    {
        /// <summary>
        /// Target position.
        /// </summary>
        private Vector3 _targetPos;

        /// <summary>
        /// Speed of movement.
        /// </summary>
        private float _speed;
        
        public TransfMoveTowards(GameObject obj, float timeout, Vector3 targetPos, float speed) : base(obj, timeout)
        {
            _targetPos = targetPos;
            _speed = speed;
        }
        
        public override void Tick(float deltaTime)
        {
            if (IsFinished)
                return;
            
            TimeoutTimer += deltaTime;
            
            Obj.transform.position = Vector3.MoveTowards(Obj.transform.position, _targetPos, deltaTime * _speed);

            if (Obj.transform.position == _targetPos)
            {
                Obj.transform.position = _targetPos;
                IsFinished = true;
            }

            if (Timeout > 0f && TimeoutTimer >= Timeout)
                IsFinished = true;
        }
    }
}