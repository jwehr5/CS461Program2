using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;


namespace Program2
{
    public class GeneticAlgorithm
    {
        //This will represent our population
        public List<Schedule> PopulationOfSchedules { get; set; }

        public int Generation { get; set; }

        public double MutationRate { get; set; }

        public double BestFitness { get; set; }
        public double GenerationAverageFitness {  get; set; }

        public double AverageFitnessOfGen100 { get; set; }

        public Schedule BestSchedule { get; set; }

        public double[] ProbabilityDistribution { get; set; }

        public bool CanStopRunningAlgorithm { get; set; } = false;

        Random rand;

        public GeneticAlgorithm(Random rand)
        {
            Generation = 1;
            MutationRate = 0.01f;
            GenerationAverageFitness = 0;
            AverageFitnessOfGen100 = 0;
            PopulationOfSchedules = new List<Schedule>();
            ProbabilityDistribution = new double[500];
            this.rand = rand;

            //Initialize 500 schedules
            for(int i = 0; i < 500; i++)
            {
                PopulationOfSchedules.Add(new Schedule(rand));

            }

          
        }

        public void NewGeneration()
        {
            if(PopulationOfSchedules.Count  <= 0)
            {
                return;
            }

            CalculateAverageGenerationFitness();
            //Console.WriteLine(GenerationAverageFitness);

            EliminateLeastFitIndividual();

            List<Schedule> newPopulation = new List<Schedule>();

            for(int i = 0; i < PopulationOfSchedules.Count; i++)
            {
                Schedule parent1 = ChooseParent();
                Schedule parent2 = ChooseParent();

                Schedule child = null;
                if (parent1 != null && parent2 != null)
                {
                    child = Crossover(parent1, parent2);

                    Mutate(child);

                    newPopulation.Add(child);
                }
                

               
            }

            PopulationOfSchedules = newPopulation;
            Generation++;

            //Console.WriteLine(newPopulation.Count);


           
        }

        private void CalculateAverageGenerationFitness()
        {
            Schedule bestSchedule = PopulationOfSchedules[0];
            double currentGenerationFitness = 0;

            //Get the average of the fitness scores for the population and find the best schedule
            for (int i = 0; i < PopulationOfSchedules.Count; i++)
            {
                currentGenerationFitness += PopulationOfSchedules[i].CalculateFitness();

                if (PopulationOfSchedules[i].Fitness > bestSchedule.Fitness)
                {
                    bestSchedule = PopulationOfSchedules[i];
                }
            }

            BestSchedule = bestSchedule;

            currentGenerationFitness /= PopulationOfSchedules.Count;
            
            //If the average fitness is better than the previous generation, divide the mutation rate in half.
            if(currentGenerationFitness > GenerationAverageFitness)
            {
                MutationRate /= 2;
            }

            GenerationAverageFitness = currentGenerationFitness;

            if(Generation == 100)
            {
                AverageFitnessOfGen100 = currentGenerationFitness;
            }

            if(Generation > 100)
            {
                if ((currentGenerationFitness - AverageFitnessOfGen100) > 0 && ((currentGenerationFitness - AverageFitnessOfGen100) / (AverageFitnessOfGen100 * 100)) < 0.01)
                {
                    CanStopRunningAlgorithm = true;
                }
            }
            
            


        }

        private void EliminateLeastFitIndividual()
        {
            for(int i = 0; i < PopulationOfSchedules.Count; i++)
            {
                ProbabilityDistribution[i] = PopulationOfSchedules[i].CalculateFitness();
            }

            //Convert the Fitness Scores to a probability distribution
            ProbabilityDistribution = Accord.Math.Special.Softmax(ProbabilityDistribution);

            List<double> probabilityDistList = new List<double>(ProbabilityDistribution);

            //Find the smallest value in the probability distribution array.
            double minValue = ProbabilityDistribution[0];
            int indexOfMinValue = 0;
            for(int i = 1; i < PopulationOfSchedules.Count; i++)
            {
                if(minValue > ProbabilityDistribution[i])
                {
                    minValue = ProbabilityDistribution[i];
                    indexOfMinValue = i;
                }

                //Console.WriteLine(ProbabilityDistribution[i]);
            }

            //Console.WriteLine(minValue);

            Schedule leastFitSchedule = PopulationOfSchedules[indexOfMinValue];


            //There may be multiple schedules that have the same score, so remove all of them.
            PopulationOfSchedules.RemoveAll(schedule => schedule.Equals(leastFitSchedule));

            //Console.WriteLine(PopulationOfSchedules.Count);

        }

        private Schedule ChooseParent()
        {
            /*
             *  Generate a random number between 0 and 1. Sum all the probabilities in the probability distribution until the sum exceeds r.
             *  Then return the schedule that we are currently at.
             */
            double r = rand.NextDouble();
            double sumOfProbabilities = 0;
            for(int i = 0; i < PopulationOfSchedules.Count; i++)
            {
                sumOfProbabilities += ProbabilityDistribution[i];

                if(sumOfProbabilities > r)
                {
                    return PopulationOfSchedules[i];
                }
            }

            return null;
        }

        private Schedule Crossover(Schedule parent1, Schedule parent2)
        {
            Schedule child = new Schedule(rand, shouldInitSchedule: false);

            /*
             *  For parent1, generate 2 random numbers, the start point and the end point.
             *  The child will inherit activites from parent1 that are within the range.
             *  The child will inherit activites from parent2 that were outside the range.
            */

            int startPoint = rand.Next(11);
            int endPoint = rand.Next(11);

            if(startPoint > endPoint)
            {
                (startPoint, endPoint) = (endPoint, startPoint);
            }

            for(int i = 0; i < 11; i++)
            {
                if(i >= startPoint && i <= endPoint)
                {
                    child.individualSchedule.listOfActivityAssignments.Add(child.individualSchedule.activites[i], 
                        parent1.individualSchedule.listOfActivityAssignments[child.individualSchedule.activites[i]]);
                }
                else
                {
                    child.individualSchedule.listOfActivityAssignments.Add(child.individualSchedule.activites[i],
                        parent2.individualSchedule.listOfActivityAssignments[child.individualSchedule.activites[i]]);
                }
            }


            foreach (var entry in child.individualSchedule.listOfActivityAssignments)
            {
                //Console.WriteLine(entry.Key);
                //Console.WriteLine(entry.Value);
                //Console.WriteLine();
            }

            return child;

        }

        private void Mutate(Schedule child)
        {
            double r = rand.NextDouble();

            if(r < MutationRate)
            {
                int randIndex = rand.Next(11);

                child.individualSchedule.listOfActivityAssignments[child.individualSchedule.activites[randIndex]] = child.individualSchedule.generateRandomActivityAssignment();
            }
        }
     



    }
}
