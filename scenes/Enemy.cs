using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 25.0f;
    public bool isChasingPlayer { get; set; } = false;
    public Player Player { get; set; } = null;
    public Vector2 Direction { get; set; } = Vector2.Zero;
    public AnimationTree AnimationTree { get; set; }
    public bool IsDying { get; set; } = false;

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        AnimationTree.Active = true;
    }

    public override void _Process(double delta)
    {
        UpdateAnimation();
    }

    public override void _PhysicsProcess(double delta)
	{
        if(Direction != Vector2.Zero)
        {
            Direction = GlobalPosition.DirectionTo(Player.GlobalPosition);
            Velocity = Direction * Speed * (float)delta;
        }
        else
        {
            Velocity = Direction;
        }
        MoveAndSlide();
	}

	public void _on_detection_area_body_entered(Node2D body)
	{
        Player = body as Player;
		if(Player != null)
		{
            Direction = GlobalPosition.DirectionTo(Player.GlobalPosition);
		}
	}

	public void _on_detection_area_body_exited(Node2D body)
    {
        Player = body as Player;
        if (Player != null)
        {
            Direction = Vector2.Zero;
            Player = null;
        }
    }

    public void UpdateAnimation()
    {
        if(Direction == Vector2.Zero)
        {
            AnimationTree.Set("parameters/conditions/idle", true);
            AnimationTree.Set("parameters/conditions/walking", false);
        }
        else
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/walking", true);
        }

        if(IsDying)
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/walking", false);
            AnimationTree.Set("parameters/conditions/die", true);
        }

        if (Direction != Vector2.Zero)
        {
            AnimationTree.Set("parameters/Idle/blend_position", Direction);
            AnimationTree.Set("parameters/Walk/blend_position", Direction);
            AnimationTree.Set("parameters/Die/blend_position", Direction);
        }
    }
}
