import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { IdentityService } from '../../identity-services/identity.service';
import { ToastrService } from 'ngx-toastr';
import { RouteService } from '../../../../shared/services/route.service';
import { NewUserDto } from '../../../../shared/models/new-user-dto';
import { AuthenticationService } from '../../../../shared/services/authentication.service';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrl: './new-user.component.scss'
})
export class NewUserComponent {
  newUserForm: FormGroup;
  NewAccountObservable : Observable<any>;
  isAuth: Observable<boolean> = this.authService.authenticated;
  isLoading: boolean = false;

  constructor(private identityService: IdentityService,
    private toastr: ToastrService,
    private routeService: RouteService,
    private formBuilder: FormBuilder,
    private authService: AuthenticationService    
  ){}

  ngOnInit():void{
    this.initForm();
  }

  initForm(){
    this.newUserForm = this.formBuilder.group({
      name: ['', Validators.required],
      lastname: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      username:['', Validators.required],
      birthDate: [new Date, Validators.required],
      gender: ['', Validators.required],
      nationality: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirm: ['', [Validators.required, Validators.minLength(8)]],
      gdpr: [false, Validators.required]
    });
  }

  redirectToHome(){
    this.routeService.goHome();
  }

  onSubmit(newUserForm: FormGroup){
    if(newUserForm.valid && newUserForm.get('password').value == newUserForm.get('confirm').value && newUserForm.get('gdpr').value == true){
      this.isLoading = true;
      var newUser: NewUserDto = { 
        'name' : newUserForm.get('name').value,
        'lastName' : newUserForm.get('lastname').value,
        'email' : newUserForm.get('email').value,
        'userName' : newUserForm.get('username').value,
        'birthDate' : new Date(newUserForm.get('birthDate').value),
        'gender': newUserForm.get('gender').value,
        'nationality': newUserForm.get('nationality').value,
        'password': newUserForm.get('password').value
      };  
      this.postNewUser(newUser);
    }
  }

  postNewUser(newUser: NewUserDto){
    this.NewAccountObservable = this.identityService.createNewUser(newUser);
    this.NewAccountObservable.subscribe({
      next: (resp)=>{
        this.toastr.success('Benvenut* '+ newUser.name + ' ' + newUser.lastName, 'Registrazione avvenuta con successo');
        this.routeService.goHome();
      },
      error: (error) => {
        this.toastr.error('Sembra ci sia stato un problema, ti invitiamo a riprovare più tardi', 'Ops.. qualcosa è andato storto');
        this.isLoading = false;
      },
      complete : () => {
        this.isLoading = false;
      }
    })
  }
}
