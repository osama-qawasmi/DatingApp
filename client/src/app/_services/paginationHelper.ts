import { HttpClient, HttpParams } from "@angular/common/http";
import { PaginatedResult } from "app/_models/pagination";
import { map } from "rxjs";

export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginationResult: PaginatedResult<T> = new PaginatedResult<T>;
    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            if (response.body) {
                paginationResult.result = response.body;
            }
            const pagination = response.headers.get('Pagination');
            if (pagination) {
                paginationResult.pagination = JSON.parse(pagination);
            }
            return paginationResult;
        })
    );
}

export function getPaginationHeader(pageNumber: number, pageSize: number) {
    var params = new HttpParams();
    params = params.append("pageNumber", pageNumber);
    params = params.append("pageSize", pageSize);
    return params;
}