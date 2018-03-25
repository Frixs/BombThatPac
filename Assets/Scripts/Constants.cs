/// <summary>
/// List of all game constants.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Default name of the character.
    /// </summary>
    public const string CharacterDefaultName = "NONAME";

    /// <summary>
    /// Player's default movement speed.
    /// </summary>
    public const float PlayerDefaultSpeed = 3.0f;
    
    /// <summary>
    /// Ghost's default movement speed.
    /// </summary>
    public const float GhostDefaultSpeed = PlayerDefaultSpeed - 1.1f;

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
    public const int BombermanBombMaxAllowedDeploys = 3;

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
        { 1,  0, 0}, // Right 
        { 0,  1, 0}, // Up
        {-1,  0, 0}, // Left
        { 0, -1, 0}, // Down
    };
    
    /// <summary>
    /// Countdown when the bomb explodes and hits another one. The new one will get new countdown set for this value.
    /// </summary>
    public const float BombChainedCountdown = 0.5f;

    /// <summary>
    /// Layer name string reference to layer which hold all objects for script to be able to find among all objects that needs to be triggered (f.e bomb is looking for players.).
    /// </summary>
    public const string UserLayerNameTriggerObject = "TriggerObject";
    
    /// <summary>
    /// Delay after death to respawn.
    /// </summary>
    public const float GhostRespawnDeathDelay = 3.0f;
    
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
}
