using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SuperIntelligence.Random.Random;

namespace SuperIntelligence.NEAT
{
    public class Generation
    {
        public int Number;
        public List<Species> Species;

        /// <summary>
        /// The chance of interspecies mating.
        /// </summary>
        public static double InterspeciesMatingChance = 0.01;

        public Generation(int number)
        {
            Number = number;
            Species = new List<Species>();
        }

        /// <summary>
        /// Adds a genome to a compatible specie.
        /// </summary>
        /// <param name="genome"></param>
        public void AddGenome(Genome genome)
        {
            // add the genome to a compatible species
            foreach (Species species in Species)
            {
                if (species.CompatibleWith(genome))
                {
                    species.AddGenome(genome);
                    return;
                }
            }

            // if there isn't any compatible species, we create a new one
            Species newSpecies = new Species(genome);
            newSpecies.AddGenome(genome);
            Species.Add(newSpecies);
        }

        public double AdjustedFitness(Genome genome, double fitness)
        {
            int sharing = Species.Find(s => s.CompatibleWith(genome))?.Members.Count ?? 1;
            return fitness / sharing;
        }

        /// <summary>
        /// Creates a new generation.
        /// </summary>
        /// <param name="generator"></param>
        /// <returns></returns>
        public Generation Next(InnovationGenerator generator, InnovationGenerator genomeGenerator)
        {
            Generation next = new Generation(Number + 1);
            int genomeId = 0;
            int oldSpecieCount = 0;
            int ReproductionsPerGenome = 2;

            foreach (Species oldSpecies in Species)
            {
                // create a new species for the new genome with a random representative from
                // the current species
                Species newSpecies = new Species(Choose(oldSpecies.Members.ToArray()));
                next.Species.Add(newSpecies);

                oldSpecieCount = oldSpecies.Members.Count;

                // removing any genome that has a bad performance
                oldSpecies.Members.RemoveAll(item => item.Fitness <= 0);

                var sortedMembers = oldSpecies.Members
                    .OrderByDescending(m => AdjustedFitness(m, m.Fitness));

                Genome top = sortedMembers.First();

                //if (oldSpecies.Members.Count > 5)
                next.AddGenome(top);

                // applying Luhn's cut to the genomes
                List<Genome> toReproduce = LuhnCut(sortedMembers, oldSpecieCount / ReproductionsPerGenome);

                foreach (Genome g in toReproduce)
                {
                    for (int i = 0; i < ReproductionsPerGenome; i++)
                    {
                        Genome child = Genome.CrossOver(top, g);
                        child.Mutate(generator);
                        child.Id = genomeGenerator.Innovate();
                        next.AddGenome(child);
                    }
                }
            }

            // TODO: maybe rethink this
            for (int i = 0; i < next.Species.Count; i++)
            {
                if (InChance(InterspeciesMatingChance))
                {
                    Species other = Choose(next.Species.ToArray());
                    Genome cross = Genome.CrossOver(next.Species[i].Members.OrderByDescending(m => m.Fitness).First(),
                        other.Members.OrderByDescending(m => m.Fitness).First());
                    cross.Id = genomeId++; // Remove genomeId++ since it isn't the innovation number
                    next.AddGenome(cross);
                }
            }

            next.Species.RemoveAll((species) => species.Members.Count == 0);

            return next;
        }

        public void RemoveEmptySpecies() =>
            Species.RemoveAll(s => s.Members.Count == 0);

        public Species RandomSpecies() =>
            Choose(Species.ToArray());

        /// <summary>
        /// Applies Luhn's cut.
        /// </summary>
        /// <param name="sortedMembers"></param>
        /// <returns></returns>
        public List<Genome> LuhnCut(IOrderedEnumerable<Genome> sortedMembers, int toRemove)
        {
            List<Genome> toReproduct = sortedMembers.ToList();

            for (int i = 0; i < toRemove; i += 2)
            {
                // removing the last element
                toReproduct.RemoveAt(toReproduct.Count() - 1);
                // removing the first element
                toReproduct.RemoveAt(0);
            }

            return toReproduct;
        }
    }
}
