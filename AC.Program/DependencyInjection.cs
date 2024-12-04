using AC.Services.DataReaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code;

public static class DependencyInjection
{
    public static void RegisterServices(HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<ITextFileReader, TextFileReader>();
    }
}
