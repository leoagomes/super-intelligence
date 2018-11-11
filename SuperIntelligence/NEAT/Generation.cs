using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SuperIntelligence.Random.Random;

namespace SuperIntelligence.NEAT
{
    class Generation
    {
        public int Number;
        public List<Species> Species;

        /// <summary>
        /// The chance of interspecies mating.
        /// </summary>
        public static double InterspeciesMatingChance = 0.01;

        /// <summary>
        /// Number of reproductions needed to generate another Generation.
        /// </summary>
        public int ReproductionsLeft = 0;

        /// <summary>
        /// Number of times a genome will be used to reproduce.
        /// </summary>
        public int ReproductionsPerGenome = 5;

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
        public Generation Next(InnovationGenerator generator, InnovationGenerator genome_generator)
        {
            Generation next = new Generation(Number + 1);
            int genomeId = 0;

            foreach (Species oldSpecies in Species)
            {
                // remove species with no members
                if (oldSpecies.Members.Count <= 0)
                {
                    Species.Remove(oldSpecies);
                    continue;
                }

                // create a new species for the new genome with a random representative from
                // the current species
                Species newSpecies = new Species(Choose(oldSpecies.Members.ToArray()));
                next.Species.Add(newSpecies);

                var sortedMembers = oldSpecies.Members
                    .OrderByDescending(m => AdjustedFitness(m, m.Fitness));

                Genome top = sortedMembers.First();

                if (oldSpecies.Members.Count > 5)
                    next.AddGenome(top);

                // updating the number of reproductions needed, excluding the top genome
                ReproductionsLeft = oldSpecies.Members.Count / ReproductionsPerGenome;

                // Removing any genome that has a bad performance
                int removedGenomes = oldSpecies.Members.RemoveAll(item => item.Fitness <= 0);

                // reproducing the top genome with others
                // we need to begin at 1 because the first member is always the top one
                for (int i = 1; i < ReproductionsLeft + 1; i++)
                {
                    for (int j = 0; j < ReproductionsPerGenome ; j++)
                    {
                        Genome child = Genome.CrossOver(top, sortedMembers.ElementAt(i));
                        child.Mutate(generator);
                        child.Id = genome_generator.Innovate();
                        next.AddGenome(child);
                    }
                }

                Console.WriteLine("next generation genomes:\n");
                foreach (Genome g in next.Species[0].Members)
                {
                    Console.WriteLine("#{0}", g.Id);
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
                    cross.Id = genomeId++;
                    next.AddGenome(cross);
                }
            }

            next.Species.RemoveAll((species) => species.Members.Count == 0);

            return next;
        }
    }
}
