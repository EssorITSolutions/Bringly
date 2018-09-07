import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Injectable, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { MomentModule } from 'angular2-moment';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { TestComponent } from './test/test.component';
import { BuyerReviewComponent } from './buyer-review/buyer-review.component';
import { ApiServiceService } from './services/shared-service/api-service.service';
import { MerchantMyOrderComponent } from './merchant-myorder/merchant-myorder.component';
import { AppSettings } from './utility/app-setting';

@Injectable()
export class LoadInitialSettingService {
    constructor() { }

    getAppUrl(): Promise<any> {
        return new Promise((resolve, reject) => {
            AppSettings.AppUrl = window.location.origin;
            resolve(true);
        });
    }
}


export function init_app(loadInitialSettingService: LoadInitialSettingService) {
    return () => loadInitialSettingService.getAppUrl();
}

@NgModule({
    declarations: [
        AppComponent,
        NotFoundComponent,
        TestComponent,
        BuyerReviewComponent,
        MerchantMyOrderComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        MomentModule,
        InfiniteScrollModule,
        NgxSpinnerModule,
        AppRoutingModule
    ],
    providers: [ApiServiceService,
        LoadInitialSettingService,
        { provide: APP_INITIALIZER, useFactory: init_app, deps: [LoadInitialSettingService], multi: true },
    ],
    bootstrap: [AppComponent]
})
export class AppModule {

}
