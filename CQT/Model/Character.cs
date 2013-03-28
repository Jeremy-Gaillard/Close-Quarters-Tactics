using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CQT
{
	class Character : Sprite, PositionNotifier
	{
        public enum MovementDirection
        {
            Forwards,
            ForwardsLeft,
            Left,
            BackwardsLeft,
            Backwards,
            BackwardsRight,
            Right,
            ForwardsRight
        }

        protected List<PositionWatcher> positionWatchers = new List<PositionWatcher>();

		protected List<Weapon> weapons;
		protected Weapon currentWeapon;

        protected Single speed = 1.0f;  // TODO : use const
		protected uint hitPoints;

		public Character (Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _position, _size)
		{
			hitPoints = Constants.MAX_HITPOINTS;
			//equipDefaultWeapons ();
		}


		// WEAPON MANAGEMENT

		/// <summary>
		/// Reset character equipment to basic weapons.
		/// </summary>
		public void equipDefaultWeapons ()
		{
			//TODO: determine what is the basic equipment, unequip old one, equip new one
			weapons = new List<Weapon> ();
			
			Weapon aGun = new Weapon (Constants.WeaponType.Gun);

			pickUp (aGun);
			switchTo (aGun);
		}

		/// <summary>
		/// Allows this character to add a weapon to his stuff.
		/// </summary>
		/// <param name="w">The new weapon to add</param>
		/// <returns>true if successful</param>
		public bool pickUp (Weapon w)
		{
			if (weapons.Contains (w)) {
				Console.Write ("Character already had this weapon.");
				// TODO: is that even remotely useful?
				return false;
			}
			weapons.Add (w);
			w.setOwner (this);
			// TODO: add auto equip if "better"?
			return true;

		}

		/// <summary>
		/// Makes this character equip the weapon he's told.
		/// </summary>
		/// <param name="w">The weapon to equip</param>
		/// <returns>true if successful, false if weapon isn't part of his equipment</param>
		public bool switchTo (Weapon w)
			// TODO: add switchToNext, switchToBest?
		{
			if (weapons.Contains (w)) {
				currentWeapon = w;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Makes this character drop the weapon he's told.
		/// </summary>
		/// <param name="w">The weapon to drop</param>
		/// <returns>true if successful, false if weapon isn't part of his equipment</param>
		public bool drop (Weapon w)
		{
			if (weapons.Remove (w)) {
				w.drop ();
				return true;
			}
			return false;
		}

        public void Update(GameTime gameTime, List<Player.Commands> commands)
        {
            if (commands.Contains(Player.Commands.MoveBackwards))
            {
                move(gameTime, MovementDirection.Backwards);
            }
            if (commands.Contains(Player.Commands.MoveForward))
            {
                move(gameTime, MovementDirection.Forwards);
            }
            if (commands.Contains(Player.Commands.MoveLeft))
            {
                move(gameTime, MovementDirection.Left);
            }
            if (commands.Contains(Player.Commands.MoveRight))
            {
                move(gameTime, MovementDirection.Right);
            }
        }

        protected void move(GameTime gameTime, MovementDirection direction)
        {
            //Console.Out.WriteLine("moving ! " + direction.ToString() );
            Vector2 movement;
            Single cos = (Single)Math.Cos(rotation);
            Vector2 directionVector = new Vector2();
            switch (direction)
            {
                case MovementDirection.Forwards:
                    directionVector.X = (Single)Math.Cos(rotation);
                    directionVector.Y = (Single)Math.Sin(rotation);
                    break;
                // +PI
                case MovementDirection.Backwards: 
                    directionVector.X = -(Single)Math.Cos(rotation); 
                    directionVector.Y = -(Single)Math.Sin(rotation);
                    break;
                // +PI/2
                case MovementDirection.Left:
                    directionVector.X = (Single)Math.Sin(rotation);
                    directionVector.Y = -(Single)Math.Cos(rotation);
                    break;
                // -PI/2
                case MovementDirection.Right:
                    directionVector.X = -(Single)Math.Sin(rotation);
                    directionVector.Y = (Single)Math.Cos(rotation);
                    break;
                case MovementDirection.BackwardsLeft: directionVector *= -1; break;
                case MovementDirection.BackwardsRight: directionVector *= -1; break;
                case MovementDirection.ForwardsLeft: directionVector *= -1; break;
                case MovementDirection.ForwardsRight: directionVector *= -1; break;
            }
            //Console.Out.WriteLine(directionVector);
            movement = directionVector * gameTime.ElapsedGameTime.Milliseconds * speed;
            //Console.Out.WriteLine(movement);
            position += movement;
            notifyMovement(movement);
        }

        public void watch(PositionWatcher watcher)
        {
            positionWatchers.Add(watcher);
        }

        public void unWatch(PositionWatcher watcher)
        {
            positionWatchers.Remove(watcher);
        }

        protected void notifyMovement(Vector2 movement)
        {
            foreach (PositionWatcher pw in positionWatchers)
            {
                pw.notifyMovement(movement);
            }
        }
	}
}
