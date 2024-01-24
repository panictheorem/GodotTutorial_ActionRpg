using Godot;
using System;

public partial class GameController : Node2D
{
    public PackedScene PreviousScene { get; set; }
    public PackedScene CurrentScene { get; set; }

    [Export]
    public PackedScene WorldScene { get; set; }

    [Export]
    public PackedScene CliffScene { get; set; }

    public bool TransitionScene { get; set; }
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
    }

    public void UpdateCurrentScene(PackedScene scene)
    {
        GD.Print($"Change Scene {scene?.ResourcePath}");
        GD.Print($"CurrentScene Scene {CurrentScene?.ResourcePath}");
        if (CurrentScene != scene)
        {
            PreviousScene = CurrentScene;
            CurrentScene = scene;
            GetTree().ChangeSceneToPacked(scene);
        }
    }
}
