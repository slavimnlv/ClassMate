using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class TagService : BaseService<TagDto>, ITagService
    {
        private readonly IUserContextService _userContextService;
        private readonly ITagRepository _tagRepository;

        public TagService(IUserContextService userContextService,
            ITagRepository tagRepository) : base(tagRepository)
        {
            _userContextService = userContextService;
            _tagRepository = tagRepository;
        }

        public async Task<List<TagDto>> GetAllTagsAsync()
        {
            var userId = _userContextService.GetCurrentUserID();

            return await _tagRepository.GetTagsByUser(userId);
        }
    }
}
