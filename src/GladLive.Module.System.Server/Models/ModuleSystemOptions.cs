using GladLive.Module.System.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladLive.Module.System.Server
{
	/// <summary>
	/// JSON options object that maps to a *.json config file
	/// </summary>
	public class ModuleSystemOptions
	{
		/// <summary>
		/// Collection of paths that point to assemblies with <see cref="ApplicationConfigurationModule"/>s.
		/// ORDER MATTERS! These affect the HTTP pipeline so order is critical.
		/// </summary>
		public List<string> ApplicationConfigurationModulesPaths { get; set; }

		/// <summary>
		/// Collection of paths that point to assemblies with <see cref="ServiceRegistrationModule"/>s.
		/// Order should not matter for DI/Service registration.
		/// </summary>
		public List<string> ServiceRegistrationModulesPaths { get; set; }
	}
}
