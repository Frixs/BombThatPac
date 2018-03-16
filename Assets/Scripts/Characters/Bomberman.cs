namespace Characters
{
    /// <summary>
    /// Player's role.
    /// </summary>
    public class Bomberman : Player
    {
        public Bomberman()
        {
            BombStackCount = Constants.BombermanBombStackCount;
            BombCountdown = Constants.BombermanBombCountdown;
        }

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