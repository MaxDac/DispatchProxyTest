// See https://aka.ms/new-console-template for more information
using ConsoleApp6;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

await Host.CreateDefaultBuilder()
	.ConfigureServices(services =>
	{
		services.AddDependency();
		services.AddHostedService<DemoHostedService>();
	})
	.RunConsoleAsync();
