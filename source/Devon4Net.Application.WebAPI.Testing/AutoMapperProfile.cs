using AutoMapper;
using Devon4Net.Application.WebAPI.Implementation.Business.TodoManagement.Dto;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;

namespace Devon4Net.Application.XUnit
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Put automapper profile here
        /// </summary>
        public AutoMapperProfile()
        {
            CreateTodoManagementMappers();
        }

        private void CreateTodoManagementMappers()
        {
            CreateMap<TodoDto, Todos>()
                .ReverseMap();
        }

    }
}
