import { Component, OnInit } from '@angular/core';
import { ApiServiceService } from '../services/shared-service/api-service.service';
import { debug } from 'util';
import { RequestQuery } from '../models/request-query-model';
import { NgxSpinnerService } from 'ngx-spinner';
import swal from 'sweetalert2';
import { AppSettings } from '../utility/app-setting';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './merchant-myorder.component.html'
})
export class MerchantMyOrderComponent implements OnInit {
    private _apiEndPointGetMerchants: string;
    private _apiEndPointUpdateOrderStatus: string;
    public myOrderData: any;
    public activeTab: string = "pending";
    public viewTotalOfOrder = 0;
    public buttonStartFrom: number = 1;
    public buttonEndTo: number = 10;
    public orderStatusList: Array<OrderStatusList> = [];
    public paginationData: {
        totalPageCount: number,
        currentPageSize: number,
        currentPageNumber: number,
        buttonCountArray: Array<number>,
        buttonCounts: number,
        previousPage: number,
        nextPage: number
    };

    private requestQuery: RequestQuery;

    constructor(private _apiService: ApiServiceService, private spinner: NgxSpinnerService, private _activatedRoute: ActivatedRoute) {
        this.requestQuery = new RequestQuery();
        this._apiEndPointGetMerchants = `${AppSettings.AppUrl}/orders/getmerchantmyorders`;
        this._apiEndPointUpdateOrderStatus = `${AppSettings.AppUrl}/orders/updateorderstatus`;
        this.requestQuery.CurrentPage = 1;
        this.requestQuery.PageSize = 10;
        this.requestQuery.SearchQuery = '';
        this.requestQuery.ScrollBy = 'pending'
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

    ngOnInit() {
        this._activatedRoute.params.subscribe((params) => {
            this.activeTab = this._activatedRoute.snapshot.queryParams["activeTab"];
            if (!this.activeTab)
                this.activeTab = "pending";
            this.fetchUserData();
        });
    }

    private fetchUserData(): void {
        this.spinner.show();
        this._apiService.getAll<any>(this._apiEndPointGetMerchants, this.requestQuery, { activeTab: this.activeTab }).subscribe((response) => {
            this.spinner.hide();
            if (response && response.myOrderData) {
                console.log(response)
                this.myOrderData = response.myOrderData;
                let buttonArray: Array<number> = [];
                let buttonCount = Math.ceil(this.myOrderData.TotalOrderCount / this.requestQuery.PageSize);

                for (var i = 1; i <= buttonCount; i++) {
                    buttonArray.push(i);
                }
                let nextPage = this.requestQuery.CurrentPage < buttonCount ? this.requestQuery.CurrentPage + 1 : 1
                let previousPage = this.requestQuery.CurrentPage > 1 ? this.requestQuery.CurrentPage - 1 : 1

                this.paginationData = {
                    totalPageCount: this.myOrderData.TotalOrderCount,
                    currentPageSize: this.requestQuery.PageSize,
                    currentPageNumber: this.requestQuery.CurrentPage,
                    buttonCountArray: buttonArray,
                    buttonCounts: buttonCount,
                    previousPage: previousPage,
                    nextPage: nextPage
                }

            }

        }, (error) => {
            this.spinner.hide();
            console.log(error);
        });
    }

    public onTabChange($event: any, tabName: string) {
        $event.preventDefault();
        this.activeTab = tabName;
        this.fetchUserData();
    }

    public onPageChange($event, requestedPage: number): void {
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
    }


    public getDate(date: string): string {
        if (date) {
            return new Date(Number(date.substring(date.indexOf("(") + 1, date.indexOf(")")))).toUTCString();
        }
        return null;
    }

    public trackByFnOrders(index: number, order: any): string {
        return order.OrderNumber;
    }

    public onScroll() {
        this.requestQuery.PageSize += 10;
        this.fetchUserData();
    }

    public orderStatusChangeHandler($event: any, orderGuid: string) {
        let selectedValue = $event.target.value;
        this._apiService.post(this._apiEndPointUpdateOrderStatus, { orderGuid: orderGuid, orderStatus: selectedValue }).subscribe((response) => {
            swal({
                position: 'top-end',
                type: 'success',
                title: 'Order Status has been Updated.',
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                this.fetchUserData();
            });
        }, (error) => {
            swal(
                'Cancelled',
                'Something going wrong.',
                'error'
            )
        });

    }
}

class OrderStatusList {
    public Id: number;
    public StatusName: string;
    public StatusDisplayName: string;
    public StatusCode: number;
}