using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace ABB.Web.JobSchedules
{
    public class ReportCleanUpJob : IJob
    {
        private readonly IConfiguration _configuration;

        public ReportCleanUpJob(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            // Access content root path and combine with your folder structure
            var folderPath = _configuration["ReportConfig:PhysicalPath"];

            try
            {
                // Get all files in the directory and delete them
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file: {file}");
                }

                // Get all subdirectories and delete them (and their contents)
                var directories = Directory.GetDirectories(folderPath);
                foreach (var directory in directories)
                {
                    Directory.Delete(directory, true); // Delete recursively
                    Console.WriteLine($"Deleted directory: {directory}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}