using AutoMapper;
using EmployeeRecords.Data.DomainEntities;
using EmployeeRecords.Web.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web.MappingProfiles
{
    public class EmployeeFileProfile:Profile
    {
        public EmployeeFileProfile()
        {
            CreateMap<IFormFile, EmployeeFile>()
                .ForMember(
                dest => dest.FileName,
                opt => opt.MapFrom(source => source.FileName))
                .ForMember(
                dest => dest.ContentType,
                opt => opt.MapFrom(source => source.ContentType))
                .ForMember(
                dest => dest.FileSize,
                opt => opt.MapFrom(source => source.Length));
            CreateMap<EmployeeFile, EmployeeFileDto>() ;
        }
    }
}
