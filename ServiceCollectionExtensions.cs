using ConsoleApp6.Decorators;
using ConsoleApp6.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp6
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDependency(this IServiceCollection services)
		{
			services.AddTransient<Dependency>();
			services.AddTransient(s =>
			{
				// This way, it will leverage the dependency resolution for the internal instance,
				// instead of creating it manually
				IDependency instance = s.GetRequiredService<Dependency>();

				// Creating the instance, composing it with two decorators
				IDependency first = LoggerDecorator<IDependency>.Create(instance);
				IDependency second = TryCatchDecorator<IDependency>.Create(first);
				return second;
			});

			return services;
		}
	}
}
