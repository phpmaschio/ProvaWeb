import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadProcessoDto } from '../models/read-processo-dto';

@Injectable({
    providedIn: 'root'
})

export class AndamentoService {
    constructor(public http: HttpClient) { }

    public getAndamento(): Observable<ReadProcessoDto[]> {
        return this.http.get<ReadProcessoDto[]>('/api/Andamento').pipe(
            retry(1)
        );
    }


}