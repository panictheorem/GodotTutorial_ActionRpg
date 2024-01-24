using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 700.0f;
    public bool isChasingPlayer { get; set; } = false;
    public Player Player { get; set; } = null;
    public Vector2 Direction { get; set; } = Vector2.Zero;
    public AnimationTree AnimationTree { get; set; }
    public bool IsDying { get; set; } = false;
    public bool IsHit { get; set; } = false;
    [Export]
    public int Health { get; set; } = 100;
    public ProgressBar HealthBar { get; set; }

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        AnimationTree.Active = true;
        HealthBar = GetNode<ProgressBar>("HealthBar");
    }

    public override void _Process(double delta)
    {
        UpdateAnimation();
        UpdateHealth();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!IsDying)
        {
            if (IsHit)
            {
                Velocity = -(Player.GlobalPosition - this.GlobalPosition) * 1000.0f * (float)delta;
                IsHit = false;
            }
            else if (Direction != Vector2.Zero)
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
    }

    public void _on_detection_area_body_entered(Node2D body)
    {
        Player = body as Player;
        if (Player != null)
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
        if (Direction == Vector2.Zero)
        {
            AnimationTree.Set("parameters/conditions/idle", true);
            AnimationTree.Set("parameters/conditions/walking", false);
        }
        else
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/walking", true);
        }

        if (IsDying)
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

    public void _on_hitbox_area_entered(Node2D body)
    {
        Health -= 20;
        if(Health <= 0)
        {
            IsDying = true;
        }
        else
        {
            IsHit = true;
        }
    }

    public void UpdateHealth()
    {
        HealthBar.Value = Health;
        if (Health >= 100)
        {
            HealthBar.Visible = false;
        }
        else
        {
            HealthBar.Visible = true;
        }
    }

    public void _on_regen_timer_timeout()
    {
        if (Health < 100)
        {
            Health += 20;
        }
        if (Health > 100)
        {
            Health = 100;
        }

        if (Health < 0)
        {
            Health = 0;
        }
    }
}
