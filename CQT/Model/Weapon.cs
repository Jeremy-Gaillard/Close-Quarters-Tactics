using CQT.Model;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CQT.Model
{
    [Serializable()]
	public class Weapon : Item
	{

		protected Character owner;
		protected WeaponInfo.Type type;
		protected int lastShotTime; // milliseconds




		public Weapon (WeaponInfo.Type _type)
			: base(null, new Vector2(1,1)) // TODO: get real values
		{
			owner = null;
			type = _type;
			lastShotTime = 0;
		}

		public override Vector2 getPosition()
		{
			return (owner!=null) ? owner.getPosition() : position;
		}

		/// <summary>
		/// Assigns this weapon to a character
		/// <param name="c">The character to set as owner</param>
		/// </summary>
		public void setOwner (Character c)
		{
			owner = c;
		}

		/// <summary>
		/// Unassigns this weapon, leaving it "on the floor".
		/// </summary>
		public void drop ()
		{
			if (owner!=null) {
				position = owner.getPosition();
				setOwner(null);
			}
			// TODO?: some more stuff to leave it lying on the map...
		}

		// currentTime = milliseconds
		public bool canShoot(float rotBonus, int currentTime) {
			// TODO
			// - check ROT
			// - check ammo in magazine

			float msPerShot = 60000 / (rotBonus * WeaponInfo.getROT(type));
			int nextShot = lastShotTime + (int)msPerShot;

			if (nextShot <= currentTime) {
				lastShotTime = currentTime;
				return true;
			}
			//Console.WriteLine ("msPerShot: "+msPerShot+"\nlastShotTime: "+lastShotTime+"\nnextShot: "+nextShot+"\ncurrentTime: "+currentTime);
			return false;
		}

        Random rand = new Random();
		public void shoot(float angle) {
			//System.Console.WriteLine ("Weapon.shoot(): "+WeaponInfo.getName (type)+" shooting at angle "+direction);
			//TODO

            float imprec = WeaponInfo.getImprecision(type);
            angle += (float)rand.NextDouble() * imprec - imprec / 2f;

			GameEnvironment environment = GameEnvironment.Instance;
            //if (environment == null) return;
            if (environment == null) throw new ArgumentNullException();
            
			Point pos = new Point(getPosition());
			float cosA = (float)Math.Cos(angle);
			float sinA = (float)Math.Sin(angle);
			
			Map.Map map = environment.Map;

			Vector2 direction = new Vector2(cosA, sinA);
			direction = Vector2.Multiply(direction, map.diagonal);

			Point origin = new Point(pos.x + (owner.getRadius() * cosA),
			                         pos.y + (owner.getRadius() * sinA));

			Line traj = new Line(origin.x, origin.y, pos.x + direction.X, pos.y + direction.Y);

			Character shootee = null;
			Line trajToChar = traj;

			foreach (Player p in environment.Players) {
				foreach (Character c in p.getCharacters()) {
					if (!c.isAlive || c==owner) continue;
					Point cPos = new Point(c.getPosition ());
					Tuple<Point?, Point?> intersct
						= traj.IntersectSegmentCircle(cPos, c.getRadius());
					Point? p1 = intersct.Item1;
					Point? p2 = intersct.Item2;

					if ( p1.HasValue || p2.HasValue ) {
						float d1 = (p1.HasValue ? Utils.distance(p1.Value, origin) : map.diagonal);
						float d2 = (p2.HasValue ? Utils.distance(p2.Value, origin) : map.diagonal);
						Point impact = ( d1 < d2 ? p1.Value : p2.Value );

						float distance = Utils.distance(impact, origin);

						if ( trajToChar.length > distance ) {
							shootee = c;
							trajToChar = new Line(origin, impact);
						}
					}
				}
			}

			Line wallShot = new Line (0,0,0,0);
			bool wallOnTraj = false;
			Line trajToWall = traj;
			
			foreach (Line l in map.getVisionBlockingLines()) {
				Point? intersct = Utils.LineIntersect(traj, l);
				if (intersct.HasValue) {
					float distance = Utils.distance(intersct.Value, pos);

					if ( trajToWall.length > distance ) {
						wallShot = l;
						wallOnTraj = true;
						trajToWall = new Line(origin, intersct.Value);
					}
				}
			}

			if (trajToChar.length < trajToWall.length) {
				float preciseDmg = ((float)WeaponInfo.getDamage(type))*CharacterInfo.getDamageBonus(owner.getCharType());
				shootee.harm( (uint)preciseDmg );
                environment.addBulletTrail(trajToChar);
			}
            else if (wallOnTraj)
            {
                Console.WriteLine("Weapon.shoot(): hit a wall at " + trajToWall.p2.ToString());
                environment.addBulletSpark(trajToWall.p2);
                environment.addBulletSpark(trajToWall.p2);
                environment.addBulletSpark(trajToWall.p2);
                environment.addBulletTrail(trajToWall);
            }
            else
            {
                environment.addBulletTrail(traj);
            }
		}
	}
}

