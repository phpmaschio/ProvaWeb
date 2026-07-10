import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadStatusProcessoDto } from '../models/read-status-processo-dto';
import { CreateStatusProcessoDto } from '../models/create-status-processo-dto';

@Injectable({
    providedIn: 'root'
})

export class StatusProcessoService {
    constructor(public http: HttpClient) { }

    public getStatusProcessos(): Observable<ReadStatusProcessoDto[]> {
        return this.http.get<ReadStatusProcessoDto[]>('/api/StatusProcesso').pipe(
            retry(1)
        );
    }

    public postStatusProcesso(createStatusProcessoDto:CreateStatusProcessoDto):Observable<ReadStatusProcessoDto>{
        return this.http.post<ReadStatusProcessoDto>('api/StatusProcesso',createStatusProcessoDto);
    }


}