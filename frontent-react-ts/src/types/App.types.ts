export interface IResponseDto{
    isSucceed?:boolean,
    statusCode?:number,
    message?:string,
    result?:object,
    error?:string}

    export interface ISelectDropDownDto{
        id:string|number
        name:string
    }