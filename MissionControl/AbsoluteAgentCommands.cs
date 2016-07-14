using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Malmo;

namespace MissionControl
{
	public static class AbsoluteAgentCommands
	{
		public static AgentHost Agent
		{ get; private set; }

		public static void Initialize(AgentHost agent)
		{
			Agent = agent;
		}

		public static void SetYaw(double thetaX)
		{
			Agent.sendCommand("setYaw " + thetaX);
		}

		public static void SetPitch(double thetaY)
		{
			Agent.sendCommand("setPitch " + thetaY);
		}

		public static void LookAt(Vector2 angle)
		{
			LookAt(angle.X, angle.Y);
		}
		public static void LookAt(double x, double y)
		{
			SetYaw(x);
			SetPitch(y);
		}

		public static void TpX(double x)
		{
			Agent.sendCommand("tpx " + x);
		}

		public static void TpY(double y)
		{
			Agent.sendCommand("tpy " + y);
		}

		public static void TpZ(double z)
		{
			Agent.sendCommand("tpz " + z);
		}

		public static void Tp(double x, double y, double z)
		{
			Agent.sendCommand($"tp {x} {y} {z}");
		}
		public static void Tp(Vector3 pos)
		{
			Tp(pos.X, pos.Y, pos.Z);
		}
	}
}