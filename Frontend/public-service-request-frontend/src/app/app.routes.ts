import { Routes } from '@angular/router';
import { ServiceRequestListComponent } from './components/service-request-list/service-request-list';
import { ServiceRequestDetailComponent } from './components/service-request-detail/service-request-detail';

export const routes: Routes = [
  { path: '', component: ServiceRequestListComponent },
  { path: 'requests/:id', component: ServiceRequestDetailComponent },
  { path: '**', redirectTo: '' },
];
