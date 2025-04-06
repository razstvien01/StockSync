using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.dtos.Comment;
using backend.interfaces;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext _context;
        public CommentRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Comment?> CreateCommentAsync(Comment commentDto)
        {
            await _context.AddAsync(commentDto);
            await _context.SaveChangesAsync();
            
            return commentDto;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            
            if(comment == null)
            {
                return null;
            }
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            
            return comment;
        }

        public async Task<Comment?> FindCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            
            if(comment == null)
            {
                return null;
            }
            
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDto request)
        {
            var comment = await _context.Comments.FindAsync(id);
            
            if(comment == null)
            {
                return null;
            }
            
            comment.Title = request.Title;
            comment.Content = request.Content;
            comment.CreatedAt = request.CreatedAt;
            comment.StockId = request.StockId;
            
            await _context.SaveChangesAsync();
            
            return comment;
        }
    }
}