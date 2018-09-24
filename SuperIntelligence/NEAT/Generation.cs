﻿using System;
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

        public Generation(int number)
        {
            Number = number;
            Species = new List<Species>();
        }

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

        public static double InterspeciesMatingChance = 0.01;
        public Generation Next(InnovationGenerator generator)
        {
            Generation next = new Generation(Number + 1);
            int genomeId = 0;

            foreach (Species oldSpecies in Species)
            {
                // skip species with no members
                if (oldSpecies.Members.Count <= 0)
                    continue;

                // create a new species for the new genome with a random representative from
                // the current species
                Species newSpecies = new Species(Choose(oldSpecies.Members.ToArray()));
                next.Species.Add(newSpecies);

                var sortedMembers = oldSpecies.Members
                    .OrderByDescending(m => AdjustedFitness(m, m.Fitness));

                Genome top = sortedMembers.First();

                if (oldSpecies.Members.Count > 5)
                    next.AddGenome(top);

                foreach (Genome genome in oldSpecies.Members)
                {
                    if (top == genome)
                        continue;

                    Genome child = Genome.CrossOver(top, genome);
                    child.Mutate(generator);
                    child.Id = genomeId++;
                    next.AddGenome(child);
                }

                /*
                 * this code takes the top sqrt of members and crosses them
                foreach (Genome genome in topMembers)
                {
                    Genome mutation = genome.Copy();
                    mutation.Mutate(generator);
                    mutation.Id = genomeId++;
                    next.AddGenome(mutation);

                    foreach (Genome other in topMembers)
                    {
                        if (genome == other)
                            continue;

                        double adjGenomeFitness = AdjustedFitness(genome, genome.Fitness);
                        double adjOtherFitness = AdjustedFitness(other, other.Fitness);

                        Genome fit = adjGenomeFitness > adjOtherFitness ? genome : other;
                        Genome cross = Genome.CrossOver(fit, fit == genome ? other : genome);
                        cross.Mutate(generator);
                        cross.Id = genomeId++;
                        next.AddGenome(cross);
                    }
                }
                */
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

            return next;
        }
    }
}
