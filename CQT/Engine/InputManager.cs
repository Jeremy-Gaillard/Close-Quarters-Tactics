using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CQT.Model;

namespace CQT.Engine
{
	class InputManager
	{
		// TODO : change the place of this enum ?
        
		protected Player player;

		protected MouseState previousMouseState;
		protected KeyboardState previousKeyboardState;

		protected MouseState currentMouseState;
		protected KeyboardState currentKeyboardState;

		public InputManager (MouseState firstMouseState, KeyboardState firstKeyboardState, Player _player)
		{
			previousMouseState = firstMouseState;
			previousKeyboardState = firstKeyboardState;
            
			currentMouseState = firstMouseState;
			currentKeyboardState = firstKeyboardState;

			player = _player;
		}

		/// <summary>
		/// Updates the input manager with the latest input state
		/// </summary>
		/// <param name="newMouseState">The new mouse state</param>
		/// <param name="newKeyboardState">The new keyboard state</param>
		public void Update (MouseState newMouseState, KeyboardState newKeyboardState)
		{
			previousKeyboardState = currentKeyboardState;
			previousMouseState = currentMouseState;

			currentMouseState = newMouseState;
			currentKeyboardState = newKeyboardState;
		}

		/// <summary>
		/// Gets the distance travelled by the mouse between two updates of the input manager
		/// </summary>
		/// <returns>The lenght of the mouse's movement</returns>
		public Vector2 getMouseMovement ()
		{
			return new Vector2 (currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
		}

		/// <summary>
		/// Gets the list of commands issued by the player
		/// </summary>
		/// <returns>The list of the player's commands</returns>
		public List<CQT.Command.Command> getCommands (GameTime gameTime)
		{
			KeyboardState cks = currentKeyboardState;
			Character currentChar = player.getCharacter ();
			List<CQT.Command.Command> commands = new List<CQT.Command.Command> ();

			// TODONE : mutually exclude move left/right up/down ?

			Character.MovementDirection d = Character.MovementDirection.None;

			if (cks.IsKeyDown (Keys.Z)) {
				if (! cks.IsKeyDown (Keys.S)) {
					if (cks.IsKeyDown (Keys.Q) ^ cks.IsKeyDown (Keys.D)) {
						d = (cks.IsKeyDown (Keys.Q) ? Character.MovementDirection.UpLeft : Character.MovementDirection.UpRight);
					} else {
						d = Character.MovementDirection.Up;
					}
				}
			} else if (cks.IsKeyDown (Keys.S)) {
				if (cks.IsKeyDown (Keys.Q) ^ cks.IsKeyDown (Keys.D)) {
					d = (cks.IsKeyDown (Keys.Q) ? Character.MovementDirection.DownLeft : Character.MovementDirection.DownRight);
				} else {
					d = Character.MovementDirection.Down;
				}
			} else if (cks.IsKeyDown (Keys.Q) ^ cks.IsKeyDown (Keys.D)) {
				d = (cks.IsKeyDown (Keys.Q) ? Character.MovementDirection.Left : Character.MovementDirection.Right);
			}

			if (d != Character.MovementDirection.None) {
				commands.Add (new Command.Move (d, currentChar, gameTime.ElapsedGameTime.Milliseconds));
			}
			if (currentMouseState.LeftButton == ButtonState.Pressed) {
				commands.Add (new Command.Shoot(currentChar, (int)gameTime.TotalGameTime.TotalMilliseconds));
			}

			return commands;
		}

		public Vector2 getMousePosition ()
		{
			return new Vector2 (currentMouseState.X, currentMouseState.Y);
		}
	}
}
