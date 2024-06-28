using AutoMapper;
using ClassMate.API.Models;
using ClassMate.API.Models.Filters;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassMate.API.Controllers
{
    [Route("api/notes")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IMapper _mapper;

        public NoteController(INoteService noteService, IMapper mapper)
        {
            _noteService = noteService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNote([FromBody] CreateNoteModel model)
        {
            var dto =_mapper.Map<NoteDto>(model);
            await _noteService.CreateNoteAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateNote([FromBody] UpdateNoteModel model)
        {
            var dto = _mapper.Map<NoteDto>(model);
            await _noteService.UpdateNoteAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNoteById(Guid id)
        {
            var dto = _mapper.Map<ResponseNoteDto>(await _noteService.GetNoteAsync(id));
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult> GetNotes([FromQuery] NoteFilter filter, [FromQuery] PaginationModel model)
        {
            var list = await _noteService.GetAllNotesAsync(model.PageNumber, model.PageSize, _mapper.Map<NoteFilterDto>(filter));
            return Ok(list);
        }

        [HttpPost("share")]
        public async Task<ActionResult> ShareNote([FromBody] ShareModel model)
        {
            await _noteService.ShareNoteAsync(model.ResourceID, model.Email, model.Role);
            return Ok();
        }

    }
}
