using AutoMapper;
using HospitalManagement.DTO;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace HospitalManagement.Mapping
{
    public class MappingProfile : Profile
    { //this is Automapper 
        //var config = new ITypeMappingConfiguration(cfg =>
        //{

        //    cfg.CreateMap<Patinet, GetPatientDto>();
        //});

        public MappingProfile()
        {
            CreateMap<Patient, GetPatientDto>();

            CreateMap<Appointment, GetAppointmentsDto>()
                  .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : "Unknown Doctor"))
                   .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Department.Name : "N/A"));

        }
       

    }
}
