using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Player's role.
    /// </summary>
    public class Bomberman : Player
    {
        public override float BombCountdown { get; set; } = Constants.BombermanBombCountdown;
        public override int BombExplosionDistance { get; set; } = Constants.BombermanBombExplosionDistance;
        public override int BombMaxAllowedDeploys { get; set; } = Constants.BombermanBombMaxAllowedDeploys;
        public override int[,] BombExplosionDirection { get; set; } = Constants.BombermanBombExplosionDirections;
        public override float RespawnDeathDelay { get; set; } = Constants.BombermanRespawnDeathDelay;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}