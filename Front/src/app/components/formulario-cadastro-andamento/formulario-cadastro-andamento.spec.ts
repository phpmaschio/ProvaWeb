import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioCadastroAndamento } from './formulario-cadastro-andamento';

describe('FormularioCadastroAndamento', () => {
  let component: FormularioCadastroAndamento;
  let fixture: ComponentFixture<FormularioCadastroAndamento>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioCadastroAndamento],
    }).compileComponents();

    fixture = TestBed.createComponent(FormularioCadastroAndamento);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
