using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Search
{
    public sealed class GeneticAlgorithm<T> : IEnumerable<T>
    {
        public T BestIndividual
        {
            get
            {
                return this.currentPopulation.First().Value;
            }
        }

        public IReadOnlyList<Individual> CurrentPopulation { get { return currentPopulation.AsReadOnly(); } }

        private readonly IGeneticFunctions<T> functions;
        private readonly GeneticSettings settings;

        private List<Individual> currentPopulation;

        public struct Individual
        {
            public readonly T Value;
            public readonly double Fitness;

            public Individual(T value, double fit)
            {
                this.Value = value;
                this.Fitness = fit;
            }
        }
        public GeneticAlgorithm(IGeneticFunctions<T> funcs, GeneticSettings settings, IEnumerable<T> population)
        {
            this.functions = funcs;
            this.settings = settings;
            var tempCount = population.Count();

            var collection = new List<T>(this.settings.PopulationCount);

            if (tempCount < settings.PopulationCount)
            {
                collection.AddRange(population);
                collection.AddRange(Enumerable.Range(0, this.settings.PopulationCount - tempCount).Select(x => funcs.Random()));
            }

            this.currentPopulation = (from person in collection
                                      select new Individual(person, this.functions.Fitness(person))).ToList();

            this.currentPopulation = (from person in this.currentPopulation
                                      orderby person.Fitness descending
                                      select person).ToList();
        }
        public GeneticAlgorithm(IGeneticFunctions<T> funcs, GeneticSettings settings)
            : this(funcs, settings, from item in Enumerable.Range(0, settings.PopulationCount) select funcs.Random()) { }

        public void CyclesUntilThreshold(double thres)
        {
            while (SingleEvolutionaryCycle() < thres)
            {
                continue;
            }
        }
        public double SingleEvolutionaryCycle()
        {
            int count = (int)(this.currentPopulation.Count * this.settings.PercentageSurvivors);

            var newPopulation = this.currentPopulation
                .Take(count)
                .Union(this.currentPopulation.Skip(count).RandomValues((int)(this.currentPopulation.Count * this.settings.PercenetageRandomSurvivors))).ToList();

            newPopulation.AddRange(Enumerable.Range(0, this.settings.PopulationCount - newPopulation.Count).Select(x =>
                {
                    var k = this.functions.Random();
                    return new Individual(k, this.functions.Fitness(k));
                }));

            List<T> afterCycle = new List<T>();

            switch (this.settings.OrderOfEvolutionMethods)
            {
                case GeneticSettings.OrderOfEvolution.MutateCrossover:
                    afterCycle = MutateCross(newPopulation);
                    break;
                case GeneticSettings.OrderOfEvolution.CrossoverMutate:
                    afterCycle = CrossMutate(newPopulation);
                    break;
            }

            this.currentPopulation = (from item in afterCycle
                                      where item != null
                                      select new Individual(item, this.functions.Fitness(item))).ToList();
            this.currentPopulation = (from item in this.currentPopulation
                                      orderby item.Fitness descending
                                      select item).ToList();

            return this.currentPopulation.First().Fitness;
        }

        public IEnumerable<T> OrderBy(Func<Individual, double> a)
        {
            return this.currentPopulation.OrderByDescending(a).Select(x => x.Value);
        }
        public void InPlaceWhere(Predicate<Individual> predicate)
        {
            this.currentPopulation = this.currentPopulation.Where(x => predicate(x)).ToList();
        }
        public void SubstitutePopulation(IEnumerable<T> l)
        {
            this.currentPopulation = l.Select(x => new Individual(x, this.functions.Fitness(x))).ToList();
        }
        private List<T> CrossMutate(IEnumerable<Individual> newPopulation)
        {
            var preList = newPopulation.ToList();
            var pairs = preList.Where((e, i) => i < (preList.Count - 1))
                    .Select((e, i) => new { First = e, Second = preList[i + 1] })
                    .ToList();

            int childrenPerPair = (int)(((double)this.settings.PopulationCount / (double)(pairs.Count)) + 0.5d);

            var final = new ConcurrentBag<T>();
            Parallel.ForEach(pairs, item =>
             {
                 for (int i = 0; i < childrenPerPair; i++)
                 {
                     final.Add(this.functions.Mutate(
                         this.functions.Cross(item.First.Value, item.Second.Value)));
                 }
             });

            return final.ToList();
        }
        private List<T> MutateCross(IEnumerable<Individual> newPopulation)
        {
            var list = (from item in newPopulation
                        select this.functions.Mutate(item.Value)).ToList();

            var pairs = list.Where((e, i) => i < list.Count - 1)
                    .Select((e, i) => new Tuple<T, T>(e, list[i + 1]))
                    .ToList();

            int childrenPerPair = (int)(((double)this.settings.PopulationCount / (double)(pairs.Count)) + 0.5d);
            
            var final = new ConcurrentBag<T>();

            Parallel.ForEach(pairs, item =>
            {
                if (item != null)
                {
                    for (int i = 0; i < childrenPerPair; i++)
                    {
                        final.Add(this.functions.Cross(item.Item1, item.Item2));
                    }
                }
            });

            return final.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.currentPopulation.Select(x => x.Value).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
