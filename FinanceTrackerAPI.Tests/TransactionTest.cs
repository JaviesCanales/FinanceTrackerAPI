using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;


namespace FinanceTrackerAPI.Tests;

public class UnitTest1
{
    [Fact]
    public void AddTransaction_ValidTransaction_SavesToDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        context.SaveChanges();

        Assert.Equal(1, context.Transactions.Count());
    }
    [Fact]
    public void AddTransaction_InvalidTransaction_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new TransactionController(context);

        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "gift"
        };
        var result = controller.AddTransaction(transaction);
        Assert.IsType<BadRequestResult>(result);
        Assert.Equal(0, context.Transactions.Count());
    }

    [Fact]
    public void AddTransaction_NegativeTransaction_Returns400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new TransactionController(context);

        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 0,
            Category = "Entertainment",
            Type = "expense"
        };
        var result = controller.AddTransaction(transaction);
        Assert.IsType<BadRequestResult>(result);
        Assert.Equal(0, context.Transactions.Count());
    }

    [Fact]
    public void DeleteTransaction_ValidTransaction_RemovesFromDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        var controller = new TransactionController(context);

        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        context.SaveChanges();
        var result = controller.DeleteTransaction(1);

        

        Assert.Equal(0, context.Transactions.Count());

    }
    [Fact]
    public void DeleteTransaction_InvalidTransaction_Return400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            var context = new AppDbContext(options);
            var controller = new TransactionController(context);
            var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        var result = controller.DeleteTransaction(3);
        context.SaveChanges();
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(1, context.Transactions.Count());
    }


    [Fact]
    public void GetId_ValidId_ReturnsTransaction()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var context = new AppDbContext(options);
            var controller = new TransactionController(context);
            var transaction = new Transaction
            {
                Description = "Geoguessr",
                Amount = 41.28m,
                Category = "Entertainment",
                Type = "expense"
            };
            context.Transactions.Add(transaction);
            context.SaveChanges();
            var result = controller.GetId(1);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, context.Transactions.Count());
    }

    [Fact]
    public void GetId_InvalidId_Returns404()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        var controller = new TransactionController(context);
        var transaction = new Transaction
            {
                Description = "Geoguessr",
                Amount = 41.28m,
                Category = "Entertainment",
                Type = "expense"
            };
            context.Transactions.Add(transaction);
            context.SaveChanges();
            var result = controller.GetId(4);
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(1, context.Transactions.Count());
    }

    [Fact]
    public void EditTransaction_ValidData_UpdatesTransaction()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new TransactionController(context);
        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        context.SaveChanges();
        var updatedTransaction = new Transaction
        {
            Description = "Walmart",
            Amount = 41.28m,
            Category = "Grocery",
            Type = "expense"
        };
        var result = controller.EditTransaction(1, updatedTransaction);
        context.SaveChanges();

        Assert.Equal("Walmart", context.Transactions.First().Description);
    }

    [Fact]
    public void EditTransaction_InvalidData_Return400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new TransactionController(context);
        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        context.SaveChanges();
        var updatedTransaction = new Transaction
        {
            Description = "Walmart",
            Amount = 41.28m,
            Category = "Grocery",
            Type = "fun"
        };
        var result = controller.EditTransaction(1, updatedTransaction);
        context.SaveChanges();

        Assert.Equal("Geoguessr", context.Transactions.First().Description);
    }

    [Fact]
    public void EditTransaction_InvalidTransaction_Return400()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var controller = new TransactionController(context);
        var transaction = new Transaction
        {
            Description = "Geoguessr",
            Amount = 41.28m,
            Category = "Entertainment",
            Type = "expense"
        };
        context.Transactions.Add(transaction);
        context.SaveChanges();
        var updatedTransaction = new Transaction
        {
            Description = "Walmart",
            Amount = 41.28m,
            Category = "Grocery",
            Type = "expense"
        };
        var result = controller.EditTransaction(2, updatedTransaction);
        context.SaveChanges();
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal("Geoguessr", context.Transactions.First().Description);
    }
}
