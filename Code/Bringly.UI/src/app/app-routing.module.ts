import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { AppComponent } from '../app/app.component';
import { TestComponent } from './test/test.component';
import { BuyerReviewComponent } from './buyer-review/buyer-review.component';
import { MerchantMyOrderComponent } from './merchant-myorder/merchant-myorder.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: MerchantMyOrderComponent,
        children: []
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule {
 
}