import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FormBorderoService {
  public onBorderoChanges = new BehaviorSubject(true);

  constructor() { }
}
