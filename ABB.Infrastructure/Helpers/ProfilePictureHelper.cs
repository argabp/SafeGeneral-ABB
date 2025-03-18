using System;
using System.IO;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ABB.Infrastructure.Helpers
{
    public class ProfilePictureHelper : IProfilePictureHelper
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _root;
        private readonly string _profilePictureFolder;

        public ProfilePictureHelper(IConfiguration config, IHostEnvironment root)
        {
            _config = config;
            _root = root;
            _profilePictureFolder = _config.GetSection("ProfilePictureFolder").Value.TrimEnd('/');
        }

        public string GetProfilePicture(string photo)
        {
            string photoPath;
            photoPath = Path.Combine(_profilePictureFolder, photo);
            var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot");
            var filePath = Path.Combine(wwwroot, photoPath.TrimStart('/'));
            photoPath = File.Exists(filePath) ? photoPath : "/img/default-profile-picture.png";
            return photoPath;
        }

        public async Task<string> Upload(IFormFile image)
        {
            string imageName = null;

            if (image != null)
            {
                imageName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(image.FileName)}";
                var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot");
                var root = Path.Combine(wwwroot, _profilePictureFolder.TrimStart('/'));
                var imgPath = Path.Combine(root, imageName);

                var fi = new FileInfo(imgPath);
                if (fi.Exists)
                {
                    fi.Delete();
                }

                using var fileStream = new FileStream(imgPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            return imageName;
        }

        public async Task<string> UploadToFolder(IFormFile image, string path)
        {
            string imageName = null;

            if (image != null)
            {
                var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot");
                var root = Path.Combine(wwwroot, path.TrimStart('/'));

                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);

                var imgPath = Path.Combine(root, image.FileName);
                imageName = imgPath;

                var fi = new FileInfo(imgPath);
                if (fi.Exists)
                    fi.Delete();

                using var fileStream = new FileStream(imgPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            return imageName;
        }

        public void UploadRawFile(string file, string fileName, string path)
        {
            byte[] fileBytes = Convert.FromBase64String(file);
            
            var apiDeployPath = _config.GetSection("DeployPathApi").Value;
            var webDeployPath = _config.GetSection("DeployPathWeb").Value;
            
            var wwwroot = Path.Combine(_root.ContentRootPath.Replace(apiDeployPath, webDeployPath), "wwwroot");
            var rootPath = wwwroot + path;
            
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            
            var filePath = Path.Combine(rootPath, fileName);

            FileStream ar = new FileStream(filePath, FileMode.Create);
            ar.Write(fileBytes, 0, fileBytes.Length);
            ar.Close();
        }

        public void UploadByteFile(byte[] file, string fileName, string path)
        {
            var fixPath = Path.Combine(_root.ContentRootPath, "wwwroot", "Reports", path);
            
            if (!Directory.Exists(fixPath))
                Directory.CreateDirectory(fixPath);
            
            var filePath = Path.Combine(fixPath, fileName);

            FileStream ar = new FileStream(filePath, FileMode.Create);
            ar.Write(file, 0, file.Length);
            ar.Close();
        }
    }
}