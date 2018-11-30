import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model:any={}
//isLoggedIn:boolean=false;
loginForm:any
  constructor(private authService:AuthService) { }

  ngOnInit() {
  }

  login(){
    this.authService.login(this.model).subscribe(suc=>{
      console.log("Logged in successfully");
      console.log('and the response is ',suc)
  //    this.isLoggedIn=true;
    },err=>{
      console.log('something very bad occured');
    })
  }

  loggedIn(){
    const tokenVar=localStorage.getItem('token');
    return !!tokenVar;
  }

  logout(){
    localStorage.removeItem('token');
  }

}
