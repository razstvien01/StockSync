using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Comment;
using backend.models;

namespace backend.mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment dto)
        {
            return new CommentDto
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt,
                StockId = dto.StockId
            };
        }
        
        public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto dto, int stockId){
            return new Comment{
                Title = dto.Title,
                Content = dto.Content,
                StockId = stockId
            };
        }
    }
}