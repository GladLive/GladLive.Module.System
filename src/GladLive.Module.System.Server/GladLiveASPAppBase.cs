using GladLive.Module.System.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladLive.Module.System.Server
{
	/// <summary>
	/// Base type for a GladLive application.
	/// </summary>
	public class GladLiveASPAppBase
	{
		/// <summary>
		/// Configuration service.
		/// </summary>
		public IConfigurationRoot Configuration { get; }

		/// <summary>
		/// Called on startup. ASP provides <see cref="IHostingEnvironment"/>.
		/// </summary>
		/// <param name="env"></param>
		public GladLiveASPAppBase(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("database.json", optional: false, reloadOnChange: true) //database config file
				.AddJsonFile("modules.json", optional: false, reloadOnChange: true) //add the modules json file.
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}


		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging();
			services.AddOptions();

			//configures the JSON DTO for the module options and database options
			services.Configure<ModuleSystemOptions>(Configuration.GetSection(nameof(ModuleSystemOptions)));

			services.Configure<DatabaseConfigOptions>(Configuration.GetSection(nameof(DatabaseConfigOptions)));

			//temporarily generate a service provider
			IServiceProvider provider = services.BuildServiceProvider();

			IOptions<ModuleSystemOptions> moduleOptions = provider.GetService<IOptions<ModuleSystemOptions>>();
			IOptions<DatabaseConfigOptions> databaseOptions = provider.GetService<IOptions<DatabaseConfigOptions>>();

			if (moduleOptions == null || moduleOptions.Value == null)
				throw new InvalidOperationException($"Couldn't create {nameof(ModuleSystemOptions)} from modules.json.");

			if (moduleOptions == null || databaseOptions.Value == null)
				throw new InvalidOperationException($"Couldn't create {nameof(DatabaseConfigOptions)} from database.json.");

			//Add DB services depending on the config
			if (databaseOptions.Value.useInMemoryDatabase)
				services.AddEntityFrameworkInMemoryDatabase();
			else
				services.AddEntityFrameworkSqlServer();

			//Register Mvc services and aquire builder
			IMvcBuilder mvcBuilder = services.AddMvc();

			try
			{
				if(moduleOptions.Value.ServiceRegistrationModulesPaths != null)
					//TODO: Refactor
					//Find all the MvcBuilder and ServiceReg modules
					foreach (string path in moduleOptions.Value.ServiceRegistrationModulesPaths)
					{
						ModuleLoader loader = new ModuleLoader(path);

						foreach (Type serviceRegisterType in loader.GetTypes<ServiceRegistrationModule>())
						{
							IRegisterModule module = Activator.CreateInstance(serviceRegisterType, new object[] { services, (Action<DbContextOptionsBuilder>)((DbContextOptionsBuilder options) =>
							{
								if(databaseOptions.Value.useInMemoryDatabase)
									options.UseInMemoryDatabase();
								else
									options.UseSqlServer(databaseOptions.Value.DatabaseConnectionString);
							}) }) as IRegisterModule;

							//Register the module.
							module.Register();
						}

						foreach (Type serviceRegisterType in loader.GetTypes<MvcBuilderServiceRegistrationModule>())
						{
							IRegisterModule module = Activator.CreateInstance(serviceRegisterType, new object[] { mvcBuilder }) as IRegisterModule;

							//Register the module.
							module.Register();
						}
					}
			}
			catch(Exception e)
			{
				throw new InvalidOperationException($"Encountered Exception in Module registration: {e.Message}", e);
			}
		}

		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			//TODO: Implement better logger handling.
			loggerFactory.AddConsole(LogLevel.Information);

			IOptions<ModuleSystemOptions> options = app.ApplicationServices.GetService<IOptions<ModuleSystemOptions>>();

			if (options.Value.ApplicationConfigurationModulesPaths != null)
				foreach (string path in options.Value.ApplicationConfigurationModulesPaths)
				{
					//Only one should be in the app.
					//ORDER MATTERS!
					ModuleLoader loader = new ModuleLoader(path);

					IRegisterModule module = Activator.CreateInstance(loader.GetType<ApplicationConfigurationModule>(), new object[] { app, loggerFactory }) as IRegisterModule;

					//Register the module.
					module.Register();
				}

			//Register MVC last in the pipeline. NO OTHER MODULE SHOULD REGISTER THIS!
			app.UseMvc();
		}
	}
}
