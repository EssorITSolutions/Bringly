"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var core_1 = require("@angular/core");
var http_1 = require("@angular/common/http");
var angular2_moment_1 = require("angular2-moment");
var ngx_infinite_scroll_1 = require("ngx-infinite-scroll");
var ngx_spinner_1 = require("ngx-spinner");
var app_routing_module_1 = require("./app-routing.module");
var app_component_1 = require("./app.component");
var not_found_component_1 = require("./not-found/not-found.component");
var test_component_1 = require("./test/test.component");
var buyer_review_component_1 = require("./buyer-review/buyer-review.component");
var api_service_service_1 = require("./services/shared-service/api-service.service");
var merchant_myorder_component_1 = require("./merchant-myorder/merchant-myorder.component");
var app_setting_1 = require("./utility/app-setting");
var LoadInitialSettingService = /** @class */ (function () {
    function LoadInitialSettingService() {
    }
    LoadInitialSettingService.prototype.getAppUrl = function () {
        return new Promise(function (resolve, reject) {
            app_setting_1.AppSettings.AppUrl = window.location.origin;
            resolve(true);
        });
    };
    LoadInitialSettingService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], LoadInitialSettingService);
    return LoadInitialSettingService;
}());
exports.LoadInitialSettingService = LoadInitialSettingService;
function init_app(loadInitialSettingService) {
    return function () { return loadInitialSettingService.getAppUrl(); };
}
exports.init_app = init_app;
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
                not_found_component_1.NotFoundComponent,
                test_component_1.TestComponent,
                buyer_review_component_1.BuyerReviewComponent,
                merchant_myorder_component_1.MerchantMyOrderComponent
            ],
            imports: [
                platform_browser_1.BrowserModule,
                http_1.HttpClientModule,
                angular2_moment_1.MomentModule,
                ngx_infinite_scroll_1.InfiniteScrollModule,
                ngx_spinner_1.NgxSpinnerModule,
                app_routing_module_1.AppRoutingModule
            ],
            providers: [api_service_service_1.ApiServiceService,
                LoadInitialSettingService,
                { provide: core_1.APP_INITIALIZER, useFactory: init_app, deps: [LoadInitialSettingService], multi: true },
            ],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map