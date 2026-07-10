import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DadosDetalhadosProcesso } from './dados-detalhados-processo';

describe('DadosDetalhadosProcesso', () => {
  let component: DadosDetalhadosProcesso;
  let fixture: ComponentFixture<DadosDetalhadosProcesso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DadosDetalhadosProcesso],
    }).compileComponents();

    fixture = TestBed.createComponent(DadosDetalhadosProcesso);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
