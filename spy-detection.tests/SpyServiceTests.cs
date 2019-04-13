using FluentAssertions;
using Moq;
using spy_detection.Api;
using spy_detection.Data;
using spy_detection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace spy_detection.tests
{
    public class SpyServiceTests
    {
        private Mock<ISpyRepository> GetMockRepository()
        {
            var spies = new List<Spy>
            {
                new Spy
                {
                    Name = "James Bond",
                    Code = new[]{ 0, 0, 7 }
                },
                new Spy
                {
                    Name = "Ethan Hunt",
                    Code = new[]{ 3, 1, 4 }
                }

            };

            //// To query our database we need to implement IQueryable 
            //var mockSet = new Mock<DbSet<Spy>>();
            //mockSet.As<IQueryable<Spy>>().Setup(m => m.Provider).Returns(spies.Provider);
            //mockSet.As<IQueryable<Spy>>().Setup(m => m.Expression).Returns(spies.Expression);
            //mockSet.As<IQueryable<Spy>>().Setup(m => m.ElementType).Returns(spies.ElementType);
            //mockSet.As<IQueryable<Spy>>().Setup(m => m.GetEnumerator()).Returns(spies.GetEnumerator());

            //var mockContext = new Mock<ApplicationDbContext>();
            //mockContext.Setup(c => c.Spies).Returns(mockSet.Object);

            var mockRepository = new Mock<ISpyRepository>();
            mockRepository.Setup(r => r.ToListAsync())
                .ReturnsAsync(spies.ToList());

            return mockRepository;
        }

        [Fact]
        public async Task GetAllSpyTest()
        {
            var mockRepository = GetMockRepository();
            var service = new SpyService(mockRepository.Object, new SpyDetector());

            //Execute test
            var result = await service.GetAllAsync();

            // Verify test result
            result.Should().NotBeNull();
            result.Should().Match(r => r.Any());
        }

        //[Fact]
        //public async Task CreateAsyncShouldPassForValidSpyDataTest()
        //{
        //    var spy = new ApiModel.SpyModel
        //    {
        //        Name = "New James Bond",
        //        Code = new[] { 0, 1, 7 }
        //    };
        //    var mockRepository = GetMockRepository();
        //    mockRepository.Setup(r => r.CreateAsync(It.IsAny<ApiModel.SpyModel>()))
        //       .Callback(() => person.FirstName = "B")

        //    var service = new SpyService(mockRepository.Object, new SpyDetector());

        //    //Execute test
        //    var result = await service.CreateAsync(new ApiModel.SpyModel
        //    {
        //        Name = "New James Bond",
        //        Code = new[] { 0, 1, 7 }
        //    });

        //    // Verify test result
        //    result.Should().NotBeNull();
        //    result.Name.Should().Be("New James Bond");
        //    result.Should().Match(c => result.IsCodeEqual(new[] { 0, 1, 7 }));
        //}

        [Fact]
        public async Task CreateAsyncShouldFailForDuplicateSpyDataTest()
        {
            var mockRepository = GetMockRepository();
            var service = new SpyService(mockRepository.Object, new SpyDetector());

            //Execute test
            var result = await Record.ExceptionAsync(async () => await service.CreateAsync(new ApiModel.SpyModel
            {
                Name = "James Bond",
                Code = new[] { 1 }
            }));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentException>();

            result = await Record.ExceptionAsync(async () => await service.CreateAsync(new ApiModel.SpyModel
            {
                Name = "New Bond",
                Code = new[] { 0, 0, 7 }
            }));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public async Task CreateAsyncShouldFailForInvalidSpyDataTest()
        {
            var mockRepository = GetMockRepository();
            var service = new SpyService(mockRepository.Object, new SpyDetector());

            //Execute test
            var result = await Record.ExceptionAsync(async () => await service.CreateAsync(null));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentNullException>();

            //Execute test
            result = await Record.ExceptionAsync(async () => await service.CreateAsync(new ApiModel.SpyModel
            {
                Name = null,
                Code = new[] { 1 }
            }));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentNullException>();

            //Execute test
            result = await Record.ExceptionAsync(async () => await service.CreateAsync(new ApiModel.SpyModel
            {

                Name = "Name",
                Code = null
            }));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentNullException>();

            //Execute test
            result = await Record.ExceptionAsync(async () => await service.CreateAsync(new ApiModel.SpyModel
            {

                Name = "Name",
                Code = new int[0]
            }));

            // Verify test result
            result.Should().NotBeNull();
            result.Should().BeOfType<ArgumentNullException>();
        }
    }
}
