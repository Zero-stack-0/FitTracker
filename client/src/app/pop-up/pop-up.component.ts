import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-popup',
  templateUrl: './pop-up.component.html',
  styleUrls: ['./pop-up.component.css']
})
export class PopupComponent {
  @Input() isOpen: boolean = false;
  @Input() isGreen: boolean = true;
  @Input() description: string = "";
  @Input() title: string = ""; // Default title if not provided
  @Output() close = new EventEmitter<void>();

  onClose() {
    this.close.emit();
  }
}