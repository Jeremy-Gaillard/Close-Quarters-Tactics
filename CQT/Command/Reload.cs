using System;
using CQT.Model;

namespace CQT.Command
{
	public class Reload : Command
	{
		protected Character reloader;
		
		public Reload(Character _reloader)
			: base(Type.Reload)
		{
			reloader = _reloader;
		}
		
		public override void execute()
		{
			if ( reloader.isAlive ) {
				reloader.reload();
			}
		}
	}
}

