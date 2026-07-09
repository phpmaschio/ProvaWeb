import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioCadastroParte } from './formulario-cadastro-parte';

describe('FormularioCadastroParte', () => {
  let component: FormularioCadastroParte;
  let fixture: ComponentFixture<FormularioCadastroParte>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioCadastroParte],
    }).compileComponents();

    fixture = TestBed.createComponent(FormularioCadastroParte);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
