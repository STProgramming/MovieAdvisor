import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment.development';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  constructor(){}

  ngOnInit(){}

  getEmailDeveloper():string{
    return environment.emailDeveloper;
  }

  getNameDeveloper():string{
    return environment.nameDeveloper;
  }
}
