namespace Characters
{
    /// <summary>
    /// Player's role.
    /// </summary>
    public class Bomberman : Player
    {
        public override int BombStackCount { get; set; } = Constants.BombermanBombStackCount;
        public override float BombCountdown { get; set; } = Constants.BombermanBombCountdown;
        public override int BombExplosionDistance { get; set; } = Constants.BombermanBombExplosionDistance;

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