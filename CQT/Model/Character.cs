using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CQT
{
	class Character : Entity
	{
		public enum MovementDirection
		{
			Up,
			UpLeft,
			Left,
			DownLeft,
			Down,
			DownRight,
			Right,
			UpRight
		}
		CharacterInfo.Type type;


		protected List<Weapon> weapons;
		protected Weapon currentWeapon;

		protected Single speed;
		protected uint hitPoints;
		protected uint maxHP;

		public Character (Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _position, _size)
		{
			type = CharacterInfo.Type.None;
			initCharacter ();
		}

		public Character (CharacterInfo.Type _type, Texture2D _texture, Vector2 _position, Vector2 _size)
			: base(_texture, _position, _size)
		{
			type = _type;
			initCharacter ();
		}

		public void initCharacter ()
		{
			hitPoints = maxHP = CharacterInfo.getMaxHP (type);
			speed = CharacterInfo.getSpeed (type);
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
			
			Weapon aGun = new Weapon (WeaponInfo.Type.Gun);

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

		public void Update (GameTime gameTime, List<Player.Commands> commands)
		{
			if (commands.Contains (Player.Commands.MoveDown)) {
                if(commands.Contains(Player.Commands.MoveLeft))
                {
                    move(gameTime, MovementDirection.DownLeft);
                }
                else if(commands.Contains(Player.Commands.MoveRight))
                {
                    move(gameTime, MovementDirection.DownRight);
                }
                else
                {
				    move(gameTime, MovementDirection.Down);
                }
			}
			else if (commands.Contains (Player.Commands.MoveUp)) {
				if(commands.Contains(Player.Commands.MoveLeft))
                {
                    move(gameTime, MovementDirection.UpLeft);
                }
                else if(commands.Contains(Player.Commands.MoveRight))
                {
                    move(gameTime, MovementDirection.UpRight);
                }
                else
                {
				    move(gameTime, MovementDirection.Up);
                }
			}
			else if (commands.Contains (Player.Commands.MoveLeft)) {
				move(gameTime, MovementDirection.Left);
			}
			else if (commands.Contains (Player.Commands.MoveRight)) {
				move(gameTime, MovementDirection.Right);
			}
		}

        protected void move(GameTime gameTime, MovementDirection direction)
        {
            //Console.Out.WriteLine("moving ! " + direction.ToString() );
            Vector2 movement;
            Single cos = (Single)Math.Cos(rotation);
            switch (direction)
            {
                case MovementDirection.Up:
                    movement.X = 0;
                    movement.Y = -1;
                    break;
                case MovementDirection.Down:
                    movement.X = 0;
                    movement.Y = 1;
                    break;
                case MovementDirection.Left:
                    movement.X = -1;
                    movement.Y = 0;
                    break;
                case MovementDirection.Right:
                    movement.X = 1;
                    movement.Y = 0;
                    break;
                case MovementDirection.DownLeft:
                    movement.X = -0.707f;
                    movement.Y = 0.707f;
                    break;
                case MovementDirection.DownRight:
                    movement.X = 0.707f;
                    movement.Y = 0.707f;
                    break;
                case MovementDirection.UpLeft:
                    movement.X = -0.707f;
                    movement.Y = -0.707f;
                    break;
                case MovementDirection.UpRight:
                    movement.X = 0.707f;
                    movement.Y = -0.707f;
                    break;
                default:
                    movement.X = 0;
                    movement.Y = 0;
                    break;
            }
            //Console.Out.WriteLine (directionVector);
            movement = movement * gameTime.ElapsedGameTime.Milliseconds * speed;
            //Console.Out.WriteLine (movement);
            position += movement;
        }
	}
}
