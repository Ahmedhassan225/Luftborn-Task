import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { IdentityRoutingModule } from './identity-routing.module';
import { LoginPage } from './pages/login/login.page';

@NgModule({
  declarations: [

    
    LoginPage
  ],
  imports: [
    CommonModule,
    IdentityRoutingModule,
    SharedModule
  ],
  providers:[]
})
export class IdentityModule { }
