using ConsoleApp6.Dependencies;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp6
{
	public class DemoHostedService : IHostedService
	{
		private readonly IDependency dependency;

		public DemoHostedService(IDependency dependency)
		{
			this.dependency = dependency;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("stopped");

			this.dependency.Method(1, new List<string>() { "one" });
			await this.dependency.MethodAsync(1, new List<string>() { "one" });
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("stopped");
			return Task.CompletedTask;
		}
	}
}
