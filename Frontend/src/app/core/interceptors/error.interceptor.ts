import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { ProblemDetails } from "../models/problem-details.model";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    
    const router = inject(Router);
   
    return next(req).pipe(
      catchError((err: HttpErrorResponse) => {
        const errorData = err.error as ProblemDetails;
        
        if (err.status === 401 && req.url.includes('/auth/me')) {
        	return throwError(() => err);
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
            alert('Unauthorized: Please log in to access this resource.');
            router.navigate(['/login']);
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