import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { ReadParteDto } from '../models/read-parte-dto';
import { CreateParteDto } from '../models/create-parte-dto';

@Injectable({
    providedIn: 'root'
})

export class ParteService {
    constructor(public http: HttpClient) { }

    public getPartes(): Observable<ReadParteDto[]> {
        return this.http.get<ReadParteDto[]>('/api/Parte').pipe(
            retry(1)
        );
    }

    public postParte(createParteDto:CreateParteDto):Observable<CreateParteDto>{
        return this.http.post<ReadParteDto>('/api/Parte',createParteDto);
    }


}