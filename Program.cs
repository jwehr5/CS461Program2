using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            /*
            Schedule s = new Schedule(random);

            foreach (var entry in s.individualSchedule.listOfActivityAssignments)
            {
                Console.WriteLine(entry.Key);
                Console.WriteLine(entry.Value);
                Console.WriteLine();
            }
            s.CalculateFitness();
            Console.WriteLine(s.Fitness);
            */

           

            GeneticAlgorithm ga = new GeneticAlgorithm(random);
            int runs = 0;
            while(runs < 100 || !ga.CanStopRunningAlgorithm)
            {
                ga.NewGeneration();
                runs++;
            }

            Console.WriteLine(ga.Generation);

            foreach (var entry in ga.BestSchedule.individualSchedule.listOfActivityAssignments)
            {
                Console.WriteLine(entry.Key);
                Console.WriteLine(entry.Value);
                Console.WriteLine();
            }


            Console.ReadLine();
        }
    }
}
