using System.IO;

namespace CleverCore.WebApp.Services
{
    public interface IFileService
    {
        bool CheckDirectoryExist(string path);

        void CreateDirectory(string path);

        FileStream CreateFile(string filePath);

        bool CheckFileExist(FileInfo file);

        void DeleteFile(FileInfo file);
    }
}
