import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmprestimoListaComponent } from './emprestimo-lista.component';

describe('EmprestimoListaComponent', () => {
  let component: EmprestimoListaComponent;
  let fixture: ComponentFixture<EmprestimoListaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmprestimoListaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmprestimoListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
