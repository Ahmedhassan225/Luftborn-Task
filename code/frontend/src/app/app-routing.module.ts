import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './page/app.component';
import { NotFoundComponent } from './page/not-found/not-found.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('src/app/features/product').then(m => m.ProductModule),
  
  },
  {
    path: 'identity',
    loadChildren: () => import('src/app/features/identity').then(m => m.IdentityModule),
  },
  {    
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
