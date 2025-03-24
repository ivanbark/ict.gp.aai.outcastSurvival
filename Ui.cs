using Godot;
using System;

public partial class Ui : CanvasLayer
{
	private Label gameOverLabel;

	public override void _Ready()
	{
		gameOverLabel = GetNode<Label>("GameOverLabel");
		gameOverLabel.Visible = false;
	}

	public void DisplayGameOverMessage()
	{
		gameOverLabel.Visible = true;
	}
}
