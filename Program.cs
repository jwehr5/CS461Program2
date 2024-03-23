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

            Schedule s = new Schedule();

            foreach (var entry in s.individualSchedule.listOfActivityAssignments)
            {
                Console.WriteLine(entry.Key);
                Console.WriteLine(entry.Value);
            }
            s.CalculateFitness();
            Console.WriteLine(s.Fitness);

            //GeneticAlgorithm g = new GeneticAlgorithm();

            Console.ReadLine();
        }
    }
}
