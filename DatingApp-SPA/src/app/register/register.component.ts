import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  values: any;
  constructor(private authService: AuthService, private http: HttpClient ) { }

  ngOnInit() {
  }

  //////////
  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('registration successful');
    }, error => {
      console.log('yeee')
    });
  
  }///////

  getValues(){
    this.http.get('http://localhost:5000/api/users').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });

  }
  cancel(){
    console.log('canceled');
  }
}
