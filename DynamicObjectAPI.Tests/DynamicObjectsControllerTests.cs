using Moq;
using Xunit;
using DynamicObjectAPI.Api.Controllers;
using DynamicObjectAPI.Service.Services.Abstract;
using DynamicObjectAPI.Core.Models;
using DynamicObjectAPI.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

public class DynamicObjectsControllerTests
{
    private readonly Mock<IDynamicObjectService> _mockService;
    private readonly IMapper _mapper;
    private readonly DynamicObjectsController _controller;

    public DynamicObjectsControllerTests()
    {
        _mockService = new Mock<IDynamicObjectService>();
        var config = new MapperConfiguration(cfg => cfg.CreateMap<DynamicObject, DynamicObjectDto>().ReverseMap());
        _mapper = config.CreateMapper();
        _controller = new DynamicObjectsController(_mockService.Object, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithDynamicObjectList()
    {
        var dynamicObjects = new List<DynamicObject>
        {
            new DynamicObject { Id = 1, ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } },
            new DynamicObject { Id = 2, ObjectType = "Type2", Fields = new Dictionary<string, object> { { "key2", "value2" } } }
        };

        _mockService.Setup(service => service.GetAllObjectsAsync()).ReturnsAsync(dynamicObjects);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<DynamicObjectDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenObjectExists()
    {
        var dynamicObject = new DynamicObject { Id = 1, ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync(dynamicObject);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<DynamicObjectDto>(okResult.Value);
        Assert.Equal("Type1", returnValue.ObjectType);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenObjectDoesNotExist()
    {
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync((DynamicObject)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenObjectTypeIsNull()
    {
        var dynamicObjectDto = new DynamicObjectDto
        {
            ObjectType = null,
            Fields = new Dictionary<string, object> { { "key1", "value1" } }
        };

        var result = await _controller.Create(dynamicObjectDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenFieldsAreEmpty()
    {
        var dynamicObjectDto = new DynamicObjectDto
        {
            ObjectType = "Type1",
            Fields = new Dictionary<string, object>()
        };

        var result = await _controller.Create(dynamicObjectDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenObjectIsValid()
    {
        var dynamicObjectDto = new DynamicObjectDto { ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };

        _mockService.Setup(service => service.CreateObjectAsync(It.IsAny<DynamicObject>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(dynamicObjectDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenObjectIsUpdated()
    {
        var dynamicObject = new DynamicObject { Id = 1, ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync(dynamicObject);
        _mockService.Setup(service => service.UpdateObjectAsync(It.IsAny<DynamicObject>())).Returns(Task.CompletedTask);

        var dynamicObjectDto = new DynamicObjectDto { ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };

        var result = await _controller.Update(1, dynamicObjectDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenObjectDoesNotExist()
    {
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync((DynamicObject)null);

        var dynamicObjectDto = new DynamicObjectDto { ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };

        var result = await _controller.Update(1, dynamicObjectDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenFieldsAreInvalid()
    {
        
        var dynamicObjectDto = new DynamicObjectDto
        {
            ObjectType = "TestType",
            Fields = new Dictionary<string, object>()
        }; 

        _mockService.Setup(s => s.GetObjectByIdAsync(It.IsAny<int>())).ReturnsAsync(new DynamicObject());

        var result = await _controller.Update(1, dynamicObjectDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }


    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenObjectIsDeleted()
    {
        var dynamicObject = new DynamicObject { Id = 1, ObjectType = "Type1", Fields = new Dictionary<string, object> { { "key1", "value1" } } };
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync(dynamicObject);
        _mockService.Setup(service => service.DeleteObjectAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenObjectDoesNotExist()
    {
        _mockService.Setup(service => service.GetObjectByIdAsync(1)).ReturnsAsync((DynamicObject)null);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldHandleExceptionsGracefully()
    {
        _mockService.Setup(s => s.GetObjectByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Error occurred"));

        var result = await _controller.Delete(1);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Error occurred: Error occurred", objectResult.Value);
    }




}
