using AutoMapper;
using ProcessosAPI.DTOs;

namespace ProcessosAPI.Profiles;

public class StatusProcessoProfile: Profile
{
   public StatusProcessoProfile()
   {
      CreateMap<CreateStatusProcessoDto,StatusProcesso>();
      CreateMap<StatusProcesso,CreateStatusProcessoDto>();
      CreateMap<UpdateStatusProcessoDto,StatusProcesso>();
      CreateMap<StatusProcesso,UpdateStatusProcessoDto>();
      CreateMap<ReadStatusProcessoDto,StatusProcesso>();
      CreateMap<StatusProcesso,ReadStatusProcessoDto>();
      
   } 
}