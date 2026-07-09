import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // Adicionado OnInit e ChangeDetectorRef
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormularioCadastroProcesso } from '../formulario-cadastro-processo/formulario-cadastro-processo';
import { ProcessoService } from '../../services/processo-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, MatButtonModule, MatIcon, CommonModule], 
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  protected processos: ReadProcessoDto[] = [];
  protected filtro: string = '';

  constructor(
    private processoService: ProcessoService, 
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.fetchProcessos();
  }

  fetchProcessos() {
    this.processoService.getProcessos().subscribe({
      next: (response: ReadProcessoDto[]) => {
        if (response) {
          this.processos = response;
          this.cdr.detectChanges();
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  criarNovoProcesso() {
    this.dialog.open(FormularioCadastroProcesso, {
      minWidth: '500px',
      minHeight: '400px',
      data:{editando:false}
    }).afterClosed().subscribe(() => {
      this.fetchProcessos();
    });
  }

  editarProcesso(processo:ReadProcessoDto) {
    this.dialog.open(FormularioCadastroProcesso, {
      minWidth: '500px',
      minHeight: '400px',
      data:{processo: processo, edicao:true}
    }).afterClosed().subscribe(() => {
      this.fetchProcessos();
    });
  }


  excluirProcesso(processoId: number) {
    if (confirm('Deseja realmente excluir o processo?')) {
      this.processoService.deleteProcesso(processoId).subscribe({
        next: () => {
          this.fetchProcessos();
        },
        error: (error) => {
          alert('Erro ao excluir o processo');
          console.error(error);
        }
      });
    }
  }
}