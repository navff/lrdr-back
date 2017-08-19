using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Camps.Tools;
using Models.HelpClasses;
using Models.Tools;
using NLog;

namespace Models.Operations
{
    public class FileOperations
    {
        private Logger _logger;
        private LrdrContext _context;

        public FileOperations(LrdrContext context)
        {
            _logger = LogManager.GetLogger("FileOperations");
            _context = context;
        }

        public async Task DeleteFile(int fileId)
        {
            var fileFromDb = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            if (fileFromDb == null)
            {
                throw new NotFoundException();
            }

            string baseFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string filepath = baseFilePath + GetRelativeFilePath(fileId, fileFromDb.Extension);

            // удалить файл картинки
            try
            {
                File.Delete(filepath);
            }
            catch (Exception e)
            {
                _logger.Error($"File was not found: {filepath}");
            }

            // удалить сущность из БД
            _context.Files.Remove(fileFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<FileDto> GetFile(int fileId)
        {
            var fileFromDb = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            var fileDto = Mapper.Map<FileDto>(fileFromDb);
            fileDto.Path = GetRelativeFilePath(fileFromDb.Id, fileFromDb.Extension);
            return fileDto;
        }

        public async Task<FileDto> Addfile(Entities.File file, string base64Data)
        {
            Contracts.Assert(!String.IsNullOrEmpty(file.Extension),
                             !String.IsNullOrEmpty(base64Data));


            // Сохранить файл в БД
            var fileInDb = _context.Files.Add(new Entities.File
            {
                Extension = file.Extension,
                LinkedObjectId = file.LinkedObjectId,
                LinkedObjectType = file.LinkedObjectType,
                Created = DateTimeOffset.Now
            });
            await _context.SaveChangesAsync();

            string fileName = AddLeftZeroesToId(fileInDb.Id) + "." + fileInDb.Extension;

            // куда складывать файлы
            string baseFilePath = AppDomain.CurrentDomain.BaseDirectory + "userfiles";

            // Создаём папку первого уровня
            string firstLevelFolderName = fileName.Substring(0, 3);
            CheckAndCreateDirectory(baseFilePath + "/" + firstLevelFolderName);

            //Создаём папку второго уровня
            string secondLevelFolderName = fileName.Substring(3, 3);
            string secondLevelPath = CheckAndCreateDirectory(baseFilePath + "/" + firstLevelFolderName + "/" + secondLevelFolderName);
            string fullPath = secondLevelPath + "/" + fileName;
            // Создать нужную папку в файловой системе
            // И сохнарить картинку туда
            FileHelpers.Base64ToFile(base64Data, fullPath, file.Extension, ModelsSettings.IMAGE_WIDTH);

            var result = Mapper.Map<FileDto>(fileInDb);
            result.Path = GetRelativeFilePath(result.Id, result.Extension);
            return result;
        }

      
        //-------------------------------------------------------------------

        /// <summary>
        /// Добавляет нули перед Id, дописывая строку до 10 символов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string AddLeftZeroesToId(int id)
        {
            string stringId = id.ToString();
            string result = "";
            for (var i = 0; i < 10 - stringId.Length; i++)
            {
                result = "0" + result;
            }
            return result + stringId;
        }


        /// <summary>
        /// Создаёт папку по указанному пути
        /// </summary>
        private string CheckAndCreateDirectory(string path)
        {
            Contracts.Assert(!String.IsNullOrEmpty(path));

            if (Directory.Exists(path))
            {
                return path;
            }

            try
            {
                Directory.CreateDirectory(path);
                return path;
            }
            catch (Exception e)
            {
                ErrorLogger.Log($"CANNOT CREATE DIRECTORY: {path}", e);
                throw ;
            }
        }


        /// <summary>
        /// Отдаёт относительный путь к файлу
        /// </summary>
        public string GetRelativeFilePath(int id, string extention)
        {
            Contracts.Assert(!String.IsNullOrEmpty(extention),
                             id != 0);

            string fileName = AddLeftZeroesToId(id) + "." + extention;
            string baseFilePath = "userfiles";
            string firstLevelFolderName = fileName.Substring(0, 3);
            string secondLevelFolderName = fileName.Substring(3, 3);
            string secondLevelPath = baseFilePath + "/" + firstLevelFolderName + "/" + secondLevelFolderName;

            return secondLevelPath + "/" + fileName;
        }
    }
}
