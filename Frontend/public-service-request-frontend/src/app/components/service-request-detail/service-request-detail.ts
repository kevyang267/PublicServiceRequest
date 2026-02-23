import { Component, inject, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Record, REQUEST_STATUSES } from '../../models/records';
import { RecordService } from '../../services/record.service';

@Component({
  selector: 'app-service-request-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './service-request-detail.html',
})
export class ServiceRequestDetailComponent {
  record: Record | null = null;
  isLoading = false;
  isSaving = false;
  isDeleting = false;
  errorMessage = '';
  successMessage = '';

  selectedStatus = '';
  readonly statuses = REQUEST_STATUSES;

  private cdr = inject(ChangeDetectorRef);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private recordService: RecordService,
  ) {
    afterNextRender(() => {
      const id = Number(this.route.snapshot.paramMap.get('id'));
      if (!id) {
        this.errorMessage = 'Invalid request ID.';
        this.cdr.detectChanges();
        return;
      }
      this.loadRecord(id);
    });
  }

  loadRecord(id: number): void {
    this.isLoading = true;
    this.cdr.detectChanges();
    this.recordService.getRecordById(id).subscribe({
      next: (record) => {
        this.record = record;
        this.selectedStatus = record.status;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  get statusChanged(): boolean {
    return !!this.record && this.selectedStatus !== this.record.status;
  }

  onUpdateStatus(): void {
    if (!this.record || !this.statusChanged) return;
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';
    const updated: Record = {
      ...this.record,
      status: this.selectedStatus,
      updatedDate: new Date(),
    };
    this.recordService.updateRecord(this.record.id, updated).subscribe({
      next: () => {
        this.record = updated;
        this.successMessage = 'Status updated successfully.';
        this.isSaving = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isSaving = false;
        this.cdr.detectChanges();
      },
    });
  }

  onDelete(): void {
    if (!this.record) return;
    if (!confirm(`Delete request #${this.record.id}? This cannot be undone.`)) return;
    this.isDeleting = true;
    this.recordService.deleteRecord(this.record.id).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => {
        this.errorMessage = err.message;
        this.isDeleting = false;
        this.cdr.detectChanges();
      },
    });
  }
}
