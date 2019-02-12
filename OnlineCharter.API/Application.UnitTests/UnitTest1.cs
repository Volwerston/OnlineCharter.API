using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSource.Entities;
using DataSource.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Implementations;
using Template.Entities;
using Template.Interfaces;
using Template.ValueObjects;

namespace Application.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(tr => tr.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                        new Template.Entities.Template(
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            1,
                            DateTime.Now,
                            "template",
                            new XmlSourceQuery(
                                new XQueryForStatement(
                                    "1.xml"),
                                new XQueryWhereStatement(
                                    new AtomicUserDefinedWhereQueryStatement("this.shiporder.item.quantity", ">", "number(1)")),
                                new XQueryReturnStatement(new UserDefinedReturnQueryStatement(
                                    "this.shiporder.item.title"))))));

            var xmlFile = File.ReadAllBytes("resources/test.xml");
            var dataSourceRepositoryMock = new Mock<IDataSourceRepository>();
            dataSourceRepositoryMock.Setup(ds => ds.FindAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DataSource.Entities.DataSource(
                    Guid.NewGuid(),
                    "dataSource",
                    DateTime.Now,
                    "location",
                    1,
                    new List<DataTypeDefinition>(),
                    xmlFile)));

            var templateService = new TemplateService(
                templateRepositoryMock.Object,
                dataSourceRepositoryMock.Object);

            templateService.Execute(Guid.NewGuid()).GetAwaiter().GetResult();
        }
    }
}
