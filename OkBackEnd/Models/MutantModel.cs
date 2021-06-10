using System;
namespace Mercadolibre.Models
{
    public class StatsResponse
    {
        public int count_mutant_dna { get; set; }
        public int count_human_dna { get; set; }
    }

    public class DnaRequest
    {
        public string[] dna { get; set; }
    }
}
