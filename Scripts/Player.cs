using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed {get; set; } = 200;
	[Export]
	public int BodyRotateSpeed = 3;
	[Export] 
	public int TurretRotateSpeed = 3;

	public Sprite2D TankTurret;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TankTurret = GetNode<Sprite2D>("Tank Body/Tank Turret");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
		MoveAndSlide();
		Animate();
	}

	public void Animate()
	{
		//turret rotation
		double mouseAngle =
			Math.Round(Mathf.RadToDeg(TankTurret.GlobalPosition.AngleToPoint(GetGlobalMousePosition()))) - 90;
		var turretAngle = (float)Math.Round(TankTurret.GlobalRotationDegrees);
		double turretAngleDifferent = AngleDifference(mouseAngle, turretAngle);
		if (turretAngleDifferent > TurretRotateSpeed)
		{
			turretAngle -= TurretRotateSpeed;
		}
		else if (turretAngleDifferent < TurretRotateSpeed)
		{
			turretAngle += TurretRotateSpeed;
		}
		TankTurret.GlobalRotationDegrees = (float) Math.Round(turretAngle);
		
		

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
