import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from 'app/_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const Toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map(user => {
      if(!user) return false;
      if(user.roles.includes('Admin') || user.roles.includes('Moderator')) {
        return true;
      }
      else {
        Toastr.error('You cannot access this area');
        return false;
      }
    })
  )
  return true;
};
