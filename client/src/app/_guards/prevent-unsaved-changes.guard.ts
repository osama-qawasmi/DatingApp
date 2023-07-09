import { Injectable } from '@angular/core';
import { CanDeactivate} from '@angular/router';
import { MemberEditComponent } from 'app/members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})

export class preventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  canDeactivate(component: MemberEditComponent): boolean {
    if(component.editForm?.dirty) {
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost')
    }
    return true;
  }
}

//export const preventUnsavedChangesGuard: CanDeactivateFn<unknown> = (component, currentRoute, currentState, nextState) => {
  //return true;
//};
