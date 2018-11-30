import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode:boolean=false;
  values:any;
  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  regToggle(){
    this.registerMode=true;
  }

  disableReg(xxx){
    this.registerMode=xxx;
  }
  
  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(res => {
      this.values = res;
      console.log(res);
    }, err => {
      console.log(err);
    });
  }
}
