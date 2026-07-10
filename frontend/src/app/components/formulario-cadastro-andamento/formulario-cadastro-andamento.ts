import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-formulario-cadastro-andamento',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './formulario-cadastro-andamento.html',
  styleUrl: './formulario-cadastro-andamento.scss',
})
export class FormularioCadastroAndamento {
  formulario!:FormGroup;

  constructor(private formBuilder: FormBuilder, protected dialogRef:MatDialogRef<FormularioCadastroAndamento>){
    this.formulario = formBuilder.group({
      descricao:['',Validators.required]
    })
  }

  salvar(){
    this.dialogRef.close(this.formulario.value);
  }

}
