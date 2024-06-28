using AutoMapper;
using ClassMate.API.Models;
using ClassMate.API.Models.Filters;
using ClassMate.Data.Entities;
using ClassMate.Domain.Dtos;
using static System.Net.Mime.MediaTypeNames;

namespace ClassMate.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, UserDto>();

            CreateMap<CreateClassModel, ClassDto>();

            CreateMap<UpdateClassModel, ClassDto>();

            CreateMap<UsersClassesDto, ResponseClassDto>()
           .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ClassID))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Class!.Title))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Class!.Description))
           .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Class!.StartDate))
           .ForMember(dest => dest.WeekRepetition, opt => opt.MapFrom(src => src.Class!.WeekRepetition))
           .ForMember(dest => dest.RepeatUntil, opt => opt.MapFrom(src => src.Class!.RepeatUntil))
           .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Class!.EndDate))
           .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<CreateNoteModel, NoteDto>();

            CreateMap<UpdateNoteModel, NoteDto>();

            CreateMap<UsersNotesDto, ResponseNoteDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.NoteID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Note!.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Note!.Content))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<CreateToDoModel, ToDoDto>();

            CreateMap<UpdateToDoModel, ToDoDto>();

            CreateMap<UsersToDosDto, ResponseToDoDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ToDoID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ToDo!.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ToDo!.Description))
                .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.ToDo!.Deadline))
                .ForMember(dest => dest.Done, opt => opt.MapFrom(src => src.ToDo!.Done))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<TagDto, ResponseTagDto>();

            CreateMap<ToDoFilter, ToDoFilterDto>();

            CreateMap<NoteFilter, NoteFilterDto>();


        }
    }
}
