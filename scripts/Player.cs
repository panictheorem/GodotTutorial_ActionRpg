using Godot;
using System;
using static Godot.TextServer;

public partial class Player : CharacterBody2D
{
    [Export]
    public const float Speed = 100.0f;
    public AnimationTree AnimationTree { get; set; }
    public Vector2 Direction { get; set; } = Vector2.Zero;

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        AnimationTree.Active = true;
    }

    public override void _Process(double delta)
    {
        UpdateAnimationParams();
    }

    public override void _PhysicsProcess(double delta)
    {
        Player_Movement(delta);
    }

    public void Player_Movement(double delta)
    {
        Direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Velocity = Direction.Normalized() * Speed;

        MoveAndSlide();
    }

    public void UpdateAnimationParams()
    {
        if (Velocity == Vector2.Zero)
        {
            AnimationTree.Set("parameters/conditions/idle", true);
            AnimationTree.Set("parameters/conditions/walk", false);
        }
        else
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/walk", true);
        }

        if (Direction != Vector2.Zero)
        {
            AnimationTree.Set("parameters/Idle/blend_position", Direction);
            AnimationTree.Set("parameters/Walk/blend_position", Direction);
            AnimationTree.Set("parameters/Attack/blend_position", Direction);
        }
    }
}
