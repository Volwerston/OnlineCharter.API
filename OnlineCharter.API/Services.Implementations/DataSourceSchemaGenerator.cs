using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json.Linq;
using Services.Interfaces;

namespace Services.Implementations
{
    public class DataSourceSchemaGenerator : IDataSourceSchemaGenerator
    {
        public JObject Generate(DataSource.Entities.DataSource dataSource)
        {
            var schemaSet = GetSchemaSet(dataSource.Value);
            var en = schemaSet.GetEnumerator();
            en.MoveNext();
            var schema = (XmlSchema)en.Current;
            var sb = TraverseSchema(schema);

            return JObject.Parse(sb.ToString());
        }

        private static string StringWithIndent(string name, string value, int indent, bool includeComma = false)
        {
            var sb = new StringBuilder()
                .Append(' ', indent * 3)
                .Append(name)
                .Append(":")
                .Append(value);

            if (includeComma)
            {
                sb.Append(",");
            }

            return sb.ToString();
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

        private static void TraverseSimple(XmlSchemaElement element, int indent, bool isLastElem, StringBuilder sb)
            => sb.Append(
                StringWithIndent(
                    element.QualifiedName.Name,
                    element.SchemaTypeName.Name,
                    indent,
                    !isLastElem));

        private static void TraverseComplex(XmlSchemaElement element, int indent, StringBuilder sb)
        {
            if (!(element.ElementSchemaType is XmlSchemaComplexType complexType))
            {
                throw new ArgumentException($"Element must be of type '{nameof(XmlSchemaComplexType)}'");
            }

            var containsElements = complexType.ContentTypeParticle is XmlSchemaSequence;


            // No attributes support for MVP
            // Will be added later

            //if (complexType.AttributeUses.Count > 0)
            //{
            //    var attrCount = 0;
            //    // iterating XmlSchemaObjectCollection for attributes 
            //    foreach (var attr in complexType.AttributeUses)
            //    {
            //        ++attrCount;
            //        var isLastAttribute = attrCount == complexType.AttributeUses.Count;

            //        var entry = (DictionaryEntry)attr;
            //        var attribute = (XmlSchemaAttribute)entry.Value;
            //        sb.Append(
            //            StringWithIndent(
            //                attribute.QualifiedName.Name,
            //                attribute.SchemaTypeName.Name,
            //                indent,
            //                !(isLastAttribute && !containsElements)));
            //    }
            //}

            if (complexType.ContentTypeParticle is XmlSchemaSequence seq)
            {
                var elemCount = 0;
                //Assuming it’s a sequence of elements 
                foreach (var p in seq.Items)
                {
                    ++elemCount;
                    var isLastElement = elemCount == seq.Items.Count;

                    // if seqence is combination of elements and elementgroups we need to filter "XmlSchemaParticle p" // as we did XmlSchemaAttribute and XmlSchemaAttributeGroupRef
                    if (!(p is XmlSchemaElement elem)) continue;

                    switch (elem.ElementSchemaType)
                    {
                        case XmlSchemaComplexType _:
                            sb.Append(StringWithIndent(elem.QualifiedName.Name, "{", indent));
                            TraverseComplex(elem, indent + 1, sb);
                            sb.Append(new string(Enumerable.Repeat(' ', indent * 3).ToArray()) + "}");
                            if (!isLastElement)
                            {
                                sb.Append(",");
                            }
                            break;
                        case XmlSchemaSimpleType _:
                            TraverseSimple(elem, indent, isLastElement, sb);
                            break;
                    }
                }
            }
        }

        private static StringBuilder TraverseSchema(XmlSchema schema)
        {
            var elemCount = 0;
            var sb = new StringBuilder();

            foreach (XmlSchemaElement parentElement in schema.Elements.Values)
            {
                ++elemCount;
                var isLastElement = elemCount == schema.Elements.Count;

                switch (parentElement.ElementSchemaType)
                {
                    case XmlSchemaComplexType _:
                        sb.Append(StringWithIndent(parentElement.QualifiedName.Name, "{", 0));
                        TraverseComplex(parentElement, 1, sb);
                        sb.Append("}");
                        if (!isLastElement)
                        {
                            sb.Append(",");
                        }
                        break;
                    case XmlSchemaSimpleType _:
                        TraverseSimple(parentElement, 0, isLastElement, sb);
                        break;
                }
            }

            return sb;
        }
    }
}
