import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadAndamentoAtualDto } from '../models/read-andamento-dto';

@Injectable({
    providedIn: 'root'
})

export class AndamentoService {
    constructor(public http: HttpClient) { }

    public getAndamento(): Observable<ReadAndamentoAtualDto[]> {
        return this.http.get<ReadAndamentoAtualDto[]>('/api/Andamento').pipe(
            retry(1)
        );
    }


}