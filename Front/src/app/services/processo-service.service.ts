import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadProcessoDto } from '../models/read-processo-dto';
import { CreateProcessoDto } from '../models/create-processo-dto';
@Injectable({
    providedIn: 'root'
})

export class ProcessoService {
    constructor(public http: HttpClient) { }

    public getProcessos(): Observable<ReadProcessoDto[]> {
        return this.http.get<ReadProcessoDto[]>('/api/Processo').pipe(
            retry(1)
        );
    }

    public postProcessos(createProcessoDto:CreateProcessoDto):Observable<ReadProcessoDto>{
        return this.http.post<ReadProcessoDto>('api/Processo',createProcessoDto);
    }


}