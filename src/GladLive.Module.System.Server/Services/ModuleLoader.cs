using Microsoft.Extensions.DependencyModel;
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
			foreach(string path in Directory.GetDirectories(Directory.GetCurrentDirectory()).Concat(new string[] { Directory.GetCurrentDirectory() }))
			{
				try
				{
					Assembly ass = Assembly.LoadWithPartialName(Path.Combine(path,args.Name).TrimEnd(".dll".ToArray()));

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
			string pathName = arg2.Name.Contains(".dll") ? arg2.Name : $"{arg2.Name}.dll";

			//Try to load it from the current directory first.
			using (FileStream fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), pathName), FileMode.Open))
			{
				Assembly ass = arg1.LoadFromStream(fs);

				if (ass != null)
					return ass;
			}

			//recurse through every subdirectory and subdirectories' subdirectories.
			return TryResolveFromDirectory(arg1, Directory.GetCurrentDirectory(), pathName);
		}

		private static Assembly TryResolveFromDirectory(AssemblyLoadContext context, string path, string pathName)
		{
			foreach (string subDir in Directory.GetDirectories(path))
			{
				try
				{
					using (FileStream fs = new FileStream(Path.Combine(path, pathName), FileMode.Open))
					{
						Assembly ass = context.LoadFromStream(fs);

						if (ass != null)
							return ass;
					}
				}
				catch (Exception e)
				{
					continue;
				}
			}

			//recurse to subdirectories
			foreach(string subDir in Directory.GetDirectories(path))
			{
				Assembly ass = TryResolveFromDirectory(context, subDir, pathName);

				if (ass == null)
					continue;
				else
					return ass;
			}

			//Wasn't in this tree
			return null;
		}
#endif

		public ModuleLoader(string path)
		{
			try
			{
#if NETSTANDARD1_6
				loadedModuleAssembly = Default_Resolving(AssemblyLoadContext.Default, new AssemblyName() { Name = path });
#else
				//Don't really want to have to have users specify fully qualified names
#pragma warning disable CS0618 // Type or member is obsolete
				loadedModuleAssembly = Assembly.LoadWithPartialName(path.TrimEnd(".dll".ToArray()));
#pragma warning restore CS0618 // Type or member is obsolete

#endif
			}
			catch(Exception e)
			{
				throw new DllNotFoundException($"Unable to locate module DLL {path}. Error: {e.Message}", e);
			}
			finally
			{
				//Verify it has been loaded
				if (loadedModuleAssembly == null)
					throw new InvalidOperationException($"Unable to locate and load module DLL {path}. Loaded a null assembly.");
			}
		}

		public IEnumerable<Type> GetTypes<TTargetType>()
			where TTargetType : class
		{
			return this.loadedModuleAssembly.GetTypes()
				.Where(t => typeof(TTargetType).IsAssignableFrom(t)); //find all types that are children of TTargetType
		}

		public Type GetType<TTargetType>()
			where TTargetType : class
		{
			return this.loadedModuleAssembly.GetTypes()
				.First(t => typeof(TTargetType).IsAssignableFrom(t)); //find the TTargetType child.
		}
	}
}
