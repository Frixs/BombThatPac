using UnityEngine.Tilemaps;

/// <summary>
/// List of all game constants.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Player's default movement speed.
    /// </summary>
    public const float PlayerDefaultSpeed = 3.0f;

    /// <summary>
    /// Role: BOMBERMAN - number of the bombs that can be placed on the same time.
    /// </summary>
    public const int BombermanBombStackCount = 1;
    
    /// <summary>
    /// Role: BOMBERMAN - countdown when the bomb explodes.
    /// </summary>
    public const float BombermanBombCountdown = 2.0f;
    
    /// <summary>
    /// Role: BOMBERMAN - distance of bomb's explosion.
    /// </summary>
    public const int BombermanBombExplosionDistance = 3;
    
    /// <summary>
    /// Default name of the character.
    /// </summary>
    public const string CharacterDefaultName = "NONAME";
}
