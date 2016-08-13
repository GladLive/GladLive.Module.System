using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladLive.Module.System.Server
{
	public class DatabaseConfigOptions
	{
		/// <summary>
		/// Indicates if an in-memory database provider should be utilized.
		/// </summary>
		public bool useInMemoryDatabase { get; set; }

		/// <summary>
		/// Indicates the connection string to use if <see cref="useInMemoryDatabase"/>
		/// is false.
		/// </summary>
		public string DatabaseConnectionString { get; set; }
	}
}
