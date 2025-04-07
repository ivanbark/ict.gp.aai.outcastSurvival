using Godot;
using System;

public partial class Globals : Node
{
    public bool IsGameOver = false;
    public bool IsGameWon = false;

    public void EndGame(bool isWon)
    {
        IsGameOver = true;
        IsGameWon = isWon;
    }

}
