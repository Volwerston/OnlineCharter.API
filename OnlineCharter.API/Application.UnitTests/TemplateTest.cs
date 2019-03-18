using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSource.Entities;
using DataSource.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Implementations;
using Template.Interfaces;
using Template.ValueObjects;

namespace Application.UnitTests
{
    [TestClass]
    public class TemplateTest
    {
       [TestMethod]
        public void TemplateExecution_StringComparison_ShouldSucceed()
        {
            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(tr => tr.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                        new Template.Entities.Template(
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            "user",
                            "pie",
                            "bar",
                            DateTime.Now,
                            "template",
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.title"),
                            new UserDefinedWhereQueryStatement(
                                "shiporder.items.item.country.name", "=", "Norway"),
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.price")
                            )));

            var xmlFile = File.ReadAllBytes("resources/test.xml");

            var dataSource = new DataSource.Entities.DataSource(
                Guid.NewGuid(),
                "dataSource",
                DateTime.Now,
                "user",
                new List<DataTypeDefinition>(),
                xmlFile);

            var dataSourceSchemaGenerator = new DataSourceSchemaGenerator();
            dataSource.Schema = dataSourceSchemaGenerator.Generate(dataSource);

            var dataSourceRepositoryMock = new Mock<IDataSourceRepository>();
            dataSourceRepositoryMock.Setup(ds => ds.FindAsync(It.IsAny<Guid>(), 
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(dataSource));

            var templateService = new TemplateService(
                templateRepositoryMock.Object,
                dataSourceRepositoryMock.Object);

            var result = templateService.Execute("user", Guid.NewGuid())
                .GetAwaiter()
                .GetResult();

            result.Value.Count.Should().Be(2);

            result.Value.Should().BeEquivalentTo(
            new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Hide your heart", "9.90"),
                new Tuple<string, string>("Item3", "15.00")
            });
        }

        [TestMethod]
        public void TemplateExecution_IntegerComparison_ShouldSucceed()
        {
            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(tr => tr.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                        new Template.Entities.Template(
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            "user",
                            "pie",
                            "bar",
                            DateTime.Now,
                            "template",
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.title"),
                            new UserDefinedWhereQueryStatement(
                                "shiporder.items.item.quantity", ">", "1"),
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.price")
                            )));

            var xmlFile = File.ReadAllBytes("resources/test.xml");

            var dataSource = new DataSource.Entities.DataSource(
                Guid.NewGuid(),
                "dataSource",
                DateTime.Now,
                "user",
                new List<DataTypeDefinition>(),
                xmlFile);

            var dataSourceSchemaGenerator = new DataSourceSchemaGenerator();
            dataSource.Schema = dataSourceSchemaGenerator.Generate(dataSource);

            var dataSourceRepositoryMock = new Mock<IDataSourceRepository>();
            dataSourceRepositoryMock.Setup(ds => ds.FindAsync(It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(dataSource));

            var templateService = new TemplateService(
                templateRepositoryMock.Object,
                dataSourceRepositoryMock.Object);

            var result = templateService.Execute("user", Guid.NewGuid())
                .GetAwaiter()
                .GetResult();

            result.Value.Count.Should().Be(1);

            result.Value.Should().BeEquivalentTo(
            new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Item3", "15.00")
            });
        }

        [TestMethod]
        public void TemplateExecution_IntegerAttributeComparison_ShouldSucceed()
        {
            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(tr => tr.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                        new Template.Entities.Template(
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            "user",
                            "pie",
                            "bar",
                            DateTime.Now,
                            "template",
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.title"),
                            new UserDefinedWhereQueryStatement(
                                "shiporder.items.item.order", "<", "2"),
                            new UserDefinedReturnQueryStatement(
                                "shiporder.items.item.price")
                            )));

            var xmlFile = File.ReadAllBytes("resources/test.xml");

            var dataSource = new DataSource.Entities.DataSource(
                Guid.NewGuid(),
                "dataSource",
                DateTime.Now,
                "user",
                new List<DataTypeDefinition>(),
                xmlFile);

            var dataSourceSchemaGenerator = new DataSourceSchemaGenerator();
            dataSource.Schema = dataSourceSchemaGenerator.Generate(dataSource);

            var dataSourceRepositoryMock = new Mock<IDataSourceRepository>();
            dataSourceRepositoryMock.Setup(ds => ds.FindAsync(It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(dataSource));

            var templateService = new TemplateService(
                templateRepositoryMock.Object,
                dataSourceRepositoryMock.Object);

            var result = templateService.Execute("user", Guid.NewGuid())
                .GetAwaiter()
                .GetResult();

            result.Value.Count.Should().Be(1);

            result.Value.Should().BeEquivalentTo(
            new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Empire Burlesque", "10.90")
            });
        }
    }
}
