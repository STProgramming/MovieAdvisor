import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrl: './paginator.component.scss'
})
export class PaginatorComponent {
  @Input() currentPage: number;
  @Input() totPages: number;
  @Output() pageChange = new EventEmitter<number>();

  constructor() { }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totPages) {
      this.pageChange.emit(page);
    }
  }

  nextPage() {
    this.goToPage(this.currentPage + 1);
  }

  previousPage() {
    this.goToPage(this.currentPage - 1);
  }
}
