using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers.FileHelper
{
    public class FileHelper : IFileHelper
    {

        public const string DeletePath = "wwwroot\\Deleted\\";

        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                var deletedFolder = Path.Combine(DeletePath, Path.GetFileNameWithoutExtension(filePath) + "deleted" + Path.GetExtension(filePath));
                File.Move(filePath, deletedFolder);
                //File.Delete(filePath);
            }
            else
            {
                Console.WriteLine("Böyle bir dosya bulunamadı");
            }
        }

        public string Update(IFormFile formFile, string filePath, string root)
        {
            if (File.Exists(filePath))
            {
                //Delete(filePath);
                var uptadedFolder = Path.Combine(DeletePath, Path.GetFileNameWithoutExtension(filePath) + "uptaded" + Path.GetExtension(filePath));
                File.Move(filePath, uptadedFolder);
            }
            else
            {
                Console.WriteLine("Böyle bir dosya bulunamadı");
                return null;
            }
            return Upload(formFile, root);
        }

        public string Upload(IFormFile formFile, string root)
        {
            if (formFile.Length > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string existsion = Path.GetExtension(formFile.FileName);
                string guid = GuidHelper.GuidHelper.CreateGuid();
                string filePath = guid + existsion;

                using (FileStream fileStream = File.Create(root + filePath))
                {
                    formFile.CopyTo(fileStream);
                    fileStream.Flush();
                    return filePath;
                }
            }
            return null;
        }



        public byte[] ConvertToBlob(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
