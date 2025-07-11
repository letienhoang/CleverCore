﻿using System.IO;

namespace CleverCore.WebApp.Services
{
    public class FileService : IFileService
    {
        public bool CheckDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public FileStream CreateFile(string filePath)
        {
            return File.Create(filePath);
        }

        public bool CheckFileExist(FileInfo file)
        {
            return file.Exists;
        }

        public void DeleteFile(FileInfo file)
        {
            file.Delete();
        }
    }
}
