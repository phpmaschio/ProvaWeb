import { CreateAndamentoDto } from "./create-andamento-dto";
import { ReadStatusProcessoDto } from "./read-status-processo-dto";

export interface CreateProcessoDto{
 id:number;
 descricao:string;
 statusProcesso: ReadStatusProcessoDto;
 andamento:CreateAndamentoDto;
}