using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using Models.Entities;
using Models.Tools;

namespace Models.Operations
{
    public class TempFileOperations
    {
        private LrdrContext _context;

        public TempFileOperations(LrdrContext context)
        {
            _context = context;
        }


        public async Task<TempFile> GetAsync(int id)
        {
            try
            {
                return await _context.TempFiles.FirstOrDefaultAsync(tf => tf.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET TEMP_FILE", ex);
                throw;
            }
        }

        public async Task<TempFile> UpdateAsync(TempFile tempFile)
        {
            try
            {
                var tempFileDb = _context.TempFiles.First(tf => tf.Id == tempFile.Id);
                tempFileDb.Filename = tempFile.Filename;
                tempFileDb.FormId = tempFile.FormId;
                tempFileDb.Data = tempFile.Data;

                await _context.SaveChangesAsync();
                return tempFileDb;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT UPDATE TEMP_FILE", ex);
                throw;
            }
        }

        public async Task<TempFile> AddAsync(TempFile tempFile)
        {
            tempFile.Data = Images.Resize(1000, tempFile.Data);
            try
            {
                tempFile.Filename = Path.GetFileNameWithoutExtension(tempFile.Filename);
                _context.TempFiles.Add(tempFile);
                await _context.SaveChangesAsync();
                return tempFile;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT ADD TEMP_FILE", ex);
                throw;
            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var fileForDelete = _context.TempFiles.First(tf => tf.Id == id);
                _context.TempFiles.Remove(fileForDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT DELETE TEMP_FILE", ex);
                throw;
            }
        }

        public async Task<IEnumerable<TempFile>> GetByFormIdAsync(string formId)
        {
            try
            {
                return await _context.TempFiles.Where(tf => tf.FormId == formId).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET TEMP_FILES", ex);
                throw;
            }
        }

        public async Task RemoveAllByFormIdAsync(string formId)
        {
            try
            {
                var files = await this.GetByFormIdAsync(formId);
                foreach (var tempFile in files)
                {
                    _context.TempFiles.Remove(tempFile);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT REMOVE TEMP_FILES", ex);
                throw;
            }
        }
    }
}
