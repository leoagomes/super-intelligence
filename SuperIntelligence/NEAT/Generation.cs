using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperIntelligence.Data.Constants;
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

        /// <summary>
        /// Number of times each of the selected genomes will be reproduced with the top genome.
        /// </summary>
        public int ReproductionsPerGenome = 2;

        /// <summary>
        /// Number of best genomes to select.
        /// </summary>
        public int NBest;

        /// <summary>
        /// The algorithm to select genomes to reproduce. See SelectionAlgorithms enum at SuperIntelligence.Data for
        /// more information about the algorithms.
        /// </summary>
        public int SelectionAlgorithm;

        public Generation(int number, int selectionAlgorithm, int reproductionsPerGenome, int nBest)
        {
            Number = number;
            Species = new List<Species>();
            SelectionAlgorithm = selectionAlgorithm;
            ReproductionsPerGenome = reproductionsPerGenome;
            NBest = nBest;
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
        public Generation Next(InnovationGenerator generator, InnovationGenerator genomeGenerator, int selectionAlgorithm,
                               int reproductionsPerGenome, int nBest)
        {
            Generation next = new Generation(Number + 1, selectionAlgorithm, reproductionsPerGenome, nBest);
            int genomeId = 0;
            int oldSpecieCount = 0;
            List<Genome> toReproduce = new List<Genome>();

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
                switch (selectionAlgorithm)
                {
                    case (int)SelectionAlgorithms.Luhn:
                        toReproduce = LuhnCut(sortedMembers, oldSpecieCount / ReproductionsPerGenome);
                        break;

                    // no need to cut sortedMembers, use all
                    case (int)SelectionAlgorithms.AllWithBest:
                        toReproduce = sortedMembers.ToList();
                        ReproductionsPerGenome = 1;
                        break;
                    
                    case (int)SelectionAlgorithms.NWithBest:
                        toReproduce = SelectNBestGenomes(sortedMembers, nBest);
                        ReproductionsPerGenome = oldSpecieCount / nBest;
                        Console.WriteLine("nBest: {0}", nBest);
                        break;

                    // the default genome selection is Luhn
                    default:
                        toReproduce = LuhnCut(sortedMembers, oldSpecieCount / ReproductionsPerGenome);
                        break;
                }

                Console.WriteLine("ReproductionsPerGenome: {0}", ReproductionsPerGenome);

                foreach (Genome g in toReproduce)
                {
                    if (g == top)
                        continue;

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

        #region Reproduction Genome Selection Algorithms
        /// <summary>
        /// Applies Luhn's cut.
        /// </summary>
        /// <param name="sortedMembers">Genomes ordered by fitness</param>
        /// <param name="toRemove">Number of genomes to be removed</param>
        /// <returns></returns>
        public List<Genome> LuhnCut(IOrderedEnumerable<Genome> sortedMembers, int toRemove)
        {
            List<Genome> toReproduct = sortedMembers.ToList();

            // we need to differentiate the remotion whether 'toRemove' is odd or not
            // if even
            if (toRemove % 2 == 0)
            {
                for (int i = 0; i < toRemove; i += 2)
                {
                    // removing the last element
                    toReproduct.RemoveAt(toReproduct.Count() - 1);
                    // removing the first element
                    toReproduct.RemoveAt(0);
                }
            }
            // if odd
            else
            {
                for (int i = 0; i < toRemove - 1; i += 2)
                {
                    // removing the last element
                    toReproduct.RemoveAt(toReproduct.Count() - 1);
                    // removing the first element
                    toReproduct.RemoveAt(0);
                }
                // removing the last one
                toReproduct.RemoveAt(toReproduct.Count() - 1);
            }

            return toReproduct;
        }

        /// <summary>
        /// Select the 'n' best genomes from the previous generation.
        /// </summary>
        /// <param name="sortedMembers"></param>
        /// <param name="n">Number of genomes to be selected</param>
        /// <returns></returns>
        public List<Genome> SelectNBestGenomes (IOrderedEnumerable<Genome> sortedMembers, int n)
        {
            List<Genome> toReproduct = sortedMembers.ToList();
            toReproduct.RemoveRange(n, toReproduct.Count() - n);

            return toReproduct;
        }
        #endregion
    }
}
