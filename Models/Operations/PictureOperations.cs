using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using Models.Entities;

namespace Models.Operations
{
    public class PictureOperations
    {
        private LrdrContext _context;

        public PictureOperations(LrdrContext context)
        {
            _context = context;
        }

        public async Task<Picture> GetAsync(int id)
        {
            try
            {
                return await _context.Pictures.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET PICTURE", ex);
                throw;
            }
        }

        public async Task<Picture> AddAsync(int tempFileId, bool isMain, 
                                            int linkedObjectId, LinkedObjectType linkedObjectType)
        {
            Contracts.Assert(tempFileId!=0);
            Contracts.Assert(linkedObjectId!=0);

            try
            {
                var tempFile = await _context.TempFiles.FirstAsync(tf => tf.Id == tempFileId);
                Picture picture = new Picture
                {
                    Data = tempFile.Data,
                    Filename = tempFile.Filename,
                    IsMain = isMain,
                    LinkedObjectId = linkedObjectId,
                    LinkedObjectType = linkedObjectType
                };
                _context.Pictures.Add(picture);
                _context.TempFiles.Remove(tempFile);
                await _context.SaveChangesAsync();
                return picture;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT ADD PICTURE", ex);
                throw;
            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var pictureForDelete = _context.Pictures.First(tf => tf.Id == id);
                _context.Pictures.Remove(pictureForDelete);
                var usersWithThisPicture = await _context.Users.Where(u => u.PictureId == id).ToListAsync();
                if (usersWithThisPicture != null)
                {
                    foreach (var user in usersWithThisPicture)
                    {
                        user.PictureId = null;
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT DELETE PICTURE", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Picture>> GetByLinkedObject(LinkedObjectType type, int linkedObjectId)
        {
            try
            {
                return await _context.Pictures.Where(p => (p.LinkedObjectId == linkedObjectId) 
                                                     && (p.LinkedObjectType == type))
                                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET PICTURE", ex);
                throw;
            }
        }

        public async Task<Picture> GetMainByLinkedObject(LinkedObjectType type, int linkedObjectId)
        {
            try
            {
                var pictures = (await GetByLinkedObject(type, linkedObjectId)).ToList();
                return pictures.FirstOrDefault(p => p.IsMain) ?? pictures.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET PICTURE", ex);
                throw;
            }
        }

        public async Task DeleteByLinkedObject(LinkedObjectType type, int linkedObjectId)
        {
            try
            {
                var pictures = await GetByLinkedObject(type, linkedObjectId);
                foreach (var picture in pictures)
                {
                    await DeleteAsync(picture.Id);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT DELETE PICTURES", ex);
                throw;
            }
        }

        public async Task SaveByFormIdAsync(string formId, int linkedObjectId, LinkedObjectType type)
        {
            var tempFileOperations = new TempFileOperations(_context);
            var tempfiles = await tempFileOperations.GetByFormIdAsync(formId);
            foreach (var tempfile in tempfiles)
            {
                await AddAsync(tempfile.Id, tempfile.IsMain, linkedObjectId, type);
            }
            await tempFileOperations.RemoveAllByFormIdAsync(formId);
        }
    }
}
