using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SuperIntelligence.Random.Random;

namespace SuperIntelligence.NEAT
{
    class Genome
    {
        public int Id;
        public int NextNodeId = 0;
        public double Fitness = 0;

        public Dictionary<int, Connection> Connections;
        public Dictionary<int, Node> Nodes;

        public Genome(int id)
        {
            Id = id;
            Connections = new Dictionary<int, Connection>();
            Nodes = new Dictionary<int, Node>();
        }

        #region Getters/Setters
        public Node GetNode(int id) =>
            Nodes[id];

        public Connection GetConnection(int innovation) =>
            Connections[innovation];
        #endregion

        #region Helpers
        public void AddNode(Node node) =>
            Nodes[node.Id] = node;

        public void AddConnection(Connection connection)
        {
            Connections[connection.InnovationNumber] = connection;
        }
        #endregion

        public void CreateConnection(InnovationGenerator generator)
        {
            // pick a source node
            Node source;
            do
            {
                source = Choose(Nodes.Values.ToArray());
            } while (source.Type == NodeType.Output);

            // and a destination node
            Node destination;
            do
            {
                destination = Choose(Nodes.Values.ToArray());
            } while (destination.Mark <= source.Mark);

            // now we can create the new connection
            double weight = GenerateWeight(); // TODO: recheck this
            int innovation = generator.Innovate();
            Connection connection = new Connection(source.Id, destination.Id, weight, true, innovation);

            Connections[innovation] = connection;
        }

        public void CreateNode(InnovationGenerator generator)
        {
            // pick a connection to split
            Connection connection = Choose(Connections.Values.ToArray());

            Node input = GetNode(connection.InputNode);
            Node output = GetNode(connection.OutputNode);

            // marks are a way of preventing cycles in the graph since
            // all ouputs must be from a higher mark than the input, which
            Node node = new Node(NextNodeId++, NodeType.Hidden, input.Mark + 1);

            // create the two new connections
            Connection a = new Connection(input.Id, node.Id, 1, true, generator.Innovate());
            Connection b = new Connection(node.Id, output.Id, connection.Weight, true, generator.Innovate());

            // disable the original connection
            connection.Expressed = false;

            // add the new connections to the genome
            AddNode(node);
            AddConnection(a);
            AddConnection(b);
        }

        public static double WeightMutationProbability = 0.8;
        public static double WeightPerturbanceProbability = 0.9;
        public void MutateWeights()
        {
            foreach (Connection conn in Connections.Values)
            {
                if (!InChance(WeightMutationProbability))
                    continue;

                if (InChance(WeightPerturbanceProbability))
                    conn.Weight = conn.Weight * (Double() * 4f - 2f);
                else
                    conn.Weight = Double() * 4f - 2f;
            }
        }

        public static double CreateNodeProbability = 0.03;
        public static double CreateConnectionProbability = 0.05;
        public void Mutate(InnovationGenerator generator)
        {
            if (InChance(WeightMutationProbability))
                MutateWeights();

            if (InChance(CreateNodeProbability))
                CreateNode(generator);

            if (InChance(CreateConnectionProbability))
                CreateConnection(generator);
        }

        public Genome Copy()
        {
            Genome copy = new Genome(Id);

            copy.NextNodeId = NextNodeId;

            foreach (var entry in Connections)
                copy.Connections.Add(entry.Key, entry.Value.Copy());

            foreach (var entry in Nodes)
                copy.Nodes.Add(entry.Key, entry.Value.Copy());

            return copy;
        }

        #region Static methods
        public static Genome CrossOver(Genome fit, Genome other)
        {
            Genome child = new Genome(0);

            foreach (Connection conn in fit.Connections.Values)
            {
                Connection newConn;
                bool eitherDisabled = true;

                if (other.Connections.ContainsKey(conn.InnovationNumber))
                {
                    newConn = Choose(conn, other.Connections[conn.InnovationNumber]).Copy();
                    eitherDisabled = (conn.Expressed == false ||
                        other.Connections[conn.InnovationNumber].Expressed == false);
                }
                else
                {
                    newConn = conn.Copy();
                }

                newConn.Expressed = eitherDisabled ? !InChance(0.75) : newConn.Expressed;

                child.AddConnection(newConn);
            }

            foreach (Node node in fit.Nodes.Values)
                child.AddNode(node);

            child.NextNodeId = fit.NextNodeId;

            return child;
        }

        public static double ExcessCoefficient = 1.0;
        public static double DisjointCoefficient = 1.0;
        public static double WeightCoefficient = 0.4;
        public double CompatibilityWith(Genome other)
        {
            int maxMutualInnovation = Math.Min(Connections.Keys.Max(), other.Connections.Keys.Max());

            double meanWeightDiff = 0;

            int disjoint = 0;
            int matching = 0;
            for (int i = 0; i < maxMutualInnovation; i++)
            {
                bool aContains = Connections.ContainsKey(i);
                bool bContains = other.Connections.ContainsKey(i);
                if (aContains ^ bContains)
                    disjoint += 1;

                if (aContains && bContains)
                {
                    meanWeightDiff += Math.Abs(Connections[i].Weight - other.Connections[i].Weight);
                    matching++;
                }
            }
            meanWeightDiff /= matching;

            int maxInnovation = Math.Max(Connections.Keys.Max(), other.Connections.Keys.Max());
            int excess = 0;
            for (int i = maxMutualInnovation; i < maxInnovation; i++)
            {
                bool aContains = Connections.ContainsKey(i);
                bool bContains = other.Connections.ContainsKey(i);
                if (aContains ^ bContains)
                    excess += 1;
            }

            int maxGenes = Math.Max(Connections.Count, other.Connections.Count);
            return (((ExcessCoefficient * excess) + (DisjointCoefficient * disjoint)) / maxGenes) + 
                WeightCoefficient * meanWeightDiff;
        }
        #endregion
    }
}
