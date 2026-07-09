import { Component } from '@angular/core';
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormularioCadastroProcesso } from '../formulario-cadastro-processo/formulario-cadastro-processo';
import { ProcessoService } from '../../services/processo-service.service';
@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, MatButtonModule, MatIcon], templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {
  public processos: ReadProcessoDto[] = [];
  public filtro: string = '';
  constructor(private processoService: ProcessoService, private dialog: MatDialog) { }

  fetchProcessos() {
    this.processoService.getProcessos().subscribe({
      next: (response: ReadProcessoDto[]) => {

        if (response) {
          this.processos = response;
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  criarNovoProcesso() {
    this.dialog.open(FormularioCadastroProcesso,{
      minWidth:'500px',
      minHeight:'400px'
    }).afterClosed().subscribe(() => {
      this.fetchProcessos();
    })
  }
}
