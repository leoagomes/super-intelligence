using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.NEAT.Computation;

namespace SuperIntelligence.NEAT
{
    public class Network
    {
        List<ConstantProvider<double>> Inputs = new List<ConstantProvider<double>>();
        List<CNode> Outputs = new List<CNode>();
        Dictionary<int, CNode> Nodes = new Dictionary<int, CNode>();

        public Network(Genome genome)
        {
            foreach (var node in genome.Nodes.Values)
            {
                if (node.Type != NodeType.Input)
                {
                    CNode cNode = new CNode(node.Id, x => (1 / (1 + Math.Exp(-4.9 * x))));
                    Nodes[node.Id] = cNode;

                    if (node.Type == NodeType.Output)
                        Outputs.Add(cNode);
                }
                else
                    Inputs.Add(new ConstantProvider<double>());
            }

            foreach (var connection in genome.Connections.Values)
            {
                Node inputNode = genome.Nodes[connection.InputNode];
                Node outputNode = genome.Nodes[connection.OutputNode];

                IValueProvider<double> dependency =
                    inputNode.Type == NodeType.Input ?
                    Inputs[inputNode.Id] :
                    (IValueProvider<double>)Nodes[inputNode.Id];

                CNode output = Nodes[outputNode.Id];
                output.AddPair(dependency, connection.Weight);
            }
        }

        public List<double> Forward(List<double> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i].Constant = inputs[i];
            }

            List<double> output = Outputs.Select(node => node.GetValue()).ToList();

            foreach (CNode node in Nodes.Values)
                node.MarkRecalculate();

            return output;
        }
    }
}
