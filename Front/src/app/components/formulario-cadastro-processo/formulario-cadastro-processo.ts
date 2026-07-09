import { Component, Inject } from '@angular/core';
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
import { CreateAndamentoDto } from '../../models/create-andamento-dto';
import { ProcessoService } from '../../services/processo-service.service';
import { ReadParteDto } from '../../models/read-parte-dto';
import { ParteService } from '../../services/parte-service.service';
import { FormularioCadastroParte } from '../formulario-cadastro-parte/formulario-cadastro-parte';
import { ReadProcessoDto } from '../../models/read-processo-dto';
@Component({
  selector: 'app-formulario-cadastro-processo',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './formulario-cadastro-processo.html',
  styleUrl: './formulario-cadastro-processo.scss',
})
export class FormularioCadastroProcesso {

  protected formulario!: FormGroup;
  protected statusProcessos!: ReadStatusProcessoDto[];
  protected andamentos!: ReadAndamentoAtualDto[];
  protected partes!: ReadParteDto[]

  constructor(
    private formBuilder: FormBuilder,
    private statusProcessoService: StatusProcessoService,
    private andamentoService: AndamentoService,
    private parteService: ParteService,
    protected dialogRef: MatDialogRef<FormularioCadastroProcesso>,
    private dialog: MatDialog,
    private processoService: ProcessoService,
    @Inject(MAT_DIALOG_DATA) private data: { processo?: ReadProcessoDto, edicao: boolean }
  ) {
    this.formulario = this.formBuilder.group({
      descricao: ['', Validators.required],
      statusProcessoId: [null, Validators.required],
      partes: [[], Validators.required],
      andamento: [null, Validators.required],
    });
    this.fetchStatusProcessos();
    this.fetchAndamentos();
    this.fetchPartes();
    if (this.data.edicao && this.data.processo) {
      console.log("processo",this.data.processo);
      this.formulario.get('descricao')?.setValue(this.data.processo?.descricao);
      this.formulario.get('statusProcessoId')?.setValue(this.data.processo?.statusProcesso.id);
      this.formulario.get('partes')?.setValue(this.data.processo?.partes);
      this.formulario.get('andamento')?.setValue(this.data.processo?.andamento);
    }
  }

  fetchStatusProcessos() {
    this.statusProcessoService.getStatusProcessos().subscribe({
      next: (response: ReadStatusProcessoDto[]) => {
        if (response) {
          this.statusProcessos = response
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  fetchAndamentos() {
    this.andamentoService.getAndamento().subscribe({
      next: (response: ReadAndamentoAtualDto[]) => {
        if (response) {
          this.andamentos = response
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  fetchPartes() {
    this.parteService.getPartes().subscribe({
      next: (response: ReadParteDto[]) => {
        if (response) {
          this.partes = response
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  abrirFormularioCadastroStatusProcesso() {
    this.dialog.open(FormularioCadastroStatusProcesso, {
      width: '500px',
      height: '200px'
    }).afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.statusProcessoService.getStatusProcessos().subscribe({
            next: (response: ReadStatusProcessoDto[]) => {
              this.statusProcessos = response;
              this.formulario.get('statusId')?.setValue(result.id);
            },
            error: (error) => console.error(error)
          });
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
    }
    );
  }

  abrirFormularioCadastroParte() {
    this.dialog.open(FormularioCadastroParte, {
      width: '500px',
      minHeight: '280px'
    }).afterClosed().subscribe({
      next: (result: ReadParteDto) => {
        this.fetchPartes
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
          alert("Erro ao registrar processo")
          console.error(error);
        }
      })
    else {
      this.processoService.updateProcesso(this.data.processo!.id,createProcessoDto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => {
          alert("Erro ao atualizar processo")
          console.error(error);
        }
      })
    }
  }

  compararPorId(opcao1: any, opcao2: any): boolean {
    return opcao1 && opcao2 ? opcao1.id === opcao2.id : opcao1 === opcao2;
  }
}

