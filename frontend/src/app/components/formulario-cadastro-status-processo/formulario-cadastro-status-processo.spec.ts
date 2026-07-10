import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioCadastroStatusProcesso } from './formulario-cadastro-status-processo';

describe('FormularioCadastroStatusProcesso', () => {
  let component: FormularioCadastroStatusProcesso;
  let fixture: ComponentFixture<FormularioCadastroStatusProcesso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioCadastroStatusProcesso],
    }).compileComponents();

    fixture = TestBed.createComponent(FormularioCadastroStatusProcesso);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
