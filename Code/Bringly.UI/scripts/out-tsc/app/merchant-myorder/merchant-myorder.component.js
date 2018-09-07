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
var request_query_model_1 = require("../models/request-query-model");
var ngx_spinner_1 = require("ngx-spinner");
var sweetalert2_1 = require("sweetalert2");
var app_setting_1 = require("../utility/app-setting");
var router_1 = require("@angular/router");
var MerchantMyOrderComponent = /** @class */ (function () {
    function MerchantMyOrderComponent(_apiService, spinner, _activatedRoute) {
        this._apiService = _apiService;
        this.spinner = spinner;
        this._activatedRoute = _activatedRoute;
        this.activeTab = "pending";
        this.viewTotalOfOrder = 0;
        this.buttonStartFrom = 1;
        this.buttonEndTo = 10;
        this.orderStatusList = [];
        this.requestQuery = new request_query_model_1.RequestQuery();
        this._apiEndPointGetMerchants = app_setting_1.AppSettings.AppUrl + "/orders/getmerchantmyorders";
        this._apiEndPointUpdateOrderStatus = app_setting_1.AppSettings.AppUrl + "/orders/updateorderstatus";
        this.requestQuery.CurrentPage = 1;
        this.requestQuery.PageSize = 10;
        this.requestQuery.SearchQuery = '';
        this.requestQuery.ScrollBy = 'pending';
        this.orderStatusList.push({ Id: 100, StatusCode: 100, StatusDisplayName: 'Incomplete', StatusName: 'Incomplete' });
        this.orderStatusList.push({ Id: 100, StatusCode: 101, StatusDisplayName: 'Inprogress', StatusName: 'Inprogress' });
        this.orderStatusList.push({ Id: 100, StatusCode: 102, StatusDisplayName: 'Completed', StatusName: 'Completed' });
        this.orderStatusList.push({ Id: 100, StatusCode: 103, StatusDisplayName: 'Cancelled', StatusName: 'Cancelled' });
        this.orderStatusList.push({ Id: 100, StatusCode: 104, StatusDisplayName: 'Pending', StatusName: 'Pending' });
        this.orderStatusList.push({ Id: 100, StatusCode: 105, StatusDisplayName: 'Rejected', StatusName: 'Rejected' });
        this.orderStatusList.push({ Id: 100, StatusCode: 106, StatusDisplayName: 'Confirmed', StatusName: 'Confirmed' });
        this.orderStatusList.push({ Id: 100, StatusCode: 107, StatusDisplayName: 'Payment Success', StatusName: 'PaymentSuccess' });
        this.orderStatusList.push({ Id: 100, StatusCode: 108, StatusDisplayName: 'Order Picked', StatusName: 'OrderPicked' });
        this.orderStatusList.push({ Id: 100, StatusCode: 109, StatusDisplayName: 'Delivered ', StatusName: 'Delivered ' });
    }
    MerchantMyOrderComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._activatedRoute.params.subscribe(function (params) {
            _this.activeTab = _this._activatedRoute.snapshot.queryParams["activeTab"];
            if (!_this.activeTab)
                _this.activeTab = "pending";
            _this.fetchUserData();
        });
    };
    MerchantMyOrderComponent.prototype.fetchUserData = function () {
        var _this = this;
        this.spinner.show();
        this._apiService.getAll(this._apiEndPointGetMerchants, this.requestQuery, { activeTab: this.activeTab }).subscribe(function (response) {
            _this.spinner.hide();
            if (response && response.myOrderData) {
                console.log(response);
                _this.myOrderData = response.myOrderData;
                var buttonArray = [];
                var buttonCount = Math.ceil(_this.myOrderData.TotalOrderCount / _this.requestQuery.PageSize);
                for (var i = 1; i <= buttonCount; i++) {
                    buttonArray.push(i);
                }
                var nextPage = _this.requestQuery.CurrentPage < buttonCount ? _this.requestQuery.CurrentPage + 1 : 1;
                var previousPage = _this.requestQuery.CurrentPage > 1 ? _this.requestQuery.CurrentPage - 1 : 1;
                _this.paginationData = {
                    totalPageCount: _this.myOrderData.TotalOrderCount,
                    currentPageSize: _this.requestQuery.PageSize,
                    currentPageNumber: _this.requestQuery.CurrentPage,
                    buttonCountArray: buttonArray,
                    buttonCounts: buttonCount,
                    previousPage: previousPage,
                    nextPage: nextPage
                };
            }
        }, function (error) {
            _this.spinner.hide();
            console.log(error);
        });
    };
    MerchantMyOrderComponent.prototype.onTabChange = function ($event, tabName) {
        $event.preventDefault();
        this.activeTab = tabName;
        this.fetchUserData();
    };
    MerchantMyOrderComponent.prototype.onPageChange = function ($event, requestedPage) {
        if ($event)
            $event.preventDefault();
        if (requestedPage != this.requestQuery.CurrentPage) {
            this.requestQuery.CurrentPage = requestedPage;
            this.fetchUserData();
            if (requestedPage > this.buttonEndTo) {
                this.buttonEndTo += 10;
                this.buttonStartFrom += 10;
            }
            else if (requestedPage < this.buttonStartFrom && this.buttonStartFrom > 10) {
                this.buttonEndTo -= 10;
                this.buttonStartFrom -= 10;
            }
        }
    };
    MerchantMyOrderComponent.prototype.getDate = function (date) {
        if (date) {
            return new Date(Number(date.substring(date.indexOf("(") + 1, date.indexOf(")")))).toUTCString();
        }
        return null;
    };
    MerchantMyOrderComponent.prototype.trackByFnOrders = function (index, order) {
        return order.OrderNumber;
    };
    MerchantMyOrderComponent.prototype.onScroll = function () {
        this.requestQuery.PageSize += 10;
        this.fetchUserData();
    };
    MerchantMyOrderComponent.prototype.orderStatusChangeHandler = function ($event, orderGuid) {
        var _this = this;
        var selectedValue = $event.target.value;
        this._apiService.post(this._apiEndPointUpdateOrderStatus, { orderGuid: orderGuid, orderStatus: selectedValue }).subscribe(function (response) {
            sweetalert2_1.default({
                position: 'top-end',
                type: 'success',
                title: 'Order Status has been Updated.',
                showConfirmButton: false,
                timer: 1500
            }).then(function () {
                _this.fetchUserData();
            });
        }, function (error) {
            sweetalert2_1.default('Cancelled', 'Something going wrong.', 'error');
        });
    };
    MerchantMyOrderComponent = __decorate([
        core_1.Component({
            templateUrl: './merchant-myorder.component.html'
        }),
        __metadata("design:paramtypes", [api_service_service_1.ApiServiceService, ngx_spinner_1.NgxSpinnerService, router_1.ActivatedRoute])
    ], MerchantMyOrderComponent);
    return MerchantMyOrderComponent;
}());
exports.MerchantMyOrderComponent = MerchantMyOrderComponent;
var OrderStatusList = /** @class */ (function () {
    function OrderStatusList() {
    }
    return OrderStatusList;
}());
//# sourceMappingURL=merchant-myorder.component.js.map