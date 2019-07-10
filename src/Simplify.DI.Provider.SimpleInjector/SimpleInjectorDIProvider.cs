﻿using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Simplify.DI.Provider.SimpleInjector
{
	/// <summary>
	/// Simple Injector DI container provider implementation
	/// </summary>
	public class SimpleInjectorDIProvider : IDIContainerProvider
	{
		private Container _container;

		/// <summary>
		/// The IOC container
		/// </summary>
		public Container Container
		{
			get => _container ??
				   (_container = new Container { Options = { DefaultScopedLifestyle = new AsyncScopedLifestyle() } });
			set => _container = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Resolves the specified service type.
		/// </summary>
		/// <param name="serviceType">Type of the service.</param>
		/// <returns></returns>
		public object Resolve(Type serviceType)
		{
			return Container.GetInstance(serviceType);
		}

		/// <summary>
		/// Registers the specified service type with corresponding implementation type.
		/// </summary>
		/// <param name="serviceType">Service type.</param>
		/// <param name="implementationType">Implementation type.</param>
		/// <param name="lifetimeType">Lifetime type of the registering services type.</param>
		public void Register(Type serviceType, Type implementationType, LifetimeType lifetimeType)
		{
			switch (lifetimeType)
			{
				case LifetimeType.PerLifetimeScope:
					Container.Register(serviceType, implementationType, Lifestyle.Scoped);
					break;

				case LifetimeType.Singleton:
					Container.Register(serviceType, implementationType, Lifestyle.Singleton);
					break;

				case LifetimeType.Transient:
					Container.Register(serviceType, implementationType, Lifestyle.Transient);
					break;
			}
		}

		/// <summary>
		/// Registers the specified provider.
		/// </summary>
		/// <typeparam name="TService">Concrete type.</typeparam>
		/// <param name="instanceCreator">The instance creator.</param>
		/// <param name="lifetimeType">Lifetime type of the registering concrete type.</param>
		public void Register<TService>(Func<IDIResolver, TService> instanceCreator, LifetimeType lifetimeType)
			where TService : class
		{
			switch (lifetimeType)
			{
				case LifetimeType.PerLifetimeScope:
					Container.Register(() => instanceCreator(this), Lifestyle.Scoped);
					break;

				case LifetimeType.Singleton:
					Container.Register(() => instanceCreator(this), Lifestyle.Singleton);
					break;

				case LifetimeType.Transient:
					Container.Register(() => instanceCreator(this), Lifestyle.Transient);
					break;
			}
		}

		/// <summary>
		/// Begins the lifetime scope.
		/// </summary>
		/// <returns></returns>
		public ILifetimeScope BeginLifetimeScope()
		{
			return new SimpleInjectorLifetimeScope(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		public void Dispose()
		{
			_container?.Dispose();
		}

		/// <summary>
		/// Performs container objects graph verification
		/// </summary>
		public void Verify()
		{
			Container.Verify();
		}
	}
}