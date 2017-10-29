using UnityEngine;
using StarWars.Actions;
using Infra.Utils;

namespace StarWars.Brains {
	public class ShieldAndShootBrain : SpaceshipBrain {
		public override string DefaultName {
			get {
				return "ShieldAndShootShip";
			}
		}

		public override Color PrimaryColor {
			get {
				return new Color((float)0x255 / 0xFF, (float)0x30 / 0xFF, (float)0x30 / 0xFF, 1f);
			}
		}
		Gilad
		public override SpaceshipBody.Type BodyType {
			get {
				return SpaceshipBody.Type.TieFighter;
			}
		}

		private const float dangerDistance = 2.0f;
		private const int shieldPauseTime = 3;
		private int shieldPauseTimer = 0;
		private int shieldTime = 7;
		private int shieldTimer = 0;
		private Spaceship nearestSpaceship = null;

		/// <summary>
		/// If the defender feels attacked - it turns on the shield if it can,
		/// otherwise it starts circling right.
		/// The defender selects the closest ship as target and tries to shoot it.
		/// </summary>
		public override Action NextAction() {

			nearestSpaceship = findNearestShip ();

			// Should raise shield?
			if (shieldTimer > 0) {
				shieldTimer--;
			} else if (shieldTimer == 0 && spaceship.IsShieldUp) {
				return ShieldDown.action;
			} else if (shouldRaiseShield ()) {
				shieldTimer = shieldTime;
				return ShieldUp.action;
			} 

			if (nearestSpaceship == null) {
				// Not suppose to happen in a game with more than 1 player.
				return DoNothing.action;
			}

			// Should turn?
			var forwardVector = spaceship.Forward;
			var angle = spaceship.ClosestRelativePosition (nearestSpaceship).GetAngle (forwardVector);
			if (angle >= 12f) {
				return TurnLeft.action;
			} else if (angle <= -12f) {
				return TurnRight.action;
			}

			// Should shoot?
			var distance = spaceship.ClosestRelativePosition(nearestSpaceship).magnitude;
			if (distance < 10f && (!nearestSpaceship.IsShieldUp || nearestSpaceship.Energy < 5)) { 
				if (spaceship.CanShoot) {
					shieldPauseTimer = shieldPauseTime;
					return Shoot.action;
				} else {
					return DoNothing.action;
				}
			}

			return DoNothing.action;
		}

		/// <summary>
		/// Finds the nearest ship.
		/// </summary>
		/// <returns>The nearest ship.</returns>
		private Spaceship findNearestShip() {
			Spaceship closest = null;
			var minDistance = float.MaxValue;
			foreach (var ship in Space.Spaceships) {
				if (spaceship == ship) continue;
				float distance = spaceship.ClosestRelativePosition(ship).magnitude;
				if (distance < minDistance) {
					minDistance = distance;
					closest = ship;
				}
			}
			return closest;
		}

		/// <summary>
		/// Checks whether the shield should be rasied according to the shots around the spaceship.
		/// </summary>
		/// <returns><c>true</c>, if should raise the shield, <c>false</c> otherwise.</returns>
		private bool shouldRaiseShield() {

			// Check if the shield timer is due
			if (shieldPauseTimer > 0 || spaceship.IsShieldUp) {
				shieldPauseTimer--;
				return false;
			}

			foreach (var shot in Space.Shots) {

				// Check if the shot is close
				float distance = spaceship.ClosestRelativePosition(shot).magnitude;
				var shotVector = shot.Forward;
				if (distance <= dangerDistance && spaceship.CanRaiseShield) {
					return true;
				}
			}

			foreach (var ship in Space.Spaceships) {

				// Check if the spaceship is close
				if (spaceship == ship) continue;
				float distance = spaceship.ClosestRelativePosition(ship).magnitude;
				var shotVector = ship.Forward;
				if (distance <= dangerDistance && spaceship.CanRaiseShield) {
					return true;
				}
			}

			return false;
		}
	}
}