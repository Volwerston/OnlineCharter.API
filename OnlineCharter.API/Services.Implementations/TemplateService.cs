using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using DataSource.Interfaces;
using Microsoft.Xml.XQuery;
using Newtonsoft.Json;
using Services.Implementations.Models;
using Services.Interfaces;
using Template.Interfaces;

namespace Services.Implementations
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IDataSourceRepository _dataSourceRepository;

        public TemplateService(
            ITemplateRepository templateRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _templateRepository = templateRepository;
            _dataSourceRepository = dataSourceRepository;
        }

        public async Task<IList<Tuple<decimal, decimal>>> Execute(Guid templateId)
        {
            var template = await _templateRepository.Get(templateId);
            var dataSource = await _dataSourceRepository.FindAsync(template.DataSourceId, true);

            var ms = new MemoryStream(dataSource.Value, 0, dataSource.Value.Length);

            var xDoc = new XmlDocument();
            xDoc.Load(ms);

            var xRoot = xDoc.DocumentElement;
            var nodes = xRoot.SelectNodes("/shiporder/item[(quantity=1) or (country/name='Sweden')]/price");
            //var dataSourceRelativePath = GetDataSourcePath(dataSource.Id);

            //using (var tr = new TemporaryResource(dataSourceRelativePath, dataSource.Value))
            //{
            //    var uri = await tr.Save();

            //    var col = new XQueryNavigatorCollection();
            //    col.AddNavigator(uri.ToString(), "doc");

            //    var queryString = template.KeySelector.BuildQuery();
            //    var xepr = new XQueryExpression(queryString);

            //    var ms = new MemoryStream();
            //    var result = xepr.Execute(col).ToXml();
            //}

            return new List<Tuple<decimal, decimal>>();
        }

        private static string GetDataSourcePath(Guid dataSourceId)
            => $"OpenCharter/dataSources/1.xml";
    }
}
