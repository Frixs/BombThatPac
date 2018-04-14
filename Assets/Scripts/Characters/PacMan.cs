using UnityEngine;

namespace Characters
{
    public class PacMan : ArtificialIntelligence
    {
        // Use this for initialization
        public override float MoveSpeed { get; set; } = Constants.PacManDefaultMoveSpeed;
        public override float RespawnDeathDelay { get; set; } = 0f;

        public override RuntimeAnimatorController EventAnimationController => _eventAnimationController;
        [SerializeField] private RuntimeAnimatorController _eventAnimationController;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Move()
        {
        }

        public override void Kill(Character attacker)
        {
            throw new System.NotImplementedException();
        }

        public override void ForceKill(bool respawn)
        {
            throw new System.NotImplementedException();
        }
    }
}