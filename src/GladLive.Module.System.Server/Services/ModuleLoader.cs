using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#if NETSTANDARD1_6
using System.Runtime.Loader;
#endif
using System.Threading.Tasks;

namespace GladLive.Module.System.Server
{
	/// <summary>
	/// Loads a module given the path.
	/// </summary>
	public class ModuleLoader
	{
		private Assembly loadedModuleAssembly { get; }

		/// <summary>
		/// Static ctor to setup assembly resolution event.
		/// This is needed because the paths may not be in the current directory.
		/// For now we'll just try to traverse the directory and load them.
		/// </summary
#if !NETSTANDARD1_6
		static ModuleLoader()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			foreach(string path in Directory.GetDirectories(Directory.GetCurrentDirectory()))
			{
				try
				{
					Assembly ass = Assembly.LoadFile($"{path}/{args.Name}");

					if (ass != null)
						return ass;
				}
				catch(Exception e)
				{
					continue;
				}
			}

			throw new InvalidOperationException($"Unable to find required assembly in any subdirectory. Name: {args.Name}");
		}
#else
		static ModuleLoader()
		{
			AssemblyLoadContext.Default.Resolving += Default_Resolving;
		}

		private static Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
		{
			foreach (string path in Directory.GetDirectories(Directory.GetCurrentDirectory()))
			{
				try
				{
					Assembly ass = arg1.LoadFromAssemblyPath($"{path}/{arg2.Name}");

					if (ass != null)
						return ass;
				}
				catch (Exception e)
				{
					continue;
				}
			}

			throw new InvalidOperationException($"Unable to find required assembly in any subdirectory. Name: {arg2.Name}");
		}
#endif

		public ModuleLoader(string path)
		{
			try
			{
#if NETSTANDARD1_6
				loadedModuleAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
#else
				loadedModuleAssembly = Assembly.LoadFile(path);
#endif
			}
			catch(Exception e)
			{
				throw new DllNotFoundException($"Unable to located module DLL {path}. Error: {e.Message}", e);
			}
			finally
			{
				//Verify it has been loaded
				if (loadedModuleAssembly == null)
					throw new InvalidOperationException($"Unable to located module DLL {path}. Loaded a null assembly.");
			}
		}
	}
}
