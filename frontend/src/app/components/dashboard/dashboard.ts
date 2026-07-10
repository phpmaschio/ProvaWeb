import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormularioCadastroProcesso } from '../formulario-cadastro-processo/formulario-cadastro-processo';
import { ProcessoService } from '../../services/processo.service';
import { CommonModule } from '@angular/common';
import { MatSpinner } from '@angular/material/progress-spinner';
import { DadosDetalhadosProcesso } from '../dados-detalhados-processo/dados-detalhados-processo';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, MatButtonModule, MatIcon, CommonModule, MatSpinner, MatTooltipModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {

  private todosProcessos: ReadProcessoDto[] = [];
  protected processos: ReadProcessoDto[] = [];
  protected loading: boolean = false;

  constructor(
    private processoService: ProcessoService, 
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.fetchProcessos();
  }

  fetchProcessos() {
    this.loading = true;
    this.cdr.detectChanges();

    this.processoService.getProcessos().subscribe({
      next: (response: ReadProcessoDto[]) => {
        this.todosProcessos = response || [];
        this.processos = [...this.todosProcessos];
        this.loading = false;
        this.cdr.markForCheck();
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();

    if (!filterValue) {
      this.processos = [...this.todosProcessos];
      return;
    }

    this.processos = this.todosProcessos.filter(p => {
      const textoParaBusca = `
        ${p.id} 
        ${p.descricao} 
        ${p.statusProcesso?.descricao ?? ''} 
        ${p.andamento?.descricao ?? ''}
      `.toLowerCase();

      return textoParaBusca.includes(filterValue);
    });
  }

  criarNovoProcesso() {
    this.dialog.open(FormularioCadastroProcesso, {
      width: '90vw',
      maxWidth: '500px',
      maxHeight: '90vh',
      data: { editando: false }
    }).afterClosed().subscribe(() => {
      this.fetchProcessos();
    });
  }

  editarProcesso(processo: ReadProcessoDto) {
    this.dialog.open(FormularioCadastroProcesso, {
      width: '90vw',
      maxWidth: '500px',
      maxHeight: '90vh',
      data: { processo: processo, edicao: true }
    }).afterClosed().subscribe(() => {
      this.fetchProcessos();
    });
  }

  excluirProcesso(processoId: number) {
    this.dialog.open(ConfirmDialog, {
      width: '90vw',
      maxWidth: '400px',
      data: {
        titulo: 'Excluir processo',
        mensagem: 'Deseja realmente excluir o processo?'
      }
    }).afterClosed().subscribe((confirmado: boolean) => {
      if (confirmado) {
        this.processoService.deleteProcesso(processoId).subscribe({
          next: () => {
            this.fetchProcessos();
          }
        });
      }
    });
  }

  visualizarProcesso(processo: ReadProcessoDto) {
    this.dialog.open(DadosDetalhadosProcesso, {
      width: '90vw',
      maxWidth: '600px',
      maxHeight: '90vh',
      data: { processo: processo }
    });
  }
}