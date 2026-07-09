import { Component } from '@angular/core';
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
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormularioCadastroStatusProcesso } from '../formulario-cadastro-status-processo/formulario-cadastro-status-processo';
import { FormularioCadastroAndamento } from '../formulario-cadastro-andamento/formulario-cadastro-andamento';
import { CreateAndamentoDto } from '../../models/create-andamento-dto';
import { ProcessoService } from '../../services/processo-service.service';
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
  protected andamentos!: CreateAndamentoDto[];

  constructor(
    private formBuilder: FormBuilder,
    private statusProcessoService: StatusProcessoService,
    private andamentoService: AndamentoService,
    protected dialogRef: MatDialogRef<FormularioCadastroProcesso>,
    private dialog: MatDialog,
    private processoService: ProcessoService
  ) { }

  ngOnInit(): void {
    this.formulario = this.formBuilder.group({
      descricao: ['', Validators.required],
      statusProcessoId: [null, Validators.required],
      partes: [[], Validators.required],
      andamento: [null, Validators.required],
    });
    this.fetchStatusProcessos();
    this.fetchAndamentos();
  }

  fetchStatusProcessos() {
    this.statusProcessoService.getStatusProcessos().subscribe({
      next: (response: ReadStatusProcessoDto[]) => {
        if (response) {
          this.statusProcessos = response
          console.log("StatusProcessoResponse",response);
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  fetchAndamentos() {
    this.andamentoService.getAndamento().subscribe({
      next: (response: CreateAndamentoDto[]) => {
        if (response) {
          this.andamentos = response
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

  salvar() {
    var createProcessoDto = this.formulario.value;
    this.processoService.postProcessos(createProcessoDto).subscribe({
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
  }
}

