import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // using angular to verify info 
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  login() {
    // goes to auth.service.ts to use login and stores/retrives token 
    this.authService.login(this.model).subscribe(next => {
      console.log('logged in successfully');
    }, error => {
      console.log('failed to login');
    });
  }
}
