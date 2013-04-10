using CQT.Model;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CQT.Model.Geometry;
using System.Collections.Generic;

namespace CQT.Model
{
    [Serializable()]
	public class Weapon : Item
	{

		protected Character owner;
		protected WeaponInfo info;
		protected int lastShotTime; // milliseconds
		protected uint ammoLeft;
		
		public bool Full {
			get {return (ammoLeft==info.magSize); }
		}

		public Weapon (WeaponInfo.Type _type)
			: base(null, new Vector2(1,1)) // TODO: get real values
		{
			owner = null;
			info = Constants.Instance.getWeaponInfo(_type);
			lastShotTime = 0;
			ammoLeft = info.magSize;
		}

		public override Vector2 getPosition()
		{
			return (owner!=null) ? owner.getPosition() : position;
		}

		public WeaponInfo getInfo() {
			return info;
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

			float msPerShot = 60000 / (rotBonus * info.ROT);
			int nextShot = lastShotTime + (int)msPerShot;

			if (nextShot <= currentTime && ammoLeft > 0) {
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

            float imprec = info.imprecision;
            angle += (float)rand.NextDouble() * imprec - imprec / 2f;

			GameEnvironment environment = GameEnvironment.Instance;
            //if (environment == null) return;
            if (environment == null) throw new ArgumentNullException();

            environment.gunShotSound();

			//Point pos = new Point(getPosition());
			float cosA = (float)Math.Cos(angle);
			float sinA = (float)Math.Sin(angle);

            //Console.WriteLine(size);
            //float shift = size.X * 2f;
            float shift = 75 * .15f;
            //Point pos = new Point(getPosition() + new Vector2(cosA * shift, sinA * shift));
            Point pos = new Point(getPosition() + new Vector2(-sinA * shift, cosA * shift));

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

			/*Line wallShot = new Line (0,0,0,0);
			bool wallOnTraj = false;*/
            Line? wallShot = null;
			Line trajToWall = traj;
			
            /*
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
            */
            foreach (Line l in map.getVisionBlockingLines())
            {
                Point? intersct = trajToWall.Intersect(l);
                if (intersct.HasValue)
                {
                    wallShot = l;
                    trajToWall = new Line(origin, intersct.Value);
                }
            }


			if (trajToChar.length < trajToWall.length) {
				float preciseDmg = ((float)info.damage)*owner.getInfo().damageBonus;
				shootee.harm( (uint)preciseDmg );
                environment.addBulletTrail(trajToChar);

                Vector2 hitPoint = trajToChar.p2.asVector() + new Vector2(cosA * owner.body.size, sinA * owner.body.size);

                List<Point> pts = new List<Point>();
                int nbCouples = 4;
                float r1 = 20, r2 = 100, dist = 50;
                for (int i = 0; i < nbCouples; i++)
                {
                    pts.Add(new Point(hitPoint + nextVector2(r1)));
                    pts.Add(new Point(hitPoint + new Vector2(cosA * dist, sinA * dist) + nextVector2(r2)));
                    //if (random.NextDouble() > .7)
                        //pts.Add(new Point(hitPoint + new Vector2(cosA * dist, sinA * dist) + nextVector2(r2*1.5f)));
                        //pts.Add(new Point(pts[pts.Count - 1].asVector() + nextVector2(r1)));
                    if (random.NextDouble() > .7)
                        pts.Add(new Point(pts[pts.Count - 2].asVector() + nextVector2(r1)));
                }
                environment.bloodStains.Add(new Polyline(pts));
			}
            else if (wallShot.HasValue)
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
			
			ammoLeft--;
		}

        Random random = new Random();
        Vector2 nextVector2(float range)
        {
            return new Vector2(
                (float)random.NextDouble() * range - range / 2,
                (float)random.NextDouble() * range - range / 2);
        }

		
		public void reload() {
			// TODO: manage ammo packs (new Item ? in some List<Item> Character.equipment? ...)
			if (!Full) {
				ammoLeft = info.magSize;
			}
		}
	}
}

