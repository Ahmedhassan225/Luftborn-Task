import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListPage } from './pages/product-list/product-list.page';
import { AuthGuard } from 'src/app/shared';

const routes: Routes = [
  {
    path: '',
    component: ProductListPage,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
