import { Component } from '@angular/core';
import { StatusProcessoService } from '../../services/status-processo.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-formulario-cadastro-status-processo',
 imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './formulario-cadastro-status-processo.html',
  styleUrl: './formulario-cadastro-status-processo.scss',
})
export class FormularioCadastroStatusProcesso {

  formulario!:FormGroup;
  constructor(
    private statusProcessoService:StatusProcessoService,
    private formBuilder: FormBuilder,
    protected dialogRef: MatDialogRef<FormularioCadastroStatusProcesso>
  ){
    this.formulario = formBuilder.group({
      descricao:['', Validators.required]
    })
  }

  salvar(){
    this.statusProcessoService.postStatusProcesso(this.formulario.value).subscribe({
      next:(response) => {
        if(response){
          this.dialogRef.close(response);
        }
      }
    })

  }
}
