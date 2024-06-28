using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public abstract class BaseService<TDto> : IBaseService<TDto>
       where TDto : BaseDto
    {
        protected readonly IBaseRepository<TDto> _baseRepository;
        public BaseService(IBaseRepository<TDto> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<IEnumerable<TDto>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<TDto, bool>>? filter = null)
        {
            return await _baseRepository.GetAllAsync((pageNumber - 1) * pageSize, pageSize, filter);
        }

        public async Task<TDto> GetByIdAsync(Guid id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            return await _baseRepository.CreateAsync(dto);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _baseRepository.DeleteAsync(id);
        }
        public async Task UpdateAsync(TDto dto)
        {
            await _baseRepository.UpdateAsync(dto);
        }
    }
}
