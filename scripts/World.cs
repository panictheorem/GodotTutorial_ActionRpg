using Godot;
using System;

public partial class World : Node2D
{
    public GameController GameController { get; set; }
    public Player Player { get; set; }
    public Marker2D InitialPlayerPosition { get; set; }
    public Marker2D WestEntrancePlayerPosition { get; set; }
    [Export]
    public PackedScene WestScene { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GameController = GetNode<GameController>("/root/GameController");
        Player = GetNode<Player>("Player");
        InitialPlayerPosition = GetNode<Marker2D>("PlayerPositions/Initial");
        WestEntrancePlayerPosition = GetNode<Marker2D>("PlayerPositions/WestEntrance");
        SetPlayerPosition();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void SetPlayerPosition()
    {
        if (GameController.PreviousScene == WestScene)
        {
            Player.GlobalPosition = WestEntrancePlayerPosition.GlobalPosition;
        }
        else
        {
            Player.GlobalPosition = InitialPlayerPosition.GlobalPosition;
        }
    }
    public void _on_west_side_exit_body_entered(Node2D body)
    {
        var player = body as Player;
        if (player != null)
        {
            GameController.CallDeferred("UpdateCurrentScene", WestScene);
        }
    }
}
