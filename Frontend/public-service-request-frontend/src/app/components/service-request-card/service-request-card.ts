import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Record } from '../../models/records';
import { RecordService } from '../../services/record.service';

@Component({
  selector: 'app-service-request-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './service-request-card.html',
})
export class ServiceRequestCardComponent {
  @Input() record!: Record;
  @Output() deleted = new EventEmitter<number>();

  isDeleting = false;

  constructor(private recordService: RecordService) {}

  get statusClasses(): string {
    switch (this.record.status) {
      case 'Open':
        return 'bg-green-100 text-green-700';
      case 'In Progress':
        return 'bg-yellow-100 text-yellow-700';
      case 'Closed':
        return 'bg-gray-100 text-gray-500';
      default:
        return 'bg-gray-100 text-gray-500';
    }
  }

  onDelete(): void {
    if (!confirm(`Delete request #${this.record.id}? This cannot be undone.`)) return;
    this.isDeleting = true;
    this.recordService.deleteRecord(this.record.id).subscribe({
      next: () => this.deleted.emit(this.record.id),
      error: (err) => {
        console.error(err);
        this.isDeleting = false;
      },
    });
  }
}
