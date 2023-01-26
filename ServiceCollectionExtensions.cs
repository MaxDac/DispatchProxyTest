using ConsoleApp6.Decorators;
using ConsoleApp6.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp6
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDependency<IInterface, IInstance>(this IServiceCollection services)
			where IInterface : class
			where IInstance : class, IInterface
		{
			services.AddTransient<IInstance>();
			services.AddTransient(s =>
			{
				// This way, it will leverage the dependency resolution for the internal instance,
				// instead of creating it manually
				IInterface instance = s.GetRequiredService<IInstance>();

				// Creating the instance, composing it with two decorators
				IInterface first = LoggerDecorator<IInterface>.Create(instance);
				IInterface second = TryCatchDecorator<IInterface>.Create(first);
				return second;
			});

			return services;
		}
	}
}
