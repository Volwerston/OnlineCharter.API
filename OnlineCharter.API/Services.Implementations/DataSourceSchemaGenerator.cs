using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using DataSource.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfaces;
using Formatting = Newtonsoft.Json.Formatting;

namespace Services.Implementations
{
    public class DataSourceSchemaGenerator : IDataSourceSchemaGenerator
    {
        public List<DataTypeDefinition> Generate(DataSource.Entities.DataSource dataSource)
        {
            var schemaSet = GetSchemaSet(dataSource.Value);
            var en = schemaSet.GetEnumerator();
            en.MoveNext();
            var schema = (XmlSchema)en.Current;

            var list = new List<DataTypeDefinition>();
            TraverseSchema(schema, list);

            return list;
        }

        private static ICollection GetSchemaSet(byte[] file)
        {
            using (var ms = new MemoryStream(file))
            {
                var reader = XmlReader.Create(ms);
                var schema = new XmlSchemaInference();

                var schemaSet = schema.InferSchema(reader);

                return schemaSet.Schemas();
            }
        }

        private static void TraverseSimple(
            XmlSchemaElement element,
            string prefixName,
            ICollection<DataTypeDefinition> dataTypeDefinitions)
        {
            dataTypeDefinitions.Add(new DataTypeDefinition
            {
                Origin = DataTypeOrigin.Element,
                FullName = $"{prefixName}{element.QualifiedName.Name}",
                DataType = element.SchemaTypeName.Name
            });
        }

        private static void TraverseComplex(
            XmlSchemaElement element,
            StringBuilder prefix,
            List<DataTypeDefinition> dataTypeDefinitions)
        {
            if (!(element.ElementSchemaType is XmlSchemaComplexType complexType))
            {
                throw new ArgumentException($"Element must be of type '{nameof(XmlSchemaComplexType)}'");
            }

            if (complexType.AttributeUses.Count > 0)
            {
                foreach (var attr in complexType.AttributeUses)
                {
                    var entry = (DictionaryEntry)attr;
                    var attribute = (XmlSchemaAttribute)entry.Value;

                    dataTypeDefinitions.Add(new DataTypeDefinition
                    {
                        Origin = DataTypeOrigin.Attribute,
                        FullName = $"{prefix}{attribute.QualifiedName.Name}",
                        DataType = attribute.SchemaTypeName.Name
                    });
                }
            }

            if (complexType.ContentTypeParticle is XmlSchemaSequence seq)
            {
                //Assuming it’s a sequence of elements 
                foreach (var p in seq.Items)
                {
                    // if seqence is combination of elements and elementgroups we need to filter "XmlSchemaParticle p" // as we did XmlSchemaAttribute and XmlSchemaAttributeGroupRef
                    if (!(p is XmlSchemaElement elem)) continue;

                    switch (elem.ElementSchemaType)
                    {
                        case XmlSchemaComplexType _:
                            var sb = new StringBuilder(prefix.ToString());
                            sb.Append($"{elem.QualifiedName.Name}.");
                            TraverseComplex(elem, sb, dataTypeDefinitions);
                            break;
                        case XmlSchemaSimpleType _:
                            TraverseSimple(elem, prefix.ToString(), dataTypeDefinitions);
                            break;
                    }
                }
            }
        }

        private static void TraverseSchema(XmlSchema schema,
            List<DataTypeDefinition> dataTypeDefinitions)
        {
            foreach (XmlSchemaElement parentElement in schema.Elements.Values)
            {
                switch (parentElement.ElementSchemaType)
                {
                    case XmlSchemaComplexType _:
                        TraverseComplex(parentElement, new StringBuilder(), dataTypeDefinitions);
                        break;
                    case XmlSchemaSimpleType _:
                        TraverseSimple(parentElement, "", dataTypeDefinitions);
                        break;
                }
            }
        }
    }
}
