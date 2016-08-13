using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladLive.Module.System.Library
{
	/// <summary>
	/// A registerable module.
	/// </summary>
	public interface IRegisterModule
	{
		/// <summary>
		/// Registers the module.
		/// </summary>
		void Register();
	}
}
