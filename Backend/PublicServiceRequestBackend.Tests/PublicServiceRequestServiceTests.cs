using Microsoft.EntityFrameworkCore;
using PublicServiceRequestBackend.Data;
using PublicServiceRequestBackend.Models;
using PublicServiceRequestBackend.Services;

namespace PublicServiceRequestBackend.Tests.Services
{
    public class PublicServiceRequestServiceTests : IDisposable
    {
        private readonly PublicServiceRequestDbContext _context;
        private readonly PublicServiceRequestService _service;

        public PublicServiceRequestServiceTests()
        {
            var options = new DbContextOptionsBuilder<PublicServiceRequestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PublicServiceRequestDbContext(options);
            _service = new PublicServiceRequestService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // -------------------------
        // CreateServiceRecordAsync
        // -------------------------

        [Fact]
        public async Task CreateServiceRecordAsync_ShouldPersistRecord()
        {
            var dto = new ServiceRecordDTO
            {
                RequesterName = "Jane Doe",
                RequestType = "Permit",
                Description = "Test description"
            };

            var result = await _service.CreateServiceRecordAsync(dto);

            var saved = await _context.ServiceRecords.FindAsync(result.Id);
            Assert.NotNull(saved);
            Assert.Equal("Jane Doe", saved.RequesterName);
        }

        [Fact]
        public async Task CreateServiceRecordAsync_ShouldAlwaysSetStatusToOpen()
        {
            var dto = new ServiceRecordDTO
            {
                RequesterName = "Jane Doe",
                RequestType = "Complaint",
                Status = "Closed" // should be overridden
            };

            var result = await _service.CreateServiceRecordAsync(dto);

            Assert.Equal("Open", result.Status);
        }

        [Fact]
        public async Task CreateServiceRecordAsync_ShouldSetCreatedAndUpdatedDates()
        {
            var before = DateTime.UtcNow;

            var result = await _service.CreateServiceRecordAsync(new ServiceRecordDTO
            {
                RequesterName = "Jane Doe",
                RequestType = "Information"
            });

            var after = DateTime.UtcNow;

            Assert.InRange(result.CreatedDate, before, after);
            Assert.InRange(result.UpdatedDate, before, after);
        }

        // -------------------------
        // GetAllServiceRecordsAsync
        // -------------------------

        [Fact]
        public async Task GetAllServiceRecordsAsync_ShouldReturnAllRecords()
        {
            _context.ServiceRecords.AddRange(
                new ServiceRecordEntity { RequesterName = "Alice", RequestType = "Permit", Status = "Open" },
                new ServiceRecordEntity { RequesterName = "Bob", RequestType = "Complaint", Status = "Closed" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllServiceRecordsAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllServiceRecordsAsync_ShouldReturnEmptyList_WhenNoRecords()
        {
            var result = await _service.GetAllServiceRecordsAsync();

            Assert.Empty(result);
        }

        // -------------------------
        // GetServiceRecordByIdAsync
        // -------------------------

        [Fact]
        public async Task GetServiceRecordByIdAsync_ShouldReturnCorrectRecord()
        {
            var entity = new ServiceRecordEntity
            {
                RequesterName = "Alice",
                RequestType = "Permit",
                Status = "Open"
            };
            _context.ServiceRecords.Add(entity);
            await _context.SaveChangesAsync();

            var result = await _service.GetServiceRecordByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.RequesterName);
        }

        [Fact]
        public async Task GetServiceRecordByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.GetServiceRecordByIdAsync(999);

            Assert.Null(result);
        }

        // -------------------------
        // UpdateServiceRecordAsync
        // -------------------------

        [Fact]
        public async Task UpdateServiceRecordAsync_ShouldUpdateStatusAndName()
        {
            var entity = new ServiceRecordEntity
            {
                RequesterName = "Alice",
                RequestType = "Permit",
                Status = "Open"
            };
            _context.ServiceRecords.Add(entity);
            await _context.SaveChangesAsync();

            await _service.UpdateServiceRecordAsync(entity.Id, new ServiceRecordDTO
            {
                RequesterName = "Alice Updated",
                Status = "In Progress"
            });

            var updated = await _context.ServiceRecords.FindAsync(entity.Id);
            Assert.Equal("Alice Updated", updated!.RequesterName);
            Assert.Equal("In Progress", updated.Status);
        }

        [Fact]
        public async Task UpdateServiceRecordAsync_ShouldThrow_WhenRecordNotFound()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateServiceRecordAsync(999, new ServiceRecordDTO
                {
                    RequesterName = "Ghost",
                    Status = "Closed"
                })
            );
        }

        // -------------------------
        // DeleteRecordAsync
        // -------------------------

        [Fact]
        public async Task DeleteRecordAsync_ShouldRemoveRecord_AndReturnTrue()
        {
            var entity = new ServiceRecordEntity
            {
                RequesterName = "Alice",
                RequestType = "Permit",
                Status = "Open"
            };
            _context.ServiceRecords.Add(entity);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteRecordAsync(entity.Id);

            Assert.True(result);
            Assert.Null(await _context.ServiceRecords.FindAsync(entity.Id));
        }

        [Fact]
        public async Task DeleteRecordAsync_ShouldReturnFalse_WhenRecordNotFound()
        {
            var result = await _service.DeleteRecordAsync(999);

            Assert.False(result);
        }
    }
}