using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Search
{
    [Serializable]
    public struct GeneticSettings
    {
        public readonly float PercentageSurvivors;
        public readonly float PercenetageRandomSurvivors;

        public readonly int PopulationCount;

        public readonly OrderOfEvolution OrderOfEvolutionMethods;

        public GeneticSettings(float percSurv, float percRand, int popCount, OrderOfEvolution evol)
        {
            (percSurv > 0 && percSurv <= 1).AssertTrue();
            (percRand > 0 && percRand <= 1).AssertTrue();
            (percRand < percSurv).AssertTrue();
            (popCount > 0).AssertTrue();

            this.PercentageSurvivors = percSurv;
            this.PercenetageRandomSurvivors = percRand;
            this.PopulationCount = popCount;

            this.OrderOfEvolutionMethods = evol;
        }

        public enum OrderOfEvolution : byte
        {
            MutateCrossover = 0,
            CrossoverMutate = 1
        }
    }
}
