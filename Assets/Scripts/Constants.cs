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
    /// Default number of the bombs that can be placed on the same time.
    /// </summary>
    public const int PlayerDefaultBombStackCount = 1;

    /// <summary>
    /// Role: BOMBERMAN - number of the bombs that can be placed on the same time.
    /// </summary>
    public const int BombermanBombStackCount = 1;

    /// <summary>
    /// Default countdown when the bomb explodes.
    /// </summary>
    public const float BombDefaultCountdown = 2.0f;
    
    /// <summary>
    /// Role: BOMBERMAN - countdown when the bomb explodes.
    /// </summary>
    public const float BombermanBombCountdown = 2.0f;
    
    /// <summary>
    /// Default name of the character.
    /// </summary>
    public const string CharacterDefaultName = "NONAME";
}
