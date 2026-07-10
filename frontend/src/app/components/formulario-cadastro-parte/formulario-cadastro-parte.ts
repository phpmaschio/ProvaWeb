import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ParteService } from '../../services/parte.service';

@Component({
  selector: 'app-formulario-cadastro-parte',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule
  ], templateUrl: './formulario-cadastro-parte.html',
  styleUrl: './formulario-cadastro-parte.scss',
})
export class FormularioCadastroParte {
  formulario!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    protected dialogRef: MatDialogRef<FormularioCadastroParte>,
    private parteService:ParteService
  ) {
    this.formulario = formBuilder.group({
      nome: ['', Validators.required],
      tipo: ['', Validators.required]
    })
  }

  salvar() {
    this.parteService.postParte(this.formulario.value).subscribe({
      next: (response) => {
        if(response){
          this.dialogRef.close(response);
        }
      }
    })
  }
}
