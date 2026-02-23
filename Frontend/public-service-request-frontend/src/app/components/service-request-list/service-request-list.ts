import { Component, inject, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Record, REQUEST_STATUSES, REQUEST_TYPES } from '../../models/records';
import { RecordService } from '../../services/record.service';
import { ServiceRequestCardComponent } from '../service-request-card/service-request-card';
import { CreateRequestFormComponent } from '../create-request-form/create-request-form';

@Component({
  selector: 'app-service-request-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ServiceRequestCardComponent, CreateRequestFormComponent],
  templateUrl: './service-request-list.html',
})
export class ServiceRequestListComponent {
  records: Record[] = [];
  filteredRecords: Record[] = [];
  isLoading = false;
  errorMessage = '';
  showCreateForm = false;

  filterStatus = '';
  filterType = '';

  readonly statuses = REQUEST_STATUSES;
  readonly requestTypes = REQUEST_TYPES;

  private cdr = inject(ChangeDetectorRef);

  constructor(private recordService: RecordService) {
    afterNextRender(() => {
      this.loadRecords();
    });
  }

  loadRecords(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.detectChanges();
    this.recordService.getAllRecords().subscribe({
      next: (records) => {
        this.records = records;
        this.applyFilters();
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

  applyFilters(): void {
    this.filteredRecords = this.records.filter((r) => {
      const matchesStatus = this.filterStatus ? r.status === this.filterStatus : true;
      const matchesType = this.filterType ? r.requestType === this.filterType : true;
      return matchesStatus && matchesType;
    });
  }

  onFilterChange(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.filterStatus = '';
    this.filterType = '';
    this.applyFilters();
  }

  onRecordCreated(record: Record): void {
    this.records.unshift(record);
    this.applyFilters();
    this.showCreateForm = false;
    this.cdr.detectChanges();
  }

  onRecordDeleted(id: number): void {
    this.records = this.records.filter((r) => r.id !== id);
    this.applyFilters();
    this.cdr.detectChanges();
  }
}
