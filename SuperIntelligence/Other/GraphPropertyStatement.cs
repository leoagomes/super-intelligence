using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

using Shields.GraphViz.Models;

namespace SuperIntelligence.Other
{
    class GraphPropertyStatement : Statement
    {
        public Id Property;
        public Id Value;

        public GraphPropertyStatement(Id property, Id value, ImmutableDictionary<Id, Id> attributes) : base(attributes)
        {
            Property = property;
            Value = value;
        }

        public override void WriteTo(StreamWriter writer, GraphKinds graphKind)
        {
            Property.WriteTo(writer);
            writer.Write('=');
            Value.WriteTo(writer);

            if (Attributes.Any())
            {
                writer.Write('[');
                WriteAttributesTo(writer);
                writer.Write(']');
            }
            writer.Write(';');
        }
    }
}
