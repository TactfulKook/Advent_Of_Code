using AC.Services.DataReaders;
using Advent_of_Code;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

DependencyInjection.RegisterServices(builder);

using IHost host = builder.Build();

host.Run();