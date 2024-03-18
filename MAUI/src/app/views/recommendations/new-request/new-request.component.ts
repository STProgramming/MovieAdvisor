import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-new-request',
  templateUrl: './new-request.component.html',
  styleUrl: './new-request.component.scss'
})
export class NewRequestComponent {

  @Output() stringEmitter = new EventEmitter<string>();

  constructor(){}

  ngOnInit(): void{
    
  }
}
