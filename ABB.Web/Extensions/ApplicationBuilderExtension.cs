using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace ABB.Web.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UserStaticFilesModulesFolder(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Modules")),
                        RequestPath = "/Modules"
            });
        }
    }
}