using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Comment;
using backend.interfaces;
using backend.mappers;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStocksById([FromRoute] int id)
        {
            var comment = await _commentRepository.FindCommentAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = comment.ToCommentDto();

            return Ok(commentDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestDto request)
        {
            var commentDto = request.ToCommentFromCreateDto();

            await _commentRepository.CreateCommentAsync(commentDto);

            return CreatedAtAction(nameof(GetStocksById), new { id = commentDto.Id }, commentDto.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto request)
        {
            var comment = await _commentRepository.FindCommentAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteCommentAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Accepted(new { message = "Comment deleted successfully", id = id });
        }
    }
}