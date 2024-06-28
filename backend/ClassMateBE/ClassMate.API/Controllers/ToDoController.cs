using AutoMapper;
using ClassMate.API.Models;
using ClassMate.API.Models.Filters;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassMate.API.Controllers
{
    [Route("api/todos")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly IMapper _mapper;

        public ToDoController(IToDoService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateToDo([FromBody] CreateToDoModel model)
        {
            var dto = _mapper.Map<ToDoDto>(model);
            await _toDoService.CreateToDoAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateToDo([FromBody] UpdateToDoModel model)
        {
            var dto = _mapper.Map<ToDoDto>(model);
            await _toDoService.UpdateToDoAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDo(Guid id)
        {
            await _toDoService.DeleteToDoAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetToDoById(Guid id)
        {
            var dto = _mapper.Map<ResponseToDoDto>(await _toDoService.GetToDoAsync(id));
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult> GetToDos([FromQuery] ToDoFilter filter, [FromQuery] PaginationModel model)
        {
            var list = await _toDoService.GetAllToDosAsync(model.PageNumber, model.PageSize, _mapper.Map<ToDoFilterDto>(filter));
            return Ok(list);
        }

        [HttpPost("share")]
        public async Task<ActionResult> ShareToDo([FromBody] ShareModel model)
        {
            await _toDoService.ShareToDoAsync(model.ResourceID, model.Email, model.Role);
            return Ok();
        }

        [HttpPost("sync")]
        public async Task<ActionResult> SyncToDo([FromBody] SyncModel model)
        {
            await _toDoService.SyncToDoAsync(model.ResourceId);
            return Ok();
        }

        [HttpGet("next")]
        public async Task<ActionResult> GetNextToDo()
        {
            var dto = await _toDoService.GetNextToDo();
            if (dto != null)
            {
                var response = _mapper.Map<ResponseToDoDto>(dto);
                return Ok(response);
            }
            return Ok(null);
        }

    }
}
