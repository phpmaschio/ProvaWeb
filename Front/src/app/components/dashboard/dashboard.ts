import { ChangeDetectorRef, Component, OnInit } from '@angular/core'; 
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormularioCadastroProcesso } from '../formulario-cadastro-processo/formulario-cadastro-processo';
import { ProcessoService } from '../../services/processo-service.service';
import { CommonModule } from '@angular/common';
import { MatSpinner } from '@angular/material/progress-spinner';
import { DadosDetalhadosProcesso } from '../dados-detalhados-processo/dados-detalhados-processo';

@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, MatButtonModule, MatIcon, CommonModule, MatSpinner], 
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  protected processos: ReadProcessoDto[] = [];
  protected filtro: string = '';
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
        this.processos = response || [];
        this.loading = false;
        this.cdr.markForCheck(); 
        this.cdr.detectChanges(); 
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
        this.cdr.detectChanges();
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

  editarProcesso(processo: ReadProcessoDto) {
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

  visualizarProcesso(processo:ReadProcessoDto){
    this.dialog.open(DadosDetalhadosProcesso,{
      minWidth:'600px',
      height: '600px',
      data:{processo:processo}
    })
  }
}