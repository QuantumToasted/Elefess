using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Hosting.AspNetCore;

/// <summary>
/// Various extension methods for registering Git LFS related ASP.NET hosting types with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Git LFS defaults compatible with ASP.NET MVC with a service collection.
    /// <p> By default, this calls <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection, Action{MvcOptions})"/> to modify the default <see cref="SystemTextJsonInputFormatter"/> and <see cref="SystemTextJsonOutputFormatter"/> to support ONLY <c>application/vnd.git-lfs+json</c>,</p>
    /// <p> and calls <see cref="MvcCoreMvcBuilderExtensions.AddJsonOptions"/> to replace the default <see cref="JsonSerializerOptions.Encoder"/> with one that uses the <see cref="UnicodeRanges.BasicLatin"/> Unicode range.</p>
    /// </summary>
    /// <param name="services">The service collection to register defaults with.</param>
    /// <returns>An <see cref="IMvcBuilder"/> to chain additional MVC methods to.</returns>
    public static IMvcBuilder AddElefessMvcDefaults(this IServiceCollection services)
    {
        return services.AddMvc(options =>
            {
                var inputFormatter = options.InputFormatters.OfType<SystemTextJsonInputFormatter>().Single();
                inputFormatter.SupportedMediaTypes.Clear();
                inputFormatter.SupportedMediaTypes.Add(LfsUtil.Constants.Headers.Values.CONTENT_TYPE);

                var outputFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Single();
                outputFormatter.SupportedMediaTypes.Clear();
                outputFormatter.SupportedMediaTypes.Add(LfsUtil.Constants.Headers.Values.CONTENT_TYPE);
            })
            .AddJsonOptions(options => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin); });
    }
}