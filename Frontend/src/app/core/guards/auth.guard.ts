import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../features/auth/services/auth.service';
import { CanActivateFn } from '@angular/router';
import { map } from 'rxjs/operators';


export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // Signal already set (user navigated within the app)
  if (auth.isLoggedIn()) return true;

  // Signal is null (page was refreshed) — ask the server
  return auth.fetchCurrentUser().pipe(
    map(user => !!user || router.createUrlTree(['/login']))
  );
};