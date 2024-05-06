import { Component, EventEmitter, Output } from '@angular/core';
import { NewRequestDto } from '../../../shared/models/new-request-dto';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-new-request',
  templateUrl: './new-request.component.html',
  styleUrl: './new-request.component.scss'
})
export class NewRequestComponent {
  userResponses: NewRequestDto;  
  questions: string[] = ['Cosa vorresti vedere?', 'Come ti senti?'];
  @Output() answersEmitter = new EventEmitter<NewRequestDto>();
  answers: FormGroup; 
  dataAnswers: NewRequestDto = null;
  counter: number = 0; 

  constructor(private formBuilder: FormBuilder){}

  ngOnInit(): void{
    this.initializeForm();
  }

  initializeForm(){
    this.answers = this.formBuilder.group({
      whatWant: ['', Validators.required],
      howFeel: ['', Validators.required]
    });
  }

  submitAnswer(values: FormGroup){
    if(values.valid){
      switch(this.counter){
        case 0:
          this.dataAnswers.whatClientWant = values.get('whatWant').value;
          break;
        case 1:
          this.dataAnswers.howClientFeels = values.get('howFeel').value;
          break;
        default:
          break;                
      }   
      if(this.counter == 1){
        this.answersEmitter.emit(this.dataAnswers);
      }
      else{
        this.counter ++;   
      }
    }
  }
}
