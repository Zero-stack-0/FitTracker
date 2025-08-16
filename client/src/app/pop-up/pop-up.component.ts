import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-popup',
  templateUrl: './pop-up.component.html',
  styleUrls: ['./pop-up.component.css']
})
export class PopupComponent {
  @Input() isOpen: boolean = false;
  @Input() isGreen: boolean = false;
  @Input() description: string = "";
  @Input() title: string = "";
  @Output() close = new EventEmitter<void>();

  onClose() {
    this.close.emit();
  }
}