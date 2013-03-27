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
        
        protected MouseState previousMouseState;
        protected KeyboardState previousKeyboardState;

        protected MouseState currentMouseState;
        protected KeyboardState currentKeyboardState;

        public InputManager(MouseState firstMouseState, KeyboardState firstKeyboardState)
        {
            previousMouseState = firstMouseState;
            previousKeyboardState = firstKeyboardState;
            
            currentMouseState = firstMouseState;
            currentKeyboardState = firstKeyboardState;
        }

        /// <summary>
        /// Updates the input manager with the latest input state
        /// </summary>
        /// <param name="newMouseState">The new mouse state</param>
        /// <param name="newKeyboardState">The new keyboard state</param>
        public void Update(MouseState newMouseState, KeyboardState newKeyboardState)
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
        public Vector2 getMouseMovement()
        {
            return new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
        }

        /// <summary>
        /// Gets the list of commands issued by the player
        /// </summary>
        /// <returns>The list of the player's commands</returns>
        public List<CQT.Model.Player.Commands> getCommands()
        {
            List<CQT.Model.Player.Commands> commands = new List<CQT.Model.Player.Commands>();

            // TODO : mutually exclude move left/right up/down ?
            if(currentKeyboardState.IsKeyDown(Keys.Z))
            {
                commands.Add(CQT.Model.Player.Commands.MoveForward);
            }
            if(currentKeyboardState.IsKeyDown(Keys.S))
            {
                commands.Add(CQT.Model.Player.Commands.MoveBackwards);
            }
            if(currentKeyboardState.IsKeyDown(Keys.Q))
            {
                commands.Add(CQT.Model.Player.Commands.MoveLeft);
            }
            if(currentKeyboardState.IsKeyDown(Keys.D))
            {
                commands.Add(CQT.Model.Player.Commands.MoveRight);
            }

            return commands;
        }

        public Vector2 getMousePosition()
        {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }
    }
}
