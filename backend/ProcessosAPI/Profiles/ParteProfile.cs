using AutoMapper;
using ProcessosAPI.DTOs;
using ProcessosAPI.Models;

namespace ProcessosAPI.Profiles;

public class ParteProfile: Profile
{
   public ParteProfile()
   {
      CreateMap<CreateParteDto,Parte>();
      CreateMap<Parte,CreateParteDto>();
      CreateMap<UpdateParteDto,Parte>();
      CreateMap<Parte,UpdateParteDto>();
      CreateMap<ReadParteDto,Parte>();
      CreateMap<Parte,ReadParteDto>();
   } 
}