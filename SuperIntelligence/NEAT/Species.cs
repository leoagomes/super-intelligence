using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIntelligence.NEAT
{
    public class Species
    {
        public static double CompatibilityThreshold = 3.0;

        public int Id;
        public Genome Representative;
        public List<Genome> Members;

        public Species(Genome representative)
        {
            Members = new List<Genome>();
            Representative = representative;
        }

        public bool CompatibleWith(Genome genome) =>
            Representative.CompatibilityWith(genome) < CompatibilityThreshold;

        public void AddGenome(Genome genome) =>
            Members.Add(genome);

        public Genome RandomMember() =>
            Random.Random.Choose(Members.ToArray());

        public List<Genome> SortedMembers() =>
            Members.OrderByDescending(m => m.Fitness).ToList();
    }

}
