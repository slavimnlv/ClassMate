using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ClassMate.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => src.Salt))
                .ReverseMap();

            CreateMap<ClassDto, Class>().ReverseMap();

            CreateMap<UsersClassesDto, UsersClasses>().ReverseMap();

            CreateMap<NoteDto, Note>().ReverseMap();

            CreateMap<UsersNotesDto, UsersNotes>().ReverseMap();

            CreateMap<UsersToDosDto, UsersToDos>().ReverseMap();

            CreateMap<UsersNotes, ResponseNoteDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.NoteID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Note!.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Note!.Content));

            CreateMap<TagDto, Tag>().ReverseMap();

            CreateMap<ToDoDto, ToDo>().ReverseMap();

            CreateMap<UsersToDos, ResponseToDoDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ToDoID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ToDo!.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ToDo!.Description))
                .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.ToDo!.Deadline))
                .ForMember(dest => dest.Done, opt => opt.MapFrom(src => src.ToDo!.Done));

            CreateMap<OAuthTokenDto, OAuthToken>().ReverseMap();

            CreateMap<CalendarEventDto, CalendarEvent>().ReverseMap();

        }
    }
}
