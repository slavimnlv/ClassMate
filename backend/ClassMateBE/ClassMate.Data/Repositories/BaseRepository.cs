using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public abstract class BaseRepository<TDto, TEntity> : IBaseRepository<TDto> where TEntity : BaseEntity where TDto : BaseDto
    {
        protected readonly ClassMateDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly DbSet<TEntity> entities;

        protected BaseRepository(ClassMateDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            entities = dbContext.Set<TEntity>();
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            dto.ID = Guid.NewGuid();
            TEntity entity = _mapper.Map<TEntity>(dto);

            await entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> GetByIdAsync(Guid id)
        {
            TEntity? entity = await entities
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);
            var dto = _mapper.Map<TDto>(entity);

            return dto;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync(int offset, int limit, Expression<Func<TDto, bool>>? filter = null)
        {
            var query = entities.AsQueryable();

            if (filter != null)
            {
                var filterDb = _mapper.Map<Expression<Func<TEntity, bool>>>(filter);
                query = query.Where(filterDb);
            }

            query = query.OrderBy(entity => entity.ID)
                         .Skip(offset)
                         .Take(limit);

            return await _mapper.ProjectTo<TDto>(query).ToListAsync();
        }

        public async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity? entity = await entities.FirstOrDefaultAsync(e => e.ID == id);

            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TDto, bool>> predicate)
        {
            return await entities.AnyAsync(_mapper.Map<Expression<Func<TEntity, bool>>>(predicate));
        }

    }
}
