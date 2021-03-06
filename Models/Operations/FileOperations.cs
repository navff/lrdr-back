﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Operations;
using AutoMapper;
using Camps.Tools;
using Models.Entities;
using Models.HelpClasses;
using Models.Tools;
using NLog;
using File = System.IO.File;

namespace Models.Operations
{
    public class FileOperations
    {
        private Logger _logger;
        private LrdrContext _context;
        private CommentOperations _commentOperations;
        private UserOperations _userOperations;

        public FileOperations(LrdrContext context)
        {
            _logger = LogManager.GetLogger("FileOperations");
            _context = context;
            _userOperations = new UserOperations(_context);
            _commentOperations = new CommentOperations(_context);
        }

        public async Task DeleteFile(int fileId)
        {
            var fileFromDb = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            if (fileFromDb == null)
            {
                throw new NotFoundException();
            }

            string baseFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string filepath = baseFilePath + "\\" + GetRelativeFilePath(fileId, fileFromDb.Extension);

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
            fileDto.PathThumb = GetRelativeFilePath(fileFromDb.Id, "thumb.jpg");
            return fileDto;
        }

        public async Task<FileDto> GetFile(string code)
        {
            var fileFromDb = await _context.Files.FirstOrDefaultAsync(f => f.Code == code);
            if (fileFromDb == null) return null;
            var fileDto = Mapper.Map<FileDto>(fileFromDb);
            fileDto.Path = GetRelativeFilePath(fileFromDb.Id, fileFromDb.Extension);
            fileDto.PathThumb = GetRelativeFilePath(fileFromDb.Id, "thumb.jpg");
            return fileDto;
        }

        public async Task<IEnumerable<FileDto>> GetFilesByLinkedObject(int linkedObjectId, LinkedObjectType linkedObjectType)
        {
            var filesFromDb = await _context.Files.Where(f => (f.LinkedObjectId == linkedObjectId) 
                                                                        && (f.LinkedObjectType == linkedObjectType) )
                                                                        .ToListAsync();
            if (filesFromDb == null) return null;

            var result = new List<FileDto>();
            foreach (var file in filesFromDb)
            {
                var fileDto = Mapper.Map<FileDto>(file);
                fileDto.Path = GetRelativeFilePath(fileDto.Id, fileDto.Extension);
                fileDto.PathThumb = GetRelativeFilePath(fileDto.Id, "thumb.jpg");
                result.Add(fileDto);
            }
            return result;
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
                Created = DateTimeOffset.Now,
                Name = file.Name,
                FormId = file.FormId,
                Code = Guid.NewGuid().ToString()
            });
            await _context.SaveChangesAsync();

            string fileName = AddLeftZeroesToId(fileInDb.Id) + "." + fileInDb.Extension;
            string thumbFileName = AddLeftZeroesToId(fileInDb.Id) + ".thumb.jpg";

            // куда складывать файлы
            string baseFilePath = AppDomain.CurrentDomain.BaseDirectory + "/userfiles";

            // Создаём папку первого уровня
            string firstLevelFolderName = fileName.Substring(0, 3);
            CheckAndCreateDirectory(baseFilePath + "/" + firstLevelFolderName);

            //Создаём папку второго уровня
            string secondLevelFolderName = fileName.Substring(3, 3);
            string secondLevelPath = CheckAndCreateDirectory(baseFilePath + "/" + firstLevelFolderName + "/" + secondLevelFolderName);
            string fullPath = secondLevelPath + "/" + fileName;
            string fullPathToThumb = secondLevelPath + "/" + thumbFileName;
            // Создать нужную папку в файловой системе
            // И сохнарить картинку туда
            if (FileHelpers.IsImage(file.Extension))
            {
                FileHelpers.Base64ToFile(base64Data, fullPathToThumb, "thumb."+file.Extension, ModelsSettings.IMAGE_WIDTH);
            }
            FileHelpers.Base64ToFile(base64Data, fullPath, file.Extension, 10000);
            
            return await GetFile(fileInDb.Id);
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

        public async Task<byte[]> GetFileData(string code, bool thumb)
        {
            var fileDto = await GetFile(code);
            var path = thumb ? fileDto.PathThumb : fileDto.Path;
            return File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/" + path);

             
        }

        public async Task<bool> CheckRights(int fileId, string email)
        {
            var fileDto = await GetFile(fileId);

            // Если файл никуда не привязан (что очень странно),
            // то пусть его удаляют все, кому не лень
            if (!fileDto.LinkedObjectId.HasValue) return true;

            if (fileDto.LinkedObjectType == LinkedObjectType.Comment)
            {
                return await _commentOperations.CheckRights(fileDto.LinkedObjectId.Value, email);
            }

            if (fileDto.LinkedObjectType == LinkedObjectType.User)
            {
                var user = await _userOperations.GetAsync(email);
                return user.Id == fileDto.LinkedObjectId;
            }

            return false;
        }
    }
}
