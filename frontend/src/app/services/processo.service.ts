import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadProcessoDto } from '../models/read-processo-dto';
import { CreateProcessoDto } from '../models/create-processo-dto';

// A API limita "take" a 100 por requisição (proteção contra consultas sem limite).
const TAKE_MAXIMO = 100;

@Injectable({
    providedIn: 'root'
})

export class ProcessoService {
    constructor(public http: HttpClient) { }

    public getProcessos(skip: number = 0, take: number = TAKE_MAXIMO): Observable<ReadProcessoDto[]> {
        const params = new HttpParams().set('skip', skip).set('take', take);
        return this.http.get<ReadProcessoDto[]>('/api/Processo', { params }).pipe(
            retry(1)
        );
    }

    public postProcesso(createProcessoDto: CreateProcessoDto): Observable<ReadProcessoDto> {
        return this.http.post<ReadProcessoDto>('api/Processo', createProcessoDto);
    }

    public updateProcesso(processoId:number,createProcessoDto: CreateProcessoDto): Observable<void> {
        return this.http.put<void>(`/api/Processo/${processoId}`, createProcessoDto);
    }


    public deleteProcesso(processoId: number): Observable<void> {
        return this.http.delete<void>(`/api/Processo/${processoId}`);
    }


}
