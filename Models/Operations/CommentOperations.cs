using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Camps.BLL.Utilities;
using Camps.Tools;
using Models.Entities;
using Models.Tools;

namespace Models.Operations
{
    public class CommentOperations
    {
        public AsyncEventHandler<Comment> OnModifyEventHandler { get; set; }
        public AsyncEventHandler<Comment> OnDeleteEventHandler { get; set; }
        private LrdrContext _context;
        

        public CommentOperations(LrdrContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetAsync(int id)
        {
            try
            {
                var result = await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) throw new NotFoundException();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET COMMENT", ex);
                throw;
            }
        }

        public async Task<PageViewDTO<Comment>> GetAllByOrder(int orderId, int page=1)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order==null) throw new NotFoundException();

                var comments = _context.Comments.Where(c => (c.OrderId == orderId))
                    .Include(c => c.User)
                    .OrderByDescending(c => c.Time);
                var total = comments.Count();

                return  new PageViewDTO<Comment>
                {
                    Content = await comments.ToListAsync(),
                    PageNumber = page,
                    SortBy = "Time",
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / (double)ModelsSettings.PAGE_SIZE)
                };
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT GET COMMENTS", ex);
                throw;
            }
        }

        public async Task<PageViewDTO<Comment>> GetAllByUser(int userId, int page=1)
        {
            try
            {
                var order = await _context.Users.FirstOrDefaultAsync(o => o.Id == userId);
                if (order == null) throw new NotFoundException();

                var comments = _context.Comments.Where(c => (c.UserId == userId))
                    .Include(c => c.User)
                    .OrderByDescending(c => c.Time);
                var total = comments.Count();

                return new PageViewDTO<Comment>
                {
                    Content = await comments.ToListAsync(),
                    PageNumber = page,
                    SortBy = "Time",
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / (double)ModelsSettings.PAGE_SIZE)
                };
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
                var result = _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                if (this.OnModifyEventHandler != null)
                {
                    await this.OnModifyEventHandler.Invoke(this, result);
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT ADD COMMENT", ex);
                throw;
            }
        }

        public async Task<Comment> UpdateAsync(int commentId, string commentText)
        {
            try
            {
                var oldComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
                if (oldComment == null) throw new NotFoundException();

                oldComment.Text = commentText;
                await _context.SaveChangesAsync();
                if (this.OnModifyEventHandler != null)
                {
                    await this.OnModifyEventHandler.Invoke(this, oldComment);
                }
                return oldComment;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT UPDATE COMMENT", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var comment = await GetAsync(id);
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                if (this.OnModifyEventHandler != null)
                {
                    await this.OnModifyEventHandler.Invoke(this, comment);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("CANNOT DELETE COMMENT", ex);
                throw;
            }
        }

        public async Task<bool> CheckRights(int commentId, string userEmail)
        {
            var comment = await GetAsync(commentId);
            var user = _context.Users.First(u => u.Email == userEmail);

            if (comment.UserId == user.Id) return true;

            if (user.Role == Role.PortalAdmin) return true;

            return await OrderOperations.CheckRights(comment.OrderId, userEmail);
        }
    }
}
