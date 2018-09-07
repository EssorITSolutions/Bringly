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
var core_1 = require("@angular/core");
var api_service_service_1 = require("../services/shared-service/api-service.service");
var app_setting_1 = require("../utility/app-setting");
var BuyerReviewComponent = /** @class */ (function () {
    function BuyerReviewComponent(_apiService) {
        this._apiService = _apiService;
        this.isMerchant = false;
        this._apiEndPoint = app_setting_1.AppSettings.AppUrl + "/orders/getmerchantmyorders";
    }
    BuyerReviewComponent.prototype.ngOnInit = function () {
        this.fetchUserData();
    };
    BuyerReviewComponent.prototype.fetchUserData = function () {
        var _this = this;
        this._apiService.getAll(this._apiEndPoint).subscribe(function (response) {
            if (response && response.reviewData) {
                _this.myreviewData = response.reviewData;
                _this.isMerchant = response.isMerchant;
            }
        }, function (error) {
            console.log(error);
        });
    };
    BuyerReviewComponent.prototype.getDate = function (date) {
        if (date) {
            return new Date(Number(date.substring(date.indexOf("(") + 1, date.indexOf(")")))).toUTCString();
        }
        return null;
    };
    BuyerReviewComponent = __decorate([
        core_1.Component({
            templateUrl: "./buyer-review.component.html"
        }),
        __metadata("design:paramtypes", [api_service_service_1.ApiServiceService])
    ], BuyerReviewComponent);
    return BuyerReviewComponent;
}());
exports.BuyerReviewComponent = BuyerReviewComponent;
//# sourceMappingURL=buyer-review.component.js.map