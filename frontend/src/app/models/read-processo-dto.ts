import { ReadAndamentoAtualDto } from "./read-andamento-dto";
import { ReadParteDto } from "./read-parte-dto";
import { ReadStatusProcessoDto } from "./read-status-processo-dto";

export interface ReadProcessoDto{
 id:number;
 descricao:string;
 statusProcesso: ReadStatusProcessoDto;
 andamento:ReadAndamentoAtualDto;
 partes: ReadParteDto[];
 criadoEm: string;
}