import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ReadProcessoDto } from '../../models/read-processo-dto';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ReadAndamentoAtualDto } from '../../models/read-andamento-dto';
import { AndamentoService } from '../../services/andamento-service.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { MatSort, MatSortModule } from '@angular/material/sort'; 

@Component({
  selector: 'app-dados-detalhados-processo',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    CommonModule,
  ],
  templateUrl: './dados-detalhados-processo.html',
  styleUrl: './dados-detalhados-processo.scss',
})
export class DadosDetalhadosProcesso implements AfterViewInit, OnInit {
  protected processo!: ReadProcessoDto;
  protected dataSource: MatTableDataSource<ReadAndamentoAtualDto> = new MatTableDataSource<ReadAndamentoAtualDto>();
  protected displayedColumns: string[] = ['id', 'descricao', 'atribuidoEm'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    protected dialogRef: MatDialogRef<DadosDetalhadosProcesso>,
    @Inject(MAT_DIALOG_DATA) private data: { processo: ReadProcessoDto },
    private andamentoService: AndamentoService,
  ) {
    this.processo = this.data.processo;
  }

  ngOnInit(){
    this.fetchAndamentos();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  fetchAndamentos() {
    this.andamentoService.getAndamentoPorProcesso(this.processo.id).subscribe({
      next: (response) => {
        this.dataSource.data = response;
        this.dataSource.paginator = this.paginator; 
      },
      error: (error) => {
        alert("Erro ao carregar andamentos");
        console.error(error);
      }
    });
  }
}
