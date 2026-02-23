using PublicServiceRequestBackend.Models;
using PublicServiceRequestBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace PublicServiceRequestBackend.Services
{
    public class PublicServiceRequestService : IPublicServiceRequest
    {

        private readonly PublicServiceRequestDbContext _context;

        public PublicServiceRequestService(PublicServiceRequestDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRecordDTO> CreateServiceRecordAsync(ServiceRecordDTO serviceRecordDto)
        {
            var entity = MapToEntity(serviceRecordDto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.Status = "Open";
            _context.ServiceRecords.Add(entity);
            await _context.SaveChangesAsync(); // This was missing
            return MapToDTO(entity);
        }

        public async Task<List<ServiceRecordDTO>> GetAllServiceRecordsAsync()
        {
            var entities = await _context.ServiceRecords.ToListAsync();
            return entities.Select(MapToDTO).ToList();  
        }

        public async Task<ServiceRecordDTO?> GetServiceRecordByIdAsync(int id)
        {
            var entity = await _context.ServiceRecords.FindAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task UpdateServiceRecordAsync(int id, ServiceRecordDTO serviceRecordDto)
        {
            var existingEntity = _context.ServiceRecords.Find(id);
            
            if (existingEntity == null)
            {
                throw new Exception ("Service record not found.");
            }

            existingEntity.RequesterName = serviceRecordDto.RequesterName;
            existingEntity.Status = serviceRecordDto.Status;
            existingEntity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteRecordAsync(int id)
        {
            var entity = await _context.ServiceRecords.FindAsync(id);
            if (entity == null) return false;
            _context.ServiceRecords.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private ServiceRecordDTO MapToDTO(ServiceRecordEntity entity) => new()
        {
            Id = entity.Id,
            RequesterName = entity.RequesterName,
            RequestType = entity.RequestType,
            Description = entity.Description,
            Status = entity.Status,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
        };

        private ServiceRecordEntity MapToEntity(ServiceRecordDTO dto) => new()
        {
            Id = dto.Id,
            RequesterName = dto.RequesterName,
            RequestType = dto.RequestType,
            Description = dto.Description,
            Status = dto.Status,
            CreatedDate = dto.CreatedDate,
            UpdatedDate = dto.UpdatedDate,
        };
    }
}