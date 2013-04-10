using System;
using CQT.Model;

namespace CQT.Command
{
	public class Move : Command
	{
		protected Character charToMove;
		protected Character.MovementDirection direction;
		protected int milliseconds;

        public Move(Character.MovementDirection _direction, Character _charToMove, int _milliseconds)
            : base(Type.Move)
		{
			direction = _direction;
			charToMove = _charToMove;
			milliseconds = _milliseconds;
		}

		public override void execute ()
		{
			if ( charToMove.isAlive ) {
				charToMove.move (milliseconds, direction);
			}
		}
	}
}

