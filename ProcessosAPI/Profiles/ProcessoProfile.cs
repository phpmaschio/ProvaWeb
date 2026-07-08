using AutoMapper;
using ProcessosAPI.DTOs;
using ProcessosAPI.Models;

namespace ProcessosAPI.Profiles;

public class ProcessoProfile: Profile
{
   public ProcessoProfile()
   {
      CreateMap<CreateProcessoDto,Processo>();
      CreateMap<Processo,CreateProcessoDto>();
      CreateMap<UpdateProcessoDto,Processo>();
      CreateMap<Processo,UpdateProcessoDto>();
      CreateMap<CreateProcessoDto, ReadProcessoDto>();
      CreateMap<ReadProcessoDto, CreateProcessoDto>();
      
      CreateMap<Processo, ReadProcessoDto>()
         .ConstructUsing((src, context) => new ReadProcessoDto(
            src.Id,
            src.Descricao,
            context.Mapper.Map<ReadStatusProcessoDto>(src.Status),
            context.Mapper.Map<List<ReadParteDto>>(null),
            context.Mapper.Map<ReadAndamentoAtualDto>(null)
         ));
   } 
}