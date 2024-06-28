using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using ClassMate.Domain.Exceptions;
using System.Text;

namespace ClassMate.Services
{
    public class ClassService : BaseService<ClassDto>, IClassService
    {
        private readonly IClassRepository _classRepository; 
        private readonly IUserContextService _userContextService;
        private readonly IUsersClassesRepository _usersClassesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IGoogleService _googleService;
        private readonly ICalendarEventRepository _calendarEventRepository;

        public ClassService(IClassRepository classRepository, 
            IUsersClassesRepository usersClassesRepository,
            IUserContextService userContextService,
            IUserRepository userRepository,
            IEmailService emailService,
            IGoogleService googleService,
            ICalendarEventRepository calendarEventRepository) : base(classRepository)
        {
            _classRepository = classRepository;
            _usersClassesRepository = usersClassesRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _emailService = emailService;
            _googleService = googleService;
            _calendarEventRepository = calendarEventRepository;
        }

        public async Task CreateClassAsync(ClassDto dto)
        {
            var id = _userContextService.GetCurrentUserID();

            dto.ID = Guid.NewGuid();

            var usersClasses = new UsersClassesDto {
                Class = dto,
                UserID = id,
            };
            
            await _usersClassesRepository.CreateAsync(usersClasses);
        }

        public async Task DeleteClassAsync(Guid classId)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto  = await _usersClassesRepository.GetByUserAndClass(userId, classId);

            if(dto != null) 
            {
                if(dto.Role == Domain.Enums.AccessRoles.Owner)
                {
                    await _classRepository.DeleteAsync(dto.Class!.ID);
                }
                else
                {
                    await _usersClassesRepository.DeleteAsync(dto.ID); //the resource wont be shared with the user
                }
            }
            else
            {
                throw new NotFoundException("Resource not found.");
            }
        }

        public async Task<List<UsersClassesDto>> GetAllAsync()
        {
            var userId = _userContextService.GetCurrentUserID();

            return await _usersClassesRepository.GetAllByUser(userId);
        }

        public async Task<UsersClassesDto> GetClassAsync(Guid id)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto = await _usersClassesRepository.GetByUserAndClass(userId, id);

            if (dto != null)
            {
                return dto;
            }

            throw new NotFoundException("Could not find resource.");
        }

        public async Task UpdateClassAsync(ClassDto dto)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersClassesDto = await _usersClassesRepository.GetByUserAndClass(userId, dto.ID);

            if (usersClassesDto != null)
            {
                if (usersClassesDto.Role != Domain.Enums.AccessRoles.Viewer)
                {
                    await _classRepository.UpdateAsync(dto);
                }
                else 
                {
                    throw new UnauthorizedException("You do not have rights to modify the resource.");
                }

            }
            else
            {
                throw new NotFoundException("Could not find resource.");
            }
        }

        public async Task ShareClassAsync(Guid resourceID, string email, AccessRoles role)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersClassesDto = await _usersClassesRepository.GetByUserAndClass(userId, resourceID);

            if (usersClassesDto != null)
            {
                if (usersClassesDto.Role == AccessRoles.Owner)
                {
                    var shareUser = await _userRepository.GetByEmailAsync(email);

                    if (shareUser == null)
                    {
                        throw new NotFoundException($"User with email {email} does not exist.");
                    }

                    var checkIfShared = await _usersClassesRepository.GetByUserAndClass(shareUser.ID, resourceID);

                    if (checkIfShared != null)
                    {
                        throw new AppException($"Note already shared with {email}.");
                    }


                    if (role == AccessRoles.Owner)
                    {
                        throw new AppException("Can not make another user a owner of the resource.");
                    }

                    var sharedDto = new UsersClassesDto
                    {
                        ClassID = resourceID,
                        UserID = shareUser.ID,
                        Role = role
                    };

                    await _usersClassesRepository.CreateAsync(sharedDto);

                    _emailService.SendSharedNotification(email, usersClassesDto.User!.Name!, shareUser.Name!, "link"); //figure out links later

                }
                else
                {
                    throw new UnauthorizedException("You do not have rights to modify the resource.");
                }
            }
            else
            {
                throw new NotFoundException("Could not find resource.");

            }
        }

        public async Task SyncClassAsync(Guid resourceId)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersClassesDto = await _usersClassesRepository.GetByUserAndClass(userId, resourceId) ?? throw new NotFoundException("Could not found resource.");

            var calendarEventDto = await _googleService.SaveClass(usersClassesDto.Class!, null);

           if(calendarEventDto!= null)
           {
                calendarEventDto.UserId = userId;

                usersClassesDto.CalendarEvent = calendarEventDto;

                await _usersClassesRepository.UpdateAsync(usersClassesDto);
           }
            else
            {
                throw new AppException("An error occurred, please try again later.");
            }

        }

        public async Task<UsersClassesDto?> GetNextClass()
        {
            var userId = _userContextService.GetCurrentUserID();

            var classes = await _usersClassesRepository.GetAllByUser(userId);


            DateTime now = DateTime.Now;
            UsersClassesDto? nextClass = null;
            DateTime? nextClassStartDate = null;

            foreach (var userClass in classes)
            {
                if (userClass == null)
                    continue;

                DateTime classStartDate = userClass.Class!.StartDate;
                DateTime classEndDate = userClass.Class.EndDate;

                if (userClass.Class!.WeekRepetition.HasValue)
                {
                    int weeks = userClass.Class!.WeekRepetition.Value;
                    DateTime repeatUntil = userClass.Class!.RepeatUntil ?? DateTime.MaxValue;

                    while (classStartDate <= repeatUntil && classStartDate <= now)
                    {
                        classStartDate = classStartDate.AddDays(7 * weeks);
                        classEndDate = classEndDate.AddDays(7 * weeks);
                    }

                    if (classStartDate > now && classStartDate <= repeatUntil)
                    {
                        if (!nextClassStartDate.HasValue || classStartDate < nextClassStartDate)
                        {
                            nextClass = userClass;
                            nextClassStartDate = classStartDate;
                        }
                    }
                }
                else
                {
                    if (classStartDate > now)
                    {
                        if (!nextClassStartDate.HasValue || classStartDate < nextClassStartDate)
                        {
                            nextClass = userClass;
                            nextClassStartDate = classStartDate;
                        }
                    }
                }
            }

            if(nextClass != null)
            {
                if (nextClass.Class != null && nextClassStartDate != null)
                {
                    nextClass.Class.StartDate = (DateTime)nextClassStartDate;
                    return nextClass;
                }
                return null;
            }

            return null;
        }
    }
}
