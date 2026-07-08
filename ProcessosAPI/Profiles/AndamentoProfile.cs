using AutoMapper;
using ProcessosAPI.DTOs;
using ProcessosAPI.Models;

namespace ProcessosAPI.Profiles;

public class AndamentoProfile: Profile
{
   public AndamentoProfile()
   {
      CreateMap<ReadAndamentoAtualDto,Andamento>();
      CreateMap<Andamento,ReadAndamentoAtualDto>();
    
   } 
}