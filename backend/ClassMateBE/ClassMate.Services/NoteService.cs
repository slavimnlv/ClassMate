using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using ClassMate.Domain.Exceptions;
using System.Linq;
using System.Web;

namespace ClassMate.Services
{
    public class NoteService : BaseService<NoteDto>, INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUsersNotesRepository _usersNotesRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public NoteService(INoteRepository noteRepository,
            IUsersNotesRepository usersNotesRepository,
            IUserContextService userContextService,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IEmailService emailService) : base(noteRepository)
        {
            _noteRepository = noteRepository;
            _usersNotesRepository = usersNotesRepository;
            _userContextService = userContextService;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task CreateNoteAsync(NoteDto dto)
        {
            var userId = _userContextService.GetCurrentUserID();

            dto.CreateDate = DateTime.Now;

            var tags = ExtractTagsFromContent(dto.Content!);

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

            var created = await _noteRepository.CreateNoteAsync(dto);

            var usersNotes = new UsersNotesDto
            {
                NoteID = created.ID,
                UserID = userId,
            };

            await _usersNotesRepository.CreateAsync(usersNotes);
            
        }

        public async Task DeleteNoteAsync(Guid noteId)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto = await _usersNotesRepository.GetByUserAndNote(userId, noteId);

            if (dto != null)
            {
                if (dto.Role == AccessRoles.Owner)
                {
                    await _noteRepository.DeleteAsync(dto.Note!.ID);
                }
                else
                {
                    await _usersNotesRepository.DeleteAsync(dto.ID); //the resource wont be shared with the user
                }


                foreach (TagDto tag in dto.Note!.Tags.Where(t => t.UserID == userId))
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

        public async Task<ResponsePaginatedDto<ResponseNoteDto>> GetAllNotesAsync(int pageNumber, int pageSize, NoteFilterDto filter)
        {
            var userId = _userContextService.GetCurrentUserID();

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            return await _usersNotesRepository.GetAllByUser(userId, (pageNumber - 1) * pageSize, pageSize, filter);
        }

        public async Task<UsersNotesDto> GetNoteAsync(Guid id)
        {
            var userId = _userContextService.GetCurrentUserID();

            var dto = await _usersNotesRepository.GetByUserAndNote(userId, id);

            if (dto != null)
            {
                if (dto.Role != AccessRoles.Owner) 
                {
                    var userNote = await _usersNotesRepository.GetNoteWithOwner(dto.NoteID);
                    dto.User = userNote!.User;
                }
                return dto;
            }

            throw new NotFoundException("Could not find resource.");
        }

        public async Task ShareNoteAsync(Guid resourceID, string email, AccessRoles role)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersNotesDto = await _usersNotesRepository.GetByUserAndNote(userId, resourceID);

            if (usersNotesDto != null)
            {
                if (usersNotesDto.Role == AccessRoles.Owner)
                {
                    var shareUser =  await _userRepository.GetByEmailAsync(email);

                    if (shareUser == null) 
                    {
                        throw new NotFoundException($"User with email {email} does not exist.");
                    }

                    var checkIfShared = await _usersNotesRepository.GetByUserAndNote(shareUser.ID, resourceID);

                    if (checkIfShared != null) 
                    {
                        throw new AppException($"Note already shared with {email}.");
                    }

                    if(role == AccessRoles.Owner) 
                    {
                        throw new AppException("Can not make another user a owner of the resource.");
                    }

                    var sharedDto = new UsersNotesDto
                    {
                        NoteID = resourceID,
                        UserID = shareUser.ID,
                        Role = role
                    };

                    await _usersNotesRepository.CreateAsync(sharedDto);

                    var queryParams = HttpUtility.ParseQueryString(string.Empty);
                    queryParams["title"] = usersNotesDto.Note!.Title;
                    queryParams["tagId"] = null;
                    queryParams["newest"] = "true";
                    queryParams["ownership"] = "shared";

                    var uriBuilder = new UriBuilder("http://localhost:5173/dashboard/notes")
                    {
                        Query = queryParams.ToString()
                    };

                    var sharedLink = uriBuilder.ToString();

                    _emailService.SendSharedNotification(email, usersNotesDto.User!.Name!, shareUser.Name!, sharedLink);

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

        public async Task UpdateNoteAsync(NoteDto dto)
        {
            var userId = _userContextService.GetCurrentUserID();

            var usersNotesDto = await _usersNotesRepository.GetByUserAndNote(userId, dto.ID);

            if (usersNotesDto != null)
            {
                if (usersNotesDto.Role != AccessRoles.Viewer)
                {

                   var tags = ExtractTagsFromContent(dto.Content!);

                   var userTags = await _tagRepository.GetTagsByUser(userId);

                    dto.CreateDate = DateTime.Now;

                    await _noteRepository.UpdateNoteAsync(userTags, tags, userId,dto);


                    foreach (TagDto tag in usersNotesDto!.Note!.Tags.Where(t => t.UserID == userId))
                    {
                        if (await _tagRepository.AnyNotesAndToDosAsync(tag.ID) == false)
                        {
                            await _tagRepository.DeleteAsync(tag.ID);
                        }
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
