using Accord.Math.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            Console.WriteLine("Generation No: " + ga.Generation);
            //Console.WriteLine("Population Count: " + ga.PopulationOfSchedules.Count);

            foreach (var entry in ga.BestSchedule.individualSchedule.listOfActivityAssignments)
            {
                Console.WriteLine(entry.Key);
                Console.WriteLine("  " + "Room: " + entry.Value.Item1.Key);
                Console.WriteLine("  " + "Time: " + entry.Value.Item2);
                Console.WriteLine("  " + "Facilitator: " + entry.Value.Item3);
                Console.WriteLine();
            }


            //Output the schedule to a file
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(projectDirectory, "Schedule.txt")))
            {
                foreach (var entry in ga.BestSchedule.individualSchedule.listOfActivityAssignments)
                {
                    outputFile.WriteLine(entry.Key);
                    outputFile.WriteLine("  " + "Room: " + entry.Value.Item1.Key);
                    outputFile.WriteLine("  " + "Time: " + entry.Value.Item2);
                    outputFile.WriteLine("  " + "Facilitator: " + entry.Value.Item3);
                    outputFile.WriteLine();
                }

            }

            //Door stop to prevent the program from closing automatically.
            Console.ReadLine();
        }
    }
}
