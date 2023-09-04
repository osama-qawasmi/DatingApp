import { Injectable, inject } from '@angular/core';
import { CanDeactivateFn} from '@angular/router';
import { ConfirmService } from 'app/_services/confirm.service';
import { MemberEditComponent } from 'app/members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
    const confirmService = inject(ConfirmService);

    if(component.editForm?.dirty) {
      //return confirm('Are you sure you want to continue? Any unsaved changes will be lost')
      return confirmService.confirm();
    }
    return true;
};

//export const preventUnsavedChangesGuard: CanDeactivateFn<unknown> = (component, currentRoute, currentState, nextState) => {
  //return true;
//};
