using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Hosting.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddElefessMvcDefaults(this IServiceCollection services)
    {
        return services.AddMvc(options =>
            {
                var inputFormatter = options.InputFormatters.OfType<SystemTextJsonInputFormatter>().Single();
                inputFormatter.SupportedMediaTypes.Clear();
                inputFormatter.SupportedMediaTypes.Add("application/vnd.git-lfs+json");

                var outputFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Single();
                outputFormatter.SupportedMediaTypes.Clear();
                outputFormatter.SupportedMediaTypes.Add("application/vnd.git-lfs+json");
            })
            .AddJsonOptions(options => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin); });
    }
}