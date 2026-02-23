export interface RecordDTO {
  id: number;
  requesterName: string;
  requestType: string;
  description: string;
  status: string;
  createdDate: string;
  updatedDate: string | null;
}

export interface Record {
  id: number;
  requesterName: string;
  requestType: string;
  description: string;
  status: string;
  createdDate: Date;
  updatedDate: Date | null;
}

export function recordFromDTO(dto: RecordDTO): Record {
  return {
    id: dto.id,
    requesterName: dto.requesterName,
    requestType: dto.requestType,

    description: dto.description,
    status: dto.status,
    createdDate: new Date(dto.createdDate),
    updatedDate: dto.updatedDate ? new Date(dto.updatedDate) : null,
  };
}

export function recordToDTO(record: Record): RecordDTO {
  return {
    id: record.id,
    requesterName: record.requesterName,
    requestType: record.requestType,
    description: record.description,
    status: record.status,
    createdDate: record.createdDate.toISOString(),
    updatedDate: record.updatedDate ? record.updatedDate.toISOString() : null,
  };
}

export const REQUEST_TYPES = ['Permit', 'Complaint', 'Information', 'Other'];
export const REQUEST_STATUSES = ['Open', 'In Progress', 'Closed'];
