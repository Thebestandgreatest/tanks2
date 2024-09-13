using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed {get; set; } = 200;
	[Export] public int BodyRotateSpeed = 3;
	[Export] public int TurretRotateSpeed = 3;
	[Export] public bool UseController = false;
	[Export] public int CursorXOffset = 19;
	[Export] public int CursorYOffset = 14;

	public Sprite2D TankTurret;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//temp cursor change for aiming purposes
		Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Sprites/crosshair007.png"));
		TankTurret = GetNode<Sprite2D>("Tank Body/Tank Turret");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		Animate();
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
	}

	public void Animate()
	{
		//turret rotation
		//see what has been used more recently mouse/keyboard movement or controller
		//ignore the other options inputs
		//get input vectors from other control stick

		Vector2 mouse = new Vector2(GetGlobalMousePosition().X + CursorXOffset, GetGlobalMousePosition().Y + CursorYOffset);
		TankTurret.GlobalRotationDegrees = Mathf.RadToDeg(GlobalPosition.AngleToPoint(mouse)) - 90;
		
		/*
		not working, has an issue with bouncing between two angles and has a creeping inaccuracy
		double turretAngle = (float)Math.Round(TankTurret.GlobalRotationDegrees);
		double turretAngleDifference = 0;
		double mouseAngle =
			Math.Round(Mathf.RadToDeg(TankTurret.GlobalPosition.AngleToPoint(GetGlobalMousePosition()))) - 90;
		turretAngleDifference = AngleDifference(mouseAngle, turretAngle);
		
		if (turretAngleDifference > (TurretRotateSpeed * 2))
		{
			turretAngle -= TurretRotateSpeed;
		}
		else if (turretAngleDifference < (TurretRotateSpeed * 2))
		{
			turretAngle += TurretRotateSpeed;
		}
		TankTurret.GlobalRotationDegrees = (float) Math.Floor(turretAngle);
		*/

		//body rotation
		double velocityAngle = Mathf.Round(Mathf.RadToDeg(Velocity.Angle()));
		double bodyAngle = Mathf.Round(GlobalRotationDegrees);
		double bodyAngleDifference = AngleDifference(bodyAngle, velocityAngle);
		if (Velocity == Vector2.Zero) return;
		if (bodyAngleDifference > 1)
		{
			bodyAngle += BodyRotateSpeed;
		}
		else if (bodyAngleDifference < -1)
		{
			bodyAngle -= BodyRotateSpeed;
		} 
		GlobalRotationDegrees = (float) bodyAngle;
	}
			

	public static double AngleDifference(double testAngle, double currentAngle)
	{
		double diff = (currentAngle - testAngle + 180) %360 - 180;
		return diff < -180 ? diff + 360 : diff;
	}
}
