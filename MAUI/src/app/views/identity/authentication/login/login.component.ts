import { Component } from '@angular/core';
import { IdentityService } from '../../identity-services/identity.service';
import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../../shared/services/authentication.service';
import { LoginDto } from '../../../../shared/models/login-dto';
import { RouteService } from '../../../../shared/services/route.service';
import { RoleService } from '../../../../shared/services/role.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loading = false;
  loginGoogleObservable: Observable<any>;
  loginMAObservable: Observable<any>;
  loginForm: FormGroup;
  isAuth: Observable<boolean> = this.authService.authenticated;

  constructor(private identityServices: IdentityService,
    private formBuilder : FormBuilder,
    private authService: AuthenticationService,
    private roleService: RoleService,
    private routeService: RouteService,
    private toastr: ToastrService){}

  ngOnInit(): void{
    this.initializeForm();
  }

  initializeForm(){
    this.loginForm = this.formBuilder.group({
      'email': ['', [Validators.required, Validators.email]],
      'password': ['', Validators.required],
      'conn': [false, Validators.required]
    });
  }

  loginWithGoogle(){
    this.loginGoogleObservable = this.identityServices.loginWithGoogle()
    this.loginGoogleObservable.subscribe((response: any) => {
      console.log(response);
    });
  }

  redirectToHome(){
    this.routeService.goHome();
  }

  onSubmit(loginForm: FormGroup){
    if(loginForm.valid){
      this.loading = true;
      var login: LoginDto = {
        'email': loginForm.get('email').value,
        'password': loginForm.get('password').value,
        'stayConnected': loginForm.get('conn').value
      };
      this.loginMAObservable = this.identityServices.loginMovieAdvisor(login);
      this.loginMAObservable.subscribe((resp : any) => {
        if(resp){
          var userName = this.authService.authSuccessfull(resp.token);
          this.roleService.isAppAdmin(true);
          this.toastr.success('Bentornat* '+userName, 'Login effettuata');
        }
        else{
          this.authService.authFail();
          this.toastr.error('Email o password errati', 'Login fallita');
        }
        this.loading = false;
      });
    }
  }
}
