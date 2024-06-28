using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IGoogleService
    {
        string GetAuthUrl();
        Task<string> Callback(string code);
        Task<CalendarEventDto?> SaveClass(ClassDto classDto, string? eventId);
        Task<CalendarEventDto?> SaveToDo(ToDoDto toDoDto, string? eventId);
    }
}
