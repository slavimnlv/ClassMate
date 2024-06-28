using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IOAuthTokenRepository : IBaseRepository<OAuthTokenDto>
    {
        Task<OAuthTokenDto?> GetByUserIdAndPlatformAsync(Guid userId, Platforms platform);
    }
}
