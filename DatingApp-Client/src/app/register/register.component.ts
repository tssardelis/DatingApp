import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesTest:any;
  @Output() cancelReg=new EventEmitter()
  model:any={}
  constructor(private authService:AuthService) { }

  ngOnInit() {
  }

  register(){
    this.authService.register(this.model).subscribe(()=>{
      console.log('Registratioon Successful')
    },err=>{
      console.log(err)
    })
  }

  cancel(){
    this.cancelReg.emit(false)
  }

}
