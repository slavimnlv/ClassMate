    using AutoMapper;
using ClassMate.API.Models;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassMate.API.Controllers
{
    [Route("api/classes")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public ClassController(IClassService classService, IMapper mapper)
        {
            _classService = classService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateClass([FromBody] CreateClassModel model)
        {
            var dto =_mapper.Map<ClassDto>(model);
            await _classService.CreateClassAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateClass([FromBody] UpdateClassModel model)
        {
            var dto = _mapper.Map<ClassDto>(model);
            await _classService.UpdateClassAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClass(Guid id)
        {
            await _classService.DeleteClassAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetClassById(Guid id)
        {
            var dto = _mapper.Map<ResponseClassDto>(await _classService.GetClassAsync(id));
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult> GetClasses()
        {
            var list = _mapper.Map<List<ResponseClassDto>>(await _classService.GetAllAsync());
            return Ok(list);
        }

        [HttpPost("share")]
        public async Task<ActionResult> ShareClass([FromBody] ShareModel model)
        {
            await _classService.ShareClassAsync(model.ResourceID, model.Email, model.Role);
            return Ok();
        }

        [HttpPost("sync")]
        public async Task<ActionResult> SyncClass([FromBody] SyncModel model)
        {
            await _classService.SyncClassAsync(model.ResourceId);
            return Ok();
        }

        [HttpGet("next")]
        public async Task<ActionResult> GetNextClass()
        {
            var dto = await _classService.GetNextClass();
            if (dto != null)
            {
                var response = _mapper.Map<ResponseClassDto>(dto);
                return Ok(response);
            }
            return Ok(null);
        }

    }
}
