using Godot;
using System;

public partial class Cliff : Node2D
{
    public Player Player { get; set; }
    public Marker2D StartingPosition { get; set; }
    public Marker2D CameraBoundsTopLeft { get; set; }
    public Marker2D CameraBoundsBottomRight { get; set; }
    public GameController GameController { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GameController = GetNode<GameController>("/root/GameController");

        CameraBoundsTopLeft = GetNode<Marker2D>("CameraBounds/TopLeft");
        CameraBoundsBottomRight = GetNode<Marker2D>("CameraBounds/BottomRight");
        Player = GetNode<Player>("Player");
        StartingPosition = GetNode<Marker2D>("StartingPosition");
        Player.GlobalPosition = StartingPosition.GlobalPosition;
        Player.FacingDirection = new Vector2(-1, 0);
        Player.UpdateCameraBounds((int)CameraBoundsTopLeft.GlobalPosition.X, (int)CameraBoundsBottomRight.GlobalPosition.X, (int)CameraBoundsTopLeft.GlobalPosition.Y, (int)CameraBoundsBottomRight.GlobalPosition.Y);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_exit_area_body_entered(Node2D body)
    {
        var player = body as Player;
        if (player != null)
        {
            GameController.CallDeferred("UpdateCurrentScene", GameController.WorldScene);
        }
    }
}
