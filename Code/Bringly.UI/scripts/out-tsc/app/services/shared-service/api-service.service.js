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
var http_1 = require("@angular/common/http");
var ApiServiceService = /** @class */ (function () {
    function ApiServiceService(_httpClient) {
        this._httpClient = _httpClient;
        this._httpHeaders = new http_1.HttpHeaders();
        this._httpHeaders.append("Content-Type", "application/json; charset=utf-8");
    }
    ApiServiceService.prototype.getAll = function (apiEndPoint, requestQuery, queryString) {
        var params = this.getParams(queryString);
        var requestQueryParams = this.getParams(requestQuery);
        if (queryString || requestQuery)
            return this._httpClient.get(apiEndPoint + "?" + requestQueryParams + "&" + params, { headers: this._httpHeaders });
        else
            return this._httpClient.get("" + apiEndPoint, { headers: this._httpHeaders });
    };
    ApiServiceService.prototype.get = function (apiEndPoint, queryString) {
        var params = this.getParams(queryString);
        if (queryString)
            return this._httpClient.get(apiEndPoint + "?" + params, { headers: this._httpHeaders });
        else
            return this._httpClient.get("" + apiEndPoint, { headers: this._httpHeaders });
    };
    ApiServiceService.prototype.post = function (apiEndPoint, data, queryString) {
        var params = this.getParams(queryString);
        if (queryString)
            return this._httpClient.post(apiEndPoint + "?" + params, data, { headers: this._httpHeaders });
        else
            return this._httpClient.post("" + apiEndPoint, data, { headers: this._httpHeaders });
    };
    ApiServiceService.prototype.put = function (apiEndPoint, data, queryString) {
        var params = this.getParams(queryString);
        if (queryString)
            return this._httpClient.put(apiEndPoint + "/?" + params, data, { headers: this._httpHeaders });
        else
            return this._httpClient.put("" + apiEndPoint, data, { headers: this._httpHeaders });
    };
    ApiServiceService.prototype.delete = function (apiEndPoint, queryString) {
        var params = this.getParams(queryString);
        if (queryString)
            return this._httpClient.delete(apiEndPoint + "?" + params, { headers: this._httpHeaders });
        else
            return this._httpClient.delete("" + apiEndPoint, { headers: this._httpHeaders });
    };
    ApiServiceService.prototype.getParams = function (queryString) {
        var params = new URLSearchParams();
        if (typeof queryString === "object") {
            for (var prop in queryString) {
                if (prop)
                    params.set(prop, queryString[prop]);
            }
        }
        return params;
    };
    ApiServiceService.prototype.requestQueryParams = function (queryString) {
        var params = new URLSearchParams();
        if (typeof queryString === "object") {
            for (var prop in queryString) {
                if (prop)
                    params.set("requestQuery." + prop, queryString[prop]);
            }
        }
        return params;
    };
    ApiServiceService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [http_1.HttpClient])
    ], ApiServiceService);
    return ApiServiceService;
}());
exports.ApiServiceService = ApiServiceService;
//# sourceMappingURL=api-service.service.js.map