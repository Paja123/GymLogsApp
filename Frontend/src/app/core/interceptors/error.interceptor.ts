import { HttpErrorResponse, HttpInterceptorFn, HttpClient} from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError , switchMap} from "rxjs";
import { ProblemDetails } from "../models/problem-details.model";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    
    const router = inject(Router);
		const http = inject(HttpClient);

    // return next(req).pipe(
    //   catchError((err: HttpErrorResponse) => {
    //     const errorData = err.error as ProblemDetails;
        
    //     if (err.status === 401 && req.url.includes('/auth/me')) {
    //     	return throwError(() => err);
    //     }
		return next(req).pipe(
      catchError((err: HttpErrorResponse) => {
        const errorData = err.error as ProblemDetails;

        // Never retry these endpoints
        if (req.url.includes('/auth/me') ||
            req.url.includes('/auth/refresh') ||
            req.url.includes('/auth/login') ||
            req.url.includes('/auth/register')) {
          return throwError(() => err);
        }

        if (err.status === 401) {
          return http.post('https://localhost:7247/api/auth/refresh', {}).pipe(
            switchMap(() => next(req)), // retry original request
            catchError(() => {
              // Refresh failed — session expired
              router.navigate(['/login'], {
                queryParams: { reason: 'session-expired' }
              });
              return throwError(() => err);
            })
          );
        }

        switch (err.status) {
        case 400:
					// console.error('Validaton error from backend:', errorData.errors);
          // alert(`Data validation error: ${errorData.detail}`);
					const errors = errorData?.errors;
					if (errors) {
						const messages = Object.values(errors).flat().join('\n');
						alert(`Validation errors:\n${messages}`);
					} else {
						alert(`Error: ${errorData?.detail}`);
					}
					// const errors = errorData?.errors;
					// if (errors) {
					// 	const messages = Object.values(errors).flat().join('\n');
					// 	alert(`Validation errors:\n${messages}`);
					// } else {
					// 	alert(`Error: ${errorData?.detail}`);
					// }
				break;
        case 401:
            // alert('Unauthorized: Please log in to access this resource.');
            // router.navigate(['/login']);
            break;
        case 404:
            break;
        case 409:
            console.warn('Data conflict:', errorData.detail);
            alert(`Conflict: ${errorData.detail}`);
            break;
        case 500:
            console.error('Server error, TraceId:', errorData.traceId);
            break;
        default:
            console.error('Unknown error:', err);
        }
          
        return throwError(() => err);
        })
    );
}