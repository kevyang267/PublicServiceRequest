import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Record, REQUEST_TYPES } from '../../models/records';
import { RecordService } from '../../services/record.service';

@Component({
  selector: 'app-create-request-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-request-form.html',
})
export class CreateRequestFormComponent {
  @Output() created = new EventEmitter<Record>();
  @Output() cancelled = new EventEmitter<void>();

  readonly requestTypes = REQUEST_TYPES;

  form = {
    requesterName: '',
    requestType: '',
    description: '',
  };

  isSubmitting = false;
  errorMessage = '';

  constructor(private recordService: RecordService) {}

  onSubmit(): void {
    if (!this.form.requesterName || !this.form.requestType) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    const newRecord: Record = {
      id: 0,
      requesterName: this.form.requesterName.trim(),
      requestType: this.form.requestType,
      description: this.form.description.trim(),
      status: 'Open',
      createdDate: new Date(),
      updatedDate: new Date(),
    };

    this.recordService.createRecord(newRecord).subscribe({
      next: (created) => {
        this.created.emit(created);
        this.isSubmitting = false;
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isSubmitting = false;
      },
    });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
