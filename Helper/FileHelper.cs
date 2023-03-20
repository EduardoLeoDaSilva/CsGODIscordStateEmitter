
using CsGOStateEmitter.ValueObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsGOStateEmitter.Helper
{
    /// <summary>
    /// Class to perform operations with files
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Reads a file
        /// </summary>      
        /// <returns>Returns the text of the file</returns>
        public static string ReadFile(string path, string nameFile)
        {
            var filePath = Path.Combine(path, nameFile);
            if (File.Exists(filePath))
            {
                using (StreamReader str = new StreamReader(filePath))
                {
                    string text = str.ReadToEnd();
                    str.Close();
                    return text;
                }
            }
            return "";
        }

        /// <summary>
        /// Get files
        /// </summary>      
        /// <returns>Returns the text of the file</returns>
        public static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// Add a file by FormFile with result object
        /// </summary>      
        /// <returns>returns file name</returns>
        public static FileResult Upload(IFormFile file, string webRootPath, string FolderUpload)
        {
            FileResult fileResult = null;            
            if (file != null && !string.IsNullOrEmpty(webRootPath) && !string.IsNullOrEmpty(FolderUpload))
            {
                var fullPath = Path.Combine(webRootPath, FolderUpload);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                var extension = Path.GetExtension(file.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                string filePath = Path.Combine(fullPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileStream.Close();
                }
                var pathResolved = ReplaceBarsPath(@$"{FolderUpload}\{uniqueFileName}", @"\", "/");
                fileResult = new FileResult()
                {
                    Name = uniqueFileName,
                    Path = pathResolved
                };
            }
            return fileResult;
        }

        /// <summary>
        /// Delete file
        /// </summary>
        public static void Delete(string uploadsFolder, string nameFile)
        {
            if (string.IsNullOrEmpty(uploadsFolder) || string.IsNullOrEmpty(nameFile))
                return;
            var fileToDelete = Path.Combine(uploadsFolder, nameFile);
            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        public static void Delete(string pathFile)
        {
            if (string.IsNullOrEmpty(pathFile))
                return;
            if (File.Exists(pathFile))
                File.Delete(pathFile);
        }

        /// <summary>
        /// Exclude multiple files
        /// </summary>
        public static void DeleteMultiple(string uploadsFolder, List<string> lstFiles)
        {
            if (string.IsNullOrEmpty(uploadsFolder) || lstFiles == null)
                return;
            foreach (var name in lstFiles)
            {
                var fileToDelete = Path.Combine(uploadsFolder, name);
                if (File.Exists(fileToDelete))
                    File.Delete(fileToDelete);
            }
        }

        /// <summary>
        /// Replaces against bars with bars
        /// </summary>
        /// <returns>Returns the adjusted path</returns>
        public static string ReplaceBarsPath(string path, string oldChar, string newChar)
        {
            return path.Replace(oldChar, newChar);
        }
    }
}
