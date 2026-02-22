using PublicServiceRequestBackend.Models;

namespace PublicServiceRequestBackend.Services
{
    public interface IPublicServiceRequest
    {
        public Task<List<ServiceRecordDTO>> GetAllServiceRecordsAsync();
        public Task<ServiceRecordDTO?> GetServiceRecordByIdAsync(int id);
        public Task<ServiceRecordDTO> CreateServiceRecordAsync(ServiceRecordDTO serviceRecordDto);
        public Task UpdateServiceRecordAsync(int id, ServiceRecordDTO serviceRecordDto);
        public Task<bool> DeleteRecordAsync(int id);
    }
}