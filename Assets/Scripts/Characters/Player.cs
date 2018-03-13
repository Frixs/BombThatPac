using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    /// <summary>
    /// Input HOTKEY to MOVE UP.
    /// </summary>
    [SerializeField] protected KeyCode InputMoveUp = KeyCode.W;

    /// <summary>
    /// Input HOTKEY to MOVE LEFT.
    /// </summary>
    [SerializeField] protected KeyCode InputMoveLeft = KeyCode.A;

    /// <summary>
    /// Input HOTKEY to MOVE DOWN.
    /// </summary>
    [SerializeField] protected KeyCode InputMoveDown = KeyCode.S;

    /// <summary>
    /// Input HOTKEY to MOVE RIGHT.
    /// </summary>
    [SerializeField] protected KeyCode InputMoveRight = KeyCode.D;

    // Use this for initialization
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput();

        base.Update();
    }

    /// <summary>
    /// Listen's to the players input.
    /// </summary>
    protected void GetInput()
    {
        Direction = Vector2.zero;

        if (Input.GetKey(InputMoveUp))
        {
            Direction += Vector2.up;
        }
        else if (Input.GetKey(InputMoveLeft))
        {
            Direction += Vector2.left;
        }
        else if (Input.GetKey(InputMoveDown))
        {
            Direction += Vector2.down;
        }
        else if (Input.GetKey(InputMoveRight))
        {
            Direction += Vector2.right;
        }
    }
}