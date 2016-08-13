using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GladLive.Module.System.Library
{
	/// <summary>
	/// Module for registering services in <see cref="IServiceCollection"/> for GladLive.
	/// Inherit this class to setup DI services for the ASP server.
	/// </summary>
	public abstract class ServiceRegistrationModule : IRegisterModule
	{
		/// <summary>
		/// Provided service collection.
		/// </summary>
		protected IServiceCollection serviceCollection { get; }

		/// <summary>
		/// Optional DB context options for context registration.
		/// </summary>
		protected Action<DbContextOptionsBuilder> DbOptions { get; }

		/// <summary>
		/// Creates a new service registration module.
		/// </summary>
		/// <param name="services">The DI container registration collection.</param>
		/// <param name="options">Optional configureation options for any DBContext additions to services.</param>
		protected ServiceRegistrationModule(IServiceCollection services, Action<DbContextOptionsBuilder> options = null)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services), $"Provided ASP service {nameof(IServiceCollection)} is null.");

			DbOptions = options;
			serviceCollection = services;
		}

		public abstract void Register();
	}
}
