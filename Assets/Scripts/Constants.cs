/// <summary>
/// List of all game constants.
/// </summary>
public static class Constants
{
    /*
     * GLOBAL
     */
 
    /// <summary>
    /// Default name of the character.
    /// </summary>
    public const string CharacterDefaultName = "NONAME";
    
    /// <summary>
    /// Player's default movement speed.
    /// </summary>
    public const float PlayerDefaultMoveSpeed = 3.0f;
 
    /// <summary>
    /// COuntdown on the start game.
    /// </summary>
    public const float GameStartCountdown = 3.0f;
 
    /*
     * BOMBERMAN
     */
    
    /// <summary>
    /// Role: BOMBERMAN - countdown when the bomb explodes.
    /// </summary>
    public const float BombermanBombCountdown = 2.0f;

    /// <summary>
    /// Role: BOMBERMAN - distance of bomb's explosion.
    /// </summary>
    public const int BombermanBombExplosionDistance = 2;

    /// <summary>
    /// Role: BOMBERMAN - bomb max deploy count.
    /// </summary>
    public const int BombermanBombMaxAllowedDeploys = 2;

    /// <summary>
    /// Role: BOMBERMAN - Time to respawn the character.
    /// </summary>
    public const float BombermanRespawnDeathDelay = 3.0f;

    /// <summary>
    /// Role: BOMBERMAN - bomb explosion directions.
    /// 
    /// To correct functionality:
    /// Vector3Int filled only with -1, 0, 1 values. Each direction can has only1 value rest 0. We are on grid only.
    /// </summary>
    public static readonly int[,] BombermanBombExplosionDirections = {
        //x,  y, z
        { 0,  1, 0}, // Up
        {-1,  0, 0}, // Left
        { 0, -1, 0}, // Down
        { 1,  0, 0}, // Right 
    };
    
    /*
     * BOMB
     */
    
    /// <summary>
    /// Countdown when the bomb explodes and hits another one. The new one will get new countdown set for this value.
    /// </summary>
    public const float BombChainedCountdown = 0.2f;
 
    /// <summary>
    /// This const says how many cells the bomb will be pushed when the caster of the bomb push the bomb in the current direction.
    /// </summary>
    public const int BombRollDistance = 4;
 
    /// <summary>
    /// Speed of a rolling bomb.
    /// </summary>
    public const float BombRollSpeed = 4.0f;
 
    /// <summary>
    /// Delayed explosion after rolling a bomb.
    /// </summary>
    public const float BombExplosionDelayAfterRolling = 0.5f;

    /*
     * LAYERS
     */
    
    /// <summary>
    /// Layer name string reference to layer which hold all objects for script to be able to find among all objects that needs to be triggered (f.e bomb is looking for players.).
    /// </summary>
    public const string UserLayerNameTriggerObject = "TriggerObject";
    
    /// <summary>
    /// Layer name string reference to layer which hold all objects for script to be able to find among all objects that needs to be refered as obstacle.
    /// </summary>
    public const string UserLayerNameObstacle = "Obstacle";
 
    /// <summary>
    /// Layer name string reference to layer which hold all objects which are collectable by the player.
    /// </summary>
    public const string UserLayerNameCollectable = "Collectable";
    
    /*
     * GHOSTS
     */
    
    /// <summary>
    /// Ghost's default movement speed.
    /// </summary>
    public const float GhostDefaultMoveSpeed = PlayerDefaultMoveSpeed - 0.1f;
 
    /// <summary>
    /// Delay after player death to respawn the player.
    /// </summary>
    public const float GhostPlayerRespawnDeathDelay = 3.0f;
    
    /// <summary>
    /// Duration of ghost frightened mode.
    /// </summary>
    public const float GhostFrightenedModeDuration = 10.0f;

    /// <summary>
    /// The ghosts start blinking a few seconds before they will go back to normal/Chase mode. This is number which says how long they will NOT blinking.
    /// </summary>
    public const float GhostFrightenedModeStartBlinkingAt = GhostFrightenedModeDuration - 3.0f;
    
    /// <summary>
    /// Blinking speed of the ghosts in frightened mode.
    /// </summary>
    public const float GhostFrightenedModeBlinkingSpeed = 0.2f;
    
    /// <summary>
    /// Number of iterations for ghost modes.
    /// </summary>
    public const int GhostModeNumberOfIterations = 4;

    /// <summary>
    /// Ghost mode phases from the start of the game.
    /// </summary>
    public static readonly float[] GhostScatterModeTimer = new float[GhostModeNumberOfIterations] { 7f, 7f, 5f, 5f };

    /// <summary>
    /// Ghost mode phases from the start of the game. Chase mode is always after Scatter mode.
    /// </summary>
    public static readonly float[] GhostChaseModeTimer = new float[GhostModeNumberOfIterations - 1] { 20f, 20f, 20f };

    /// <summary>
    /// Release Blinky ghost from its house.
    /// </summary>
    public const float GhostBlinkyReleaseTimer = 0.0f;
    
    /// <summary>
    /// Release Pinky ghost from its house.
    /// </summary>
    public const float GhostPinkyReleaseTimer = 0.0f;
    
    /// <summary>
    /// Release Inky ghost from its house.
    /// </summary>
    public const float GhostInkyReleaseTimer = 0.1f;
    
    /// <summary>
    /// Release Clyde ghost from its house.
    /// </summary>
    public const float GhostClydeReleaseTimer = 0.2f;
    
    /// <summary>
    /// Pinky targets its target X cells ahead of target's direction.
    /// This is the number of cells.
    /// </summary>
    public const int GhostPinkyNumberOfCellsAheadToTarget = 3;
    
    /// <summary>
    /// Select the position X cells in front of the closest player.
    /// Draw Vector from Blinky ghost to that position.
    /// X-times the length of the vector (GhostInkyVectorMultiplier).
    /// </summary>
    public const int GhostInkyNumberOfCellsAhead = 2;
    public const int GhostInkyVectorMultiplier = 2;
    
    /// <summary>
    /// Calculate the distance from the closest player.
    /// If the distance is greater than X cells targeting is the same as Blinky.
    /// If the distance is less than X cells, then target is his scatter base, so same as scatter mode.
    /// </summary>
    public const float GhostClydeNumberOfCellsDistance = 4f;
    
    /// <summary>
    /// Ghost method to get random cell around ghost start target position. This is the max. distance to reach random cell around.
    /// </summary>
    public const int GhostGetRandomCellMethodMaxDistance = 6;
 
    /*
     * PACMAN
     */
    
    /// <summary>
    /// PacMan's default movement speed.
    /// </summary>
    public const float PacManDefaultMoveSpeed = PlayerDefaultMoveSpeed + 0.5f;
}
