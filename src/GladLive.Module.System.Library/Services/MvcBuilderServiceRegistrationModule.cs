using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GladLive.Module.System.Library
{
	/// <summary>
	/// Module for registering services with the <see cref="IMvcBuilder"/> for GladLive.
	/// </summary>
	public abstract class MvcBuilderServiceRegistrationModule : IRegisterModule
	{
		/// <summary>
		/// Provided mvc builder.
		/// </summary>
		protected IMvcBuilder mvcBuilderService { get; }

		/// <summary>
		/// Creates a new MVC builder registration module.
		/// </summary>
		/// <param name="mvcBuilder">The Mvc Builder service.</param>
		protected MvcBuilderServiceRegistrationModule(IMvcBuilder mvcBuilder)
		{
			if (mvcBuilder == null)
				throw new ArgumentNullException(nameof(mvcBuilder), $"Provided ASP service {nameof(IMvcBuilder)} is null.");

			mvcBuilderService = mvcBuilder;
		}

		public abstract void Register();
	}
}
