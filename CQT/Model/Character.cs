using System;
using CQT.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CQT.Model.Physics;

namespace CQT.Model
{
    [Serializable()]
    public struct LightCharacter
    {
        public LightCharacter(Character c)
        {
            textureName = c.getTextureName();
            position = c.body.position;
            size = c.getSize();
        }

        //Weapon public weapon; TODO : add
        // TODO : add currentHP
        public String textureName;
        public Vector2 position;
        public Vector2 size;
    }
    
	public class Character : Entity
	{
		public enum MovementDirection
		{
			None,
			Up,
			UpLeft,
			Left,
			DownLeft,
			Down,
			DownRight,
			Right,
			UpRight
		}
		public enum State
		{
			Standing,
			Reloading,
			Dead
		}
		protected State currentState;
		protected int timeInState;
		
		protected CharacterInfo info;

		protected List<Weapon> weapons;
		protected Weapon currentWeapon;

		protected uint hitPoints;
		public bool isAlive {
			get { return (currentState!=State.Dead); }
		}

        public readonly Body body;

        public Character (String _texture, PhysicsEngine engine, Vector2 _position, Vector2 _size, float bodySize)
			: base(_texture, _size)
		{
			info = Constants.Instance.getCharacterInfo(CharacterInfo.Type.Default);
            initCharacter();
            //body.setPosition(_position);
            body = new Body(bodySize, _position); // TODO: size as a FLOAT instead?!
            engine.AddBody(body);
		}

		public Character (CharacterInfo.Type _type, String _texture, PhysicsEngine engine, Vector2 _position, Vector2 _size, float bodySize)
			: base(_texture, _size)
		{
			info = Constants.Instance.getCharacterInfo(_type);
			initCharacter ();
            //body.setPosition(_position);
            body = new Body(bodySize, _position); // TODO: size as a FLOAT instead?!
            engine.AddBody(body);
		}

		public void initCharacter ()
		{
			setState(State.Standing);
			hitPoints = info.maxHP;
			equipDefaultWeapons ();
		}

		// WEAPON MANAGEMENT

		/// <summary>
		/// Reset character equipment to basic weapons.
		/// </summary>
		public void equipDefaultWeapons ()
		{
			//TODO: determine what is the basic equipment, unequip old one, equip new one
			weapons = new List<Weapon> ();

            //Weapon aGun = new Weapon (WeaponInfo.Type.Gun);
            Weapon aGun = new Weapon(WeaponInfo.Type.Assault);

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

		public void reload()
		{
			if (currentWeapon!=null && !currentWeapon.Full) {
				currentWeapon.reload();
				setState(State.Reloading);
			}
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
		public bool drop(Weapon w)
		{
			if (weapons.Remove (w)) {
				w.drop ();
				return true;
			}
			return false;
		}

        public bool drop()
        {
            if (currentWeapon != null)
            {
                currentWeapon.drop();
                weapons.Remove(currentWeapon);
                currentWeapon = null;
                return true;
            }
            return false;
        }

        //Random rand = new Random();
		public void shoot (int now) {
            if (currentWeapon != null && currentWeapon.canShoot(info.ROTBonus, now))
            {
                /*
                float imprec = WeaponInfo.getImprecision(currentWeapon.GetType());
                float angle = rotation + (float)rand.NextDouble() * imprec - imprec / 2f;
                */
                currentWeapon.shoot(rotation);
			}
			else {
				// play *click-click* noise?
				//Console.WriteLine ("Character.shoot(): Cannot shoot...");
			}
		}

		public bool canShoot() {
			return (isAlive && (currentState!=State.Reloading));
		}		
		
		// END (WEAPON MANAGEMENT)

		public void Update (GameTime gameTime)
		{
//			if (commands.Contains (Player.Commands.MoveDown)) {
//				if (commands.Contains (Player.Commands.MoveLeft)) {
//					move (gameTime, MovementDirection.DownLeft);
//				} else if (commands.Contains (Player.Commands.MoveRight)) {
//					move (gameTime, MovementDirection.DownRight);
//				} else {
//					move (gameTime, MovementDirection.Down);
//				}
//			} else if (commands.Contains (Player.Commands.MoveUp)) {
//				if (commands.Contains (Player.Commands.MoveLeft)) {
//					move (gameTime, MovementDirection.UpLeft);
//				} else if (commands.Contains (Player.Commands.MoveRight)) {
//					move (gameTime, MovementDirection.UpRight);
//				} else {
//					move (gameTime, MovementDirection.Up);
//				}
//			} else if (commands.Contains (Player.Commands.MoveLeft)) {
//				move (gameTime, MovementDirection.Left);
//			} else if (commands.Contains (Player.Commands.MoveRight)) {
//				move (gameTime, MovementDirection.Right);
//			}
			timeInState+= gameTime.ElapsedGameTime.Milliseconds;
			
			switch (currentState) {
			case State.Standing: break;
			case State.Dead: break;

			case State.Reloading:
				float baseReloadTime = (currentWeapon==null ? 0 : currentWeapon.getInfo().reloadTime);
				int reloadTime = 1000*(int)(baseReloadTime / info.reloadSpeed);
//				Console.WriteLine("baseReloadTime:" + baseReloadTime + " --- reloadSpeed: "+info.reloadSpeed);
//				Console.WriteLine("time in state: "+timeInState+" --- reloadTime: "+reloadTime);
				if (timeInState > (reloadTime)) {
					setState(State.Standing); // TODO: when we'll have 1 state/command, change this (ie with moving, ...)
				}
		
				break;
			
			default: break;
			}
		}

		public void move (MovementDirection direction) // TODONE rm useless param "millisecond"
		{
			//Console.Out.WriteLine("moving ! " + direction.ToString() );
			Vector2 movement;
			Single cos = (Single)Math.Cos (rotation);
			switch (direction) {
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
				movement.X = -0.707f; // LOL c'est quoi �a ?? pk c'est pas dans une constante ?!
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
			///movement = movement * milliseconds * speed; // ce n'est pas � Character de faire ce genre de trucs (millisecond)
			//Console.Out.WriteLine (movement);
			///position += movement;
            body.tryMove(movement * info.speedBonus);
		}

		public void harm(uint damage) {
			if (damage >= hitPoints) {
				hitPoints = 0;
				Console.WriteLine("Blerg!");
                textureName = "dead";   // TODO : change this asap
				setState(State.Dead);
			}
			else {
				hitPoints-= damage;
				Console.WriteLine("Ouch.");
			}
		}

        public override Vector2 getPosition()
        {
            return body.getPosition();
        }
		public float getRadius()
		{
			return (getSize().X)/2F; // TODO: is that actually correct?
		}
		public CharacterInfo getInfo() {
			return info;
		}
		
		public void setState(State s) {
			// could have used property setter, but what would be the name of the property... ? setter is just as good
			Console.WriteLine("old state: "+currentState.ToString()+" --- new state: "+s.ToString());
			currentState = s;
			timeInState = 0;
		}

		public override void setRotation (float _rotation)
		{
			if (isAlive) {
				base.setRotation (_rotation);
			}
		}
	
	}
}
