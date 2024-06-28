using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IBaseRepository<TDto>
        where TDto : BaseDto
    {
        public Task<TDto> GetByIdAsync(Guid id);

        public Task<IEnumerable<TDto>> GetAllAsync(int offset, int limit, Expression<Func<TDto, bool>>? filter = null);

        public Task<TDto> CreateAsync(TDto dto);

        public Task DeleteAsync(Guid id);

        public Task UpdateAsync(TDto dto);

        Task<bool> AnyAsync(Expression<Func<TDto, bool>> predicate);

    }
}
