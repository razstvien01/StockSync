using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Comment;
using backend.models;

namespace backend.interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment?> FindCommentAsync(int id);
        Task<Comment?> CreateCommentAsync(Comment commentDto);
        Task<Comment?> UpdateCommentAsync(int id, Comment request);
        Task<Comment?> DeleteCommentAsync(int id);
    }
}