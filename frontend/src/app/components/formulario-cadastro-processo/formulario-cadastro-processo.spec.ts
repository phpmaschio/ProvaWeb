import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioCadastroProcesso } from './formulario-cadastro-processo';

describe('FormularioCadastroProcesso', () => {
  let component: FormularioCadastroProcesso;
  let fixture: ComponentFixture<FormularioCadastroProcesso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioCadastroProcesso],
    }).compileComponents();

    fixture = TestBed.createComponent(FormularioCadastroProcesso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
