using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Controllers;
using FinanceTrackerAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FinanceTrackerAPI.Tests;

public class userTests
{
    [Fact]
    public void GetUsers_Valid_ReturnUsers()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new UserController(context);
        var user = new User
        {
            Name = "Javies",
            Email = "javies.jay@gmail.com",
            Password = "jayjay33"
        };
        context.Users.Add(user);
        var user2 = new User
        {
            Name = "Andrea",
            Email = "andrea.and@gmail.com",
            Password = "jayjay33"
        };
        context.Users.Add(user2);
        context.SaveChanges();
        var result = controller.GetUsers();
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(2, context.Users.Count());
    }

    [Fact]
    public void AddUser_Valid_SavesToDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var user = new User
        {
            Name = "Javies",
            Email = "javies.jay@gmail.com",
            Password = "jayjay33"
        };
        context.Users.Add(user);
        context.SaveChanges();
        Assert.Equal(1, context.Users.Count());
    }

    [Fact]
    public void AddUser_duplicate_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new UserController(context);
        var user = new CreateUserDTO
        {
            Name = "Javies",
            Email = "javies.jay@gmail.com",
            Password = "jayjay33"
        };

        var user2 = new CreateUserDTO
        {
            Name = "Andrea",
            Email = "javies.jay@gmail.com",
            Password = "jayjay33"
        };
        var result = controller.AddUser(user);
        Assert.Equal(1, context.Users.Count());
        Assert.Equal("Javies", context.Users.First().Name);
    }

    [Fact]
    public void AddUser_NoName_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new UserController(context);
        var user = new CreateUserDTO
        {
            Name = null,
            Email = "javies.jay@gmail.com",
            Password = "jayjay33"
        };
        var result = controller.AddUser(user);
        Assert.IsType<BadRequestResult>(result);
        Assert.Equal(0, context.Users.Count());
    }

    [Fact]
    public void AddUser_NoEmail_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new UserController(context);
        var user = new CreateUserDTO
        {
            Name = "Javies",
            Email = null,
            Password = "jayjay33"
        };
        var result = controller.AddUser(user);
        Assert.IsType<BadRequestResult>(result);
        Assert.Equal(0, context.Users.Count());
    }

    [Fact]
    public void AddUser_NoPassword_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new UserController(context);
        var user = new CreateUserDTO
        {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = null
        };
        var result = controller.AddUser(user);
        Assert.IsType<BadRequestResult>(result);
        Assert.Equal(0, context.Users.Count());
    }

    [Fact]
    public void DeleteUser_ValidId_ReturnUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new User
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jjaavv33"
            };
            context.Users.Add(user);
            context.SaveChanges();
            var result = controller.DeleteUser(1);
            Assert.Equal(0, context.Users.Count());
    }

    [Fact]
    public void DeleteUser_InvalidId_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new CreateUserDTO
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jjaavv33"
            };
            var result1 = controller.AddUser(user);
            var result = controller.DeleteUser(3);
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(1, context.Users.Count());
    }
    [Fact]
    public void GetById_ValidId_ReturnUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new User
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jjaavv33"
            };
            context.Users.Add(user);
            context.SaveChanges();
            var result = controller.GetById(1);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, context.Users.Count());
    }

    [Fact]
    public void GetById_InvalidId_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new User
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jjaavv33"
            };
            context.Users.Add(user);
            context.SaveChanges();
            var result = controller.GetById(3);
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(1, context.Users.Count());
    }

    [Fact]
    public void EditUser_ValidData_UpdatesDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new User
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jayjay78"
            };
            context.Users.Add(user);
            var dto = new EditUserDTO
            {
            Name = "Javies",
            Email = "jayjavies@gmail.com",
            Password = "jayjay78"
            };
            context.SaveChanges();
            var result = controller.EditUser(1, dto);
            Assert.Equal("jayjavies@gmail.com", context.Users.First().Email);
    }

    [Fact]
    public void EditUser_InvalidId_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new UserController(context);
            var user = new User
            {
            Name = "Javies",
            Email = "javiesjay@gmail.com",
            Password = "jayjay78"
            };
            context.Users.Add(user);
            var dto = new EditUserDTO
            {
            Name = "Javies",
            Email = "jayjavies@gmail.com",
            Password = "jayjay78"
            };
            context.SaveChanges();
            var result = controller.EditUser(3, dto);
            Assert.Equal("javiesjay@gmail.com", context.Users.First().Email);
    }
}