using System.Threading;

using Microsoft.Research.Malmo;

namespace MissionControl
{
	public static class ContinuousAgentCommands
	{
		public static AgentHost Agent
		{ get; private set; }

		public static void Initialize(AgentHost agent)
		{
			Agent = agent;
		}

		public static void Move(double velocity)
		{
			Agent.sendCommand("move " + velocity);
		}
		
		public static void Strafe(double velocity)
		{
			Agent.sendCommand("strafe " + velocity);
		}

		public static void MoveStrafe(Vector2 velocity)
		{
			Strafe(velocity.X);
			Move(velocity.Y);
		}

		public static void Jump()
		{
			Agent.sendCommand("jump 1");
		}

		public static void StopJump()
		{
			Agent.sendCommand("jump 0");
		}

		public static void Crouch()
		{
			Agent.sendCommand("crouch 1");
		}

		public static void UnCrouch()
		{
			Agent.sendCommand("crouch 0");
		}

		public static void Attack()
		{
			Agent.sendCommand("attack 1");
		}

		public static void StopAttack()
		{
			Agent.sendCommand("attack 0");
		}

		public static void Click()
		{
			Thread clicker = new Thread(() => {
				Attack();
				Thread.Sleep(100);
				StopAttack();
			});
			clicker.Start();
		}

		public static void Use()
		{
			Agent.sendCommand("attack 1");
		}

		public static void StopUse()
		{
			Agent.sendCommand("attack 0");
		}

		public static void RightClick()
		{
			Thread clicker = new Thread(() => {
				Use();
				Thread.Sleep(100);
				StopUse();
			});
			clicker.Start();
		}

		public static void Turn(double velocity)
		{
			Agent.sendCommand("turn " + velocity);
		}

		public static void Pitch(double velocity)
		{
			Agent.sendCommand("pitch " + velocity);
		}

		public static void Look(Vector2 direction)
		{
			Turn(direction.X);
			Pitch(direction.Y);
		}

		public static void StopEverything()
		{
			Look(Vector2.Zero);
			MoveStrafe(Vector2.Zero);
			StopUse();
			StopAttack();
			StopJump();
			UnCrouch();
		}
	}
}