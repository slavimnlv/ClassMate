using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using ClassMate.Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace ClassMate.Services
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly IToDoRepository _todoRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUsersToDosRepository _usersToDosRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IGoogleService _googleService;

        public ToDoService(IToDoRepository todoRepository,
            IUsersToDosRepository usersToDosRepository,
            IUserContextService userContextService,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IGoogleService googleService) : base(todoRepository)
        {
            _todoRepository = todoRepository;
            _usersToDosRepository = usersToDosRepository;
            _userContextService = userContextService;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _googleService = googleService;
        }

        public async Task CreateToDoAsync(ToDoDto dto)
        {
            var userId = _userContextService.GetCurrentUserID();

            var tags = ExtractTagsFromContent(dto.Description!);

            var userTags = await _tagRepository.GetTagsByUser(userId);

            foreach (var tagName in tags)
            {
                var tag = userTags.FirstOrDefault(tag => tag.Title == tagName);

                if (tag == null)
                {
                    tag = new TagDto
                    {
                        UserID = userId,
                        Title = tagName
                    };
                }

                dto.Tags.Add(tag);

            } 

            var created  = await _todoRepository.CreateToDoAsync(dto);

            var usersTodos = new UsersToDosDto
            {
                ToDoID = created.ID,
                UserID = userId
            };

            await _usersToDosRepository.CreateAsync(usersTodos);
        }

        public async Task DeleteToDoAsync(Guid todoId)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto = await _usersToDosRepository.GetByUserAndToDo(userId, todoId);

            if (dto != null)
            {
                if (dto.Role == AccessRoles.Owner)
                {
                    await _todoRepository.DeleteAsync(dto.ToDo!.ID);
                }
                else
                {
                    await _usersToDosRepository.DeleteAsync(dto.ID); //the resource wont be shared with the user
                }

                foreach (TagDto tag in dto.ToDo!.Tags.Where(t => t.UserID == userId))
                {
                    if (await _tagRepository.AnyNotesAndToDosAsync(tag.ID) == false)
                    {
                        await _tagRepository.DeleteAsync(tag.ID);
                    }
                }
            }
            else
            {
                throw new NotFoundException("Resource not found.");
            }

        }

        public async Task<ResponsePaginatedDto<ResponseToDoDto>> GetAllToDosAsync(int pageNumber, int pageSize, ToDoFilterDto filter)
        {
            var userId = _userContextService.GetCurrentUserID();

            if(pageNumber < 1)
            {
                pageNumber = 1;
            }

            return await _usersToDosRepository.GetAllByUser(userId, (pageNumber - 1) * pageSize, pageSize, filter);
        }

        public async Task<UsersToDosDto> GetToDoAsync(Guid id)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto = await _usersToDosRepository.GetByUserAndToDo(userId, id);

            if (dto != null)
            {
                if (dto.Role != AccessRoles.Owner)
                {
                    var userNote = await _usersToDosRepository.GetToDoWithOwner(dto.ToDoID);
                    dto.User = userNote!.User;
                }
                return dto;
            }

            throw new NotFoundException("Could not find resource.");
        }

        public async Task UpdateToDoAsync(ToDoDto dto)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersToDosDto = await _usersToDosRepository.GetByUserAndToDo(userId, dto.ID);

            if (usersToDosDto != null)
            {
                if (usersToDosDto.Role != Domain.Enums.AccessRoles.Viewer)
                {

                    if (!dto.Description.IsNullOrEmpty() || !usersToDosDto.ToDo!.Description.IsNullOrEmpty())
                    {
                        var tags = new List<string>();
                        if(dto.Description != null)
                        {
                            tags = ExtractTagsFromContent(dto.Description);
                        }

                        var userTags = await _tagRepository.GetTagsByUser(userId);

                        await _todoRepository.UpdateToDoAsync(userTags, tags, userId, dto);

                        foreach (TagDto tag in usersToDosDto.ToDo!.Tags.Where(t => t.UserID == userId))
                        {
                            if (await _tagRepository.AnyNotesAndToDosAsync(tag.ID) == false)
                            {
                                await _tagRepository.DeleteAsync(tag.ID);
                            }
                        }

                    }
                    else
                    {
                        await UpdateAsync(dto);
                    }

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

        public async Task ShareToDoAsync(Guid resourceID, string email, AccessRoles role)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersToDosDto = await _usersToDosRepository.GetByUserAndToDo(userId, resourceID);

            if (usersToDosDto != null)
            {
                if (usersToDosDto.Role == AccessRoles.Owner)
                {
                    var shareUser = await _userRepository.GetByEmailAsync(email);

                    if (shareUser == null)
                    {
                        throw new NotFoundException($"User with email {email} does not exist.");
                    }

                    var checkIfShared = await _usersToDosRepository.GetByUserAndToDo(shareUser.ID, resourceID);

                    if (checkIfShared != null)
                    {
                        throw new AppException($"Note already shared with {email}.");
                    }


                    if (role == AccessRoles.Owner)
                    {
                        throw new AppException("Can not make another user a owner of the resource.");
                    }

                    var sharedDto = new UsersToDosDto
                    {
                        ToDoID = resourceID,
                        UserID = shareUser.ID,
                        Role = role
                    };

                    await _usersToDosRepository.CreateAsync(sharedDto);
                    var queryParams = HttpUtility.ParseQueryString(string.Empty);
                    queryParams["title"] = usersToDosDto.ToDo!.Title;
                    queryParams["tagId"] = null;
                    queryParams["hideDone"] = "false";
                    queryParams["ownership"] = "shared";

                    var uriBuilder = new UriBuilder("http://localhost:5173/dashboard/todos")
                    {
                        Query = queryParams.ToString()
                    };

                    var sharedLink = uriBuilder.ToString();

                    _emailService.SendSharedNotification(email, usersToDosDto.User!.Name!, shareUser.Name!, sharedLink);

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

        public async Task SyncToDoAsync(Guid resourceId)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersToDosDto = await _usersToDosRepository.GetByUserAndToDo(userId, resourceId) ?? throw new NotFoundException("Could not found resource.");

            var calendarEventDto = await _googleService.SaveToDo(usersToDosDto.ToDo!, null);

            if (calendarEventDto != null)
            {
                calendarEventDto.UserId = userId;

                usersToDosDto.CalendarEvent = calendarEventDto;

                await _usersToDosRepository.UpdateAsync(usersToDosDto);
            }
            else
            {
                throw new AppException("An error occurred, please try again later.");
            }

        }

        public async Task<UsersToDosDto?> GetNextToDo()
        {
            var userId = _userContextService.GetCurrentUserID();

            var nextToDo = await _usersToDosRepository.GetNextToDo(userId);

            return nextToDo;
        }

        private List<string> ExtractTagsFromContent(string content)
        {
            var tags = new HashSet<string>();

            var words = content.Split(' ');

            foreach (var word in words)
            {
                if (word.StartsWith("#"))
                {
                    var cleanedTag = word.TrimStart('#').Replace("\n", "");
                    tags.Add(cleanedTag);
                }
            }

            return tags.ToList();
        }
    }
}
