import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

// Define the authGuard function
export const authGuard: CanActivateFn = () => {
  const isLoggedIn = localStorage.getItem('isLoggedIn');

  if (isLoggedIn === 'true') {
    // If the user is logged in, allow navigation to the requested route
    return true;
  } else {
    const router = inject(Router);  // Inject router instance
    router.navigate(['/login']);
    return false;
  }
};
