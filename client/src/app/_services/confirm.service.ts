import { Injectable } from '@angular/core';
import { ConfirmDialogComponent } from 'app/_modals/confirm-dialog/confirm-dialog.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef?: BsModalRef<ConfirmDialogComponent>;

  constructor(private modelService: BsModalService) { }

  confirm(
    title = 'Confirmation',
    message = 'Are you sure you want to leave and lose data unsaved?',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'
  ): Observable<boolean> {
    const config = {
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText
      }
    }
    this.bsModelRef = this.modelService.show(ConfirmDialogComponent, config);
    return this.bsModelRef.onHidden!.pipe(
      map(() => {
        return this.bsModelRef!.content!.result
      })
    )
  }
}
