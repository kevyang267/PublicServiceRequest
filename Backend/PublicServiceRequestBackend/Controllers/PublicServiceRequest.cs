using Microsoft.AspNetCore.Mvc;
using PublicServiceRequestBackend.Services;
using PublicServiceRequestBackend.Models;

namespace PublicServiceRequestBackend.Controllers
{
    [ApiController]
    [Route("api/records")]
    public class PublicServiceRequest : ControllerBase
    {

        private readonly IPublicServiceRequest _service;

        public PublicServiceRequest(IPublicServiceRequest service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRecordDTO>>> GetAllServiceRecords()
        {
            var records = await _service.GetAllServiceRecordsAsync();
            return Ok(records);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRecordDTO>> GetServiceRecordById(int id)
        {
            var record = await _service.GetServiceRecordByIdAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRecordDTO>> CreateServiceRecord(ServiceRecordDTO serviceRecordDto)
        {
            var createdRecord = await _service.CreateServiceRecordAsync(serviceRecordDto);
            return CreatedAtAction(nameof(GetServiceRecordById), new { id = createdRecord.Id }, createdRecord);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateServiceRecord(int id, ServiceRecordDTO serviceRecordDto)
        {
            try
            {
                await _service.UpdateServiceRecordAsync(id, serviceRecordDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRecord(int id)
        {
            var record = await _service.DeleteRecordAsync(id);
            if (record == false)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
