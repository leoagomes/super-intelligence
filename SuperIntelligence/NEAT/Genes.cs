using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.NEAT
{
    public class Connection
    {
        public int InputNode;
        public int OutputNode;
        public double Weight;
        public bool Expressed;
        public int InnovationNumber;

        public Connection(int inputNode, int outputNode, double weight, bool expressed, int innovationNumber)
        {
            InputNode = inputNode;
            OutputNode = outputNode;
            Weight = weight;
            Expressed = expressed;
            InnovationNumber = innovationNumber;
        }

        public Connection Copy() =>
            new Connection(InputNode, OutputNode, Weight, Expressed, InnovationNumber);
    }

    public enum NodeType
    {
        Input,
        Output,
        Hidden
    }

    public class Node
    {
        public int Id;
        public NodeType Type;
        public int Mark;

        public Node(int id, NodeType type, int mark)
        {
            Id = id;
            Type = type;
            Mark = mark;
        }

        public Node Copy() =>
            new Node(Id, Type, Mark);
    }
}
