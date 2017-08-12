using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using Models.Entities;

namespace Models.Operations
{
    public class CommentOperations
    {
        private LrdrContext _context;
        private OrderOperations _orderOperations;

        public CommentOperations(LrdrContext context)
        {
            _context = context;
            _orderOperations = new OrderOperations(_context);
        }

        public async Task<Comment> GetAsync(int id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET COMMENT", ex);
                throw;
            }
        }

        public async Task<PageViewDTO<Comment>> GetAllByOrder(int orderId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET COMMENTS", ex);
                throw;
            }
        }

        public async Task<PageViewDTO<Comment>> GetAllByUser(int orderId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET COMMENTS", ex);
                throw;
            }
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            try
            {
                throw new NotImplementedException();
                await _orderOperations.UpdateUpdatedDate(comment.OrderId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT ADD COMMENT", ex);
                throw;
            }
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            try
            {
                throw new NotImplementedException();
                await _orderOperations.UpdateUpdatedDate(comment.OrderId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT UPDATE COMMENT", ex);
                throw;
            }
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            try
            {
                throw new NotImplementedException();
                var comment = await GetAsync(id);
                await _orderOperations.UpdateUpdatedDate(comment.OrderId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT DELETE COMMENT", ex);
                throw;
            }
        }

        public async Task<bool> CheckRights(int commentId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
