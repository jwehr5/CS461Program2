using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    public class GeneticAlgorithm
    {
        //This will represent our population
        public List<Schedule> PopulationOfSchedules { get; set; }

        public int Generation { get; set; }

        public double MutationRate { get; set; }

        public double FitnessSum {  get; set; }


        public GeneticAlgorithm()
        {
            Generation = 1;
            PopulationOfSchedules = new List<Schedule>();

            //Initialize 500 schedules
            for(int i = 0; i < 500; i++)
            {
                PopulationOfSchedules.Add(new Schedule());
            }
        }

    }
}
