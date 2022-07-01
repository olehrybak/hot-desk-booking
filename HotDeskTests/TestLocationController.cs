using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotDeskAPI.Controllers;
using HotDeskAPI.Data;
using HotDeskAPI.Entities;
using HotDeskAPI.Models;
using HotDeskAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HotDeskTests;

public class FakeDbSet<T> : DbSet<T> where T : class
{
    public override IEntityType EntityType { get; }
}

[TestClass]
public class TestLocationController
{
    [TestMethod]
    public async Task GetAllLocations()
    {
        var controller = CreateLocationControllerInstance();

        var actionResult = await controller.Get();
        var result = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(result);
        var valueResult = result.Value as List<LocationDto>;
        Assert.IsNotNull(valueResult);
        Assert.AreEqual(GetLocationsList().Count, valueResult.Count);
    }

    [TestMethod]
    public async Task DeleteByExistingId()
    {
        var controller = CreateLocationControllerInstance();

        var actionResult = await controller.Delete(1);
        Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
        var result = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(result);
        var valueResult = result.Value as LocationDto;
        Assert.IsNotNull(valueResult);
        var expectedResult = new LocationDto()
        {
            LocationId = 1,
            Name = "Location1"
        };
        Assert.AreEqual(expectedResult.LocationId, valueResult.LocationId);
        Assert.AreEqual(expectedResult.Name, valueResult.Name);
    }
    
    [TestMethod]
    public async Task DeleteByNonExistingId()
    {
        var controller = CreateLocationControllerInstance();

        var actionResult = await controller.Delete(10);
        Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public async Task DeleteIfContainsDesks()
    {
        var controller = CreateLocationControllerInstance();

        var actionResult = await controller.Delete(0);
        Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
    }

    private LocationController CreateLocationControllerInstance()
    {
        var locations = GetLocationsList().AsQueryable();
        
        var desks = new List<Desk>
        {
            new Desk()
            {
                LocationId = 0,
                DeskId = 0,
                IsAvailable = true
            },
            new Desk()
            {
                LocationId = 0,
                DeskId = 1,
                IsAvailable = true
            }
        }.AsQueryable();
        
        var mockSetLocations = new Mock<DbSet<Location>>();
        mockSetLocations.As<IQueryable<Location>>().Setup(m => m.Provider).Returns(locations.Provider);
        mockSetLocations.As<IQueryable<Location>>().Setup(m => m.Expression).Returns(locations.Expression);
        mockSetLocations.As<IQueryable<Location>>().Setup(m => m.ElementType).Returns(locations.ElementType);
        mockSetLocations.As<IQueryable<Location>>().Setup(m => m.GetEnumerator()).Returns(locations.GetEnumerator());
        
        var mockSetDesks = new Mock<DbSet<Desk>>();
        mockSetDesks.As<IQueryable<Desk>>().Setup(m => m.Provider).Returns(desks.Provider);
        mockSetDesks.As<IQueryable<Desk>>().Setup(m => m.Expression).Returns(desks.Expression);
        mockSetDesks.As<IQueryable<Desk>>().Setup(m => m.ElementType).Returns(desks.ElementType);
        mockSetDesks.As<IQueryable<Desk>>().Setup(m => m.GetEnumerator()).Returns(desks.GetEnumerator());

        var mockContext = new Mock<DataContext>();
        mockContext.Setup(c => c.Locations).Returns(mockSetLocations.Object);
        mockContext.Setup(c => c.Desks).Returns(mockSetDesks.Object);
        mockContext.Setup(c => c.Set<Location>()).Returns(mockSetLocations.Object);

        var service = new LocationService(mockContext.Object);
        var controller = new LocationController(service);

        return controller;
    }

    private List<Location> GetLocationsList()
    {
        return new List<Location>
        {
            new Location()
            {
                LocationId = 0,
                Name = "Location0"
            },
            new Location()
            {
                LocationId = 1,
                Name = "Location1"
            },
            new Location()
            {
                LocationId = 2,
                Name = "Location2"
            },
            new Location()
            {
                LocationId = 3,
                Name = "Location3"
            }
        };
    }
    
}