﻿using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class CalendarEventRepository : BaseRepository<CalendarEventDto, CalendarEvent>, ICalendarEventRepository
    {
        public CalendarEventRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
