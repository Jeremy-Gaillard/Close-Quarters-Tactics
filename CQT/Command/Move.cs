using System;
using CQT.Model;

namespace CQT.Command
{
	public class Move : Command
	{
		protected Character charToMove;
		protected Character.MovementDirection direction;
		protected int milliseconds;

		public Move (Type _type, Character.MovementDirection _direction, Character _charToMove, int _milliseconds)
			: base(_type)
		{
			direction = _direction;
			charToMove = _charToMove;
			milliseconds = _milliseconds;
		}

		public override void execute ()
		{
			charToMove.move (milliseconds, direction);
		}
	}
}

