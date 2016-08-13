using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace GladLive.Module.System.Library
{
	/// <summary>
	/// Module for configuring the pipeline and application for GladLive.
	/// Inherit this class to configure <see cref="IApplicationBuilder"/> for the ASP server.
	/// </summary>
	public class ServiceConfigurationModule
	{
		/// <summary>
		/// The application builder configuration service.
		/// </summary>
		protected IApplicationBuilder applicationBuilder { get; }

		/// <summary>
		/// The logger factory configuration service.
		/// </summary>
		protected ILoggerFactory loggerFactoryService { get; }

		/// <summary>
		/// Creates a new service configuration module.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="loggerFactory"></param>
		public ServiceConfigurationModule(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app), $"Provided ASP service {nameof(IApplicationBuilder)} is null.");

			if (loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory), $"Provided ASP service {nameof(ILoggerFactory)} is null.");

			loggerFactoryService = loggerFactory;
			applicationBuilder = app;
		}
	}
}
