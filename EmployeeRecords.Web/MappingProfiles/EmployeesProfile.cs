using AutoMapper;
using EmployeeRecords.Data.DomainEntities;
using EmployeeRecords.Web.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeForCreationDto, Employee>()
                .ForMember(
                dest => dest.Files,
                opt => opt.Ignore());
            CreateMap<EmployeeForUpdateDto, Employee>()
                .ForMember(
                dest => dest.Files,
                opt => opt.Ignore());

            CreateMap<Employee, EmployeeDto>();
        }
    }
}
