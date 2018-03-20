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
}
