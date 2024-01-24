using Godot;
using System;
using static Godot.TextServer;

public partial class Player : CharacterBody2D
{
    [Export]
    public const float Speed = 100.0f;
    public AnimationTree AnimationTree { get; set; }
    public Timer AttackCooldown { get; set; }
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;
    public Vector2 FacingDirection { get; set; } = new Vector2(0, 1); //Default facing forward
    public Enemy Enemy { get; set; }
    public bool IsEnemyInAttackRange { get; set; } = false;
    public bool IsEnemyInAttackCooldown { get; set; } = false;
    [Export]
    public int Health { get; set; } = 100;
    public bool IsPlayerAlive { get; set; } = true;

    public bool IsPlayerAttacking { get; set; } = false;
    public Camera2D PlayerCamera { get; set; }
    public ProgressBar HealthBar { get; set; }

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        AnimationTree.Active = true;

        AttackCooldown = GetNode<Timer>("AttackCooldown");
        PlayerCamera = GetNode<Camera2D>("Camera2D");
        HealthBar = GetNode<ProgressBar>("HealthBar");
    }

    public override void _Process(double delta)
    {
        UpdateAnimationParams();
    }

    public override void _PhysicsProcess(double delta)
    {
        Player_Movement(delta);
        EnemyAttack();
        UpdateHealth();
    }

    public void Player_Movement(double delta)
    {
        MoveDirection = Vector2.Zero;

        if (IsPlayerAlive && !IsPlayerAttacking)
        {
            MoveDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            Velocity = MoveDirection.Normalized() * Speed;
            if (MoveDirection != Vector2.Zero)
            {
                FacingDirection = MoveDirection;
            }
            MoveAndSlide();
        }
    }

    public void UpdateAnimationParams()
    {
        if (!IsPlayerAttacking && Input.IsActionJustPressed("attack"))
        {
            IsPlayerAttacking = true;
            GD.Print($"Attack anim: {FacingDirection}");
            AnimationTree.Set("parameters/conditions/attack", true);
            AnimationTree.Set("parameters/conditions/walk", false);
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/Attack/blend_position", FacingDirection);

        }
        else if (Velocity == Vector2.Zero)
        {

            AnimationTree.Set("parameters/conditions/attack", false);
            AnimationTree.Set("parameters/conditions/walk", false);
            AnimationTree.Set("parameters/conditions/idle", true);
        }
        else
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/attack", false);
            AnimationTree.Set("parameters/conditions/walk", true);
        }

        if (!IsPlayerAlive)
        {
            AnimationTree.Set("parameters/conditions/idle", false);
            AnimationTree.Set("parameters/conditions/walk", false);
            AnimationTree.Set("parameters/conditions/attack", false);
            AnimationTree.Set("parameters/conditions/die", true);
        }

        if (MoveDirection != Vector2.Zero)
        {
            AnimationTree.Set("parameters/Idle/blend_position", MoveDirection);
            AnimationTree.Set("parameters/Walk/blend_position", MoveDirection);
            AnimationTree.Set("parameters/Die/blend_position", MoveDirection);
        }
    }

    public void _on_damage_hitbox_body_entered(Node2D body)
    {
        var enemy = body as Enemy;

        if (enemy != null)
        {
            IsEnemyInAttackRange = true;
        }
    }

    public void _on_damage_hitbox_body_exited(Node2D body)
    {
        var enemy = body as Enemy;

        if (enemy != null)
        {
            IsEnemyInAttackRange = false;
        }
    }

    public void EnemyAttack()
    {
        if (IsEnemyInAttackRange && !IsEnemyInAttackCooldown)
        {
            Health -= 10;
            IsEnemyInAttackCooldown = true;
            AttackCooldown.Start();
            if (Health <= 0)
            {
                IsPlayerAlive = false;
            }
        }
    }

    public void _on_attack_cooldown_timeout()
    {
        IsEnemyInAttackCooldown = false;
    }

    public void _on_attack_finished()
    {
        IsPlayerAttacking = false;
        UpdateAnimationParams();
    }

    public void UpdateCameraBounds(int left, int right, int top, int bottom)
    {
        PlayerCamera.LimitLeft = left;
        PlayerCamera.LimitRight = right;
        PlayerCamera.LimitTop = top;
        PlayerCamera.LimitBottom = bottom;
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
