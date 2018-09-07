export class UserViewModel {
    public id: number;
    public first_name: string;
    public last_name: string;
    public email: string;
    public contact_number: string
    public address: string;
}

export class ResponseViewModel<T> {
    public data: T;
    public count: number;
}
