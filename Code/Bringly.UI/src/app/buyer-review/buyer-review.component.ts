import { Component, OnInit } from '@angular/core';
import { ApiServiceService } from '../services/shared-service/api-service.service';
import { AppSettings } from '../utility/app-setting';

@Component({
    templateUrl: "./buyer-review.component.html"
})

export class BuyerReviewComponent implements OnInit {
    private _apiEndPoint: string;
    public myreviewData: any;
    public isMerchant: boolean = false;
    constructor(private _apiService: ApiServiceService) {
        this._apiEndPoint = `${AppSettings.AppUrl}/orders/getmerchantmyorders`;
    }

    ngOnInit() {
        this.fetchUserData();
    }

    private fetchUserData(): void {
        this._apiService.getAll<any>(this._apiEndPoint).subscribe((response) => {
            if (response && response.reviewData) {
                this.myreviewData = response.reviewData;
                this.isMerchant = response.isMerchant;
            }
        }, (error) => {
            console.log(error);
        });
    }

    public getDate(date: string): string {
        if (date) {
            return new Date(Number(date.substring(date.indexOf("(") + 1, date.indexOf(")")))).toUTCString();
        }
        return null;
    }
     
}

