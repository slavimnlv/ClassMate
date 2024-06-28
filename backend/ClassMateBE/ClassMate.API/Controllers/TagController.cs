using AutoMapper;
using ClassMate.API.Models.Filters;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassMate.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetTags()
        {
            var list =  _mapper.Map<List<ResponseTagDto>>(await _tagService.GetAllTagsAsync());
            return Ok(list);
        }
    }
}
