using UnityEngine;
using StarWars.Actions;
using Infra.Utils;

namespace StarWars.Brains {
	public class DrunkBrain : SpaceshipBrain {
		public override string DefaultName {
			get {
				return "drunk";
			}
		}
		public override Color PrimaryColor {
			get {
				return new Color((float)0xF9 / 0xFF, (float)0x6C / 0xFF, (float)0xC6 / 0xFF, 1f);
			}
		}
		public override SpaceshipBody.Type BodyType {
			get {
				return SpaceshipBody.Type.TieFighter;
			}
		}
		private Spaceship target = null;
		private int shieldTimer = 3;
		public override Action NextAction() {
			
			// Find nearest target
			var distance = float.MaxValue;
			foreach (var ship in Space.Spaceships) {
				// Make sure not to target self or dead spaceships and choose the closest one.
				if (spaceship != ship && ship.IsAlive) {
					if (spaceship.ClosestRelativePosition (ship).magnitude < distance) {
						target = ship;
						distance = spaceship.ClosestRelativePosition (ship).magnitude;
					}
				}
			}
			float mistake = Random.Range (-20f, 20f);
			if (target != null) {
				// Try to kill the target, but adding a little mistake to the aim.
				var pos = spaceship.ClosestRelativePosition(target);
				var forwardVector = spaceship.Forward;
				var angle = pos.GetAngle(forwardVector) + mistake;
				if (angle >= 10f) return TurnLeft.action;
				if (angle <= -10f) return TurnRight.action;
				if (distance < 15f && (!target.IsShieldUp)) { 
					return spaceship.CanShoot ? Shoot.action : DoNothing.action;
				}

			}
			/// Randomly put shield up and down for 3 turns
			if (spaceship.IsShieldUp) {
				shieldTimer--;
				if (shieldTimer == 0) {
					shieldTimer = 3;
					return ShieldDown.action;

				}
			}
			else if (spaceship.CanRaiseShield) {
				int shield = Random.Range(0,10);
				if(shield < 5){
					return ShieldUp.action;
				}

			}
			return DoNothing.action;
		}
	}
}

