import { Component, Inject, OnInit, ChangeDetectorRef } from '@angular/core'; // Adicionado OnInit e ChangeDetectorRef
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ReadStatusProcessoDto } from '../../models/read-status-processo-dto';
import { StatusProcessoService } from '../../services/status-processo.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from "@angular/material/button";
import { ReadAndamentoAtualDto } from '../../models/read-andamento-dto';
import { AndamentoService } from '../../services/andamento-service.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormularioCadastroStatusProcesso } from '../formulario-cadastro-status-processo/formulario-cadastro-status-processo';
import { FormularioCadastroAndamento } from '../formulario-cadastro-andamento/formulario-cadastro-andamento';
import { ProcessoService } from '../../services/processo-service.service';
import { ReadParteDto } from '../../models/read-parte-dto';
import { ParteService } from '../../services/parte-service.service';
import { FormularioCadastroParte } from '../formulario-cadastro-parte/formulario-cadastro-parte';
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { forkJoin } from 'rxjs'; // <-- IMPORTANTE: Importar o forkJoin

@Component({
  selector: 'app-formulario-cadastro-processo',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinner
  ],
  templateUrl: './formulario-cadastro-processo.html',
  styleUrl: './formulario-cadastro-processo.scss',
})
export class FormularioCadastroProcesso implements OnInit { // Implementar OnInit

  protected formulario!: FormGroup;
  protected statusProcessos: ReadStatusProcessoDto[] = [];
  protected andamentos: ReadAndamentoAtualDto[] = [];
  protected partes: ReadParteDto[] = [];
  protected loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private statusProcessoService: StatusProcessoService,
    private andamentoService: AndamentoService,
    private parteService: ParteService,
    protected dialogRef: MatDialogRef<FormularioCadastroProcesso>,
    private dialog: MatDialog,
    private processoService: ProcessoService,
    private cdr: ChangeDetectorRef, // <-- Injetando o CDR
    @Inject(MAT_DIALOG_DATA) private data: { processo?: ReadProcessoDto, edicao: boolean }
  ) {
    this.formulario = this.formBuilder.group({
      descricao: ['', Validators.required],
      statusProcessoId: [null, Validators.required],
      partes: [[], Validators.required],
      andamento: [null, Validators.required],
    });
  }

  // Usamos o OnInit para não sobrecarregar o construtor
  ngOnInit(): void {
    this.carregarDadosIniciais();
  }

  carregarDadosIniciais() {
    this.loading = true;

    // forkJoin espera todas as requisições terminarem ao mesmo tempo
    forkJoin({
      status: this.statusProcessoService.getStatusProcessos(),
      andamentos: this.andamentoService.getAndamento(),
      partes: this.parteService.getPartes()
    }).subscribe({
      next: (resultados) => {
        // 1. Populamos as listas
        this.statusProcessos = resultados.status;
        this.andamentos = resultados.andamentos;
        this.partes = resultados.partes;

        // 2. AGORA SIM preenchemos o formulário (pois as listas já tem dados para o mat-select ler)
        if (this.data.edicao && this.data.processo) {
          this.formulario.patchValue({
            descricao: this.data.processo.descricao,
            statusProcessoId: this.data.processo.statusProcesso.id,
            partes: this.data.processo.partes,
            andamento: this.data.processo.andamento
          });
        }

        // 3. Desligamos o loading e forçamos a tela a atualizar
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Erro ao carregar dados iniciais:', error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // --- Mantivemos os métodos individuais para quando você adicionar um item novo nos modais menores ---

  fetchStatusProcessos() {
    this.loading = true;
    this.statusProcessoService.getStatusProcessos().subscribe({
      next: (response: ReadStatusProcessoDto[]) => {
        if (response) this.statusProcessos = response;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  fetchAndamentos() {
    this.loading = true;
    this.andamentoService.getAndamento().subscribe({
      next: (response: ReadAndamentoAtualDto[]) => {
        if (response) this.andamentos = response;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  fetchPartes() {
    this.loading = true;
    this.parteService.getPartes().subscribe({
      next: (response: ReadParteDto[]) => {
        if (response) this.partes = response;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // --- Modais de Cadastro ---

  abrirFormularioCadastroStatusProcesso() {
    this.dialog.open(FormularioCadastroStatusProcesso, {
      width: '500px',
      height: '200px'
    }).afterClosed().subscribe({
      next: (result) => {
        if (result) {
          // Após cadastrar, buscamos novamente apenas a lista de status
          this.fetchStatusProcessos(); 
          this.formulario.get('statusProcessoId')?.setValue(result.id);
        }
      }
    });
  }

  abrirFormularioCadastroAndamento() {
    this.dialog.open(FormularioCadastroAndamento, {
      width: '500px',
      height: '200px'
    }).afterClosed().subscribe({
      next: (result: ReadAndamentoAtualDto) => {
        if (result) {
          this.andamentos.push(result);
          this.formulario.get('andamento')?.setValue(result);
        }
      }
    });
  }

  abrirFormularioCadastroParte() {
    this.dialog.open(FormularioCadastroParte, {
      width: '500px',
      minHeight: '280px'
    }).afterClosed().subscribe({
      next: (result: ReadParteDto) => {
        // CORRIGIDO: Estava this.fetchPartes sem os parênteses
        this.fetchPartes(); 
      },
      error: (error) => {
        alert('Erro ao cadastrar parte');
        console.error(error);
      }
    });
  }

  salvar() {
    var createProcessoDto = this.formulario.value;

    if (!this.data.edicao)
      this.processoService.postProcesso(createProcessoDto).subscribe({
        next: (response) => {
          if (response) {
            this.dialogRef.close();
          }
        },
        error: (error) => {
          alert("Erro ao registrar processo");
          console.error(error);
        }
      });
    else {
      this.processoService.updateProcesso(this.data.processo!.id, createProcessoDto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => {
          alert("Erro ao atualizar processo");
          console.error(error);
        }
      });
    }
  }

  compararPorId(opcao1: any, opcao2: any): boolean {
    return opcao1 && opcao2 ? opcao1.id === opcao2.id : opcao1 === opcao2;
  }
}