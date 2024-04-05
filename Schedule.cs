using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    public class Schedule
    {
        //The scheudle list acts as the genes for the schedule.
        public ActivityAssignments individualSchedule {  get; set; }

        public double Fitness { get; private set; }

        private Random random;

        public Schedule(Random rand, bool shouldInitSchedule = true)
        {
            this.random = rand;
            individualSchedule = new ActivityAssignments(rand, shouldInitSchedule);
            
            
        }

        public double CalculateFitness()
        {
            double activityFitness;
            foreach(var activity in individualSchedule.listOfActivityAssignments)
            {
                //The fitness for each activity initially starts at 0.
                activityFitness = 0;

                //Check the current activity to make sure it doesn't conflict with any other activity
                activityFitness += CheckForConflictingRoomAndTime(activity);
                //Fitness += activityFitness;

                //Console.WriteLine(Fitness);

                //Check the room size of the current activity.
                activityFitness += CheckRoomSize(activity);

                //Check the facilitator for each activity.
                activityFitness += CheckFacilitator(activity);

                //Check on the facilitator load.
                activityFitness += CheckFacilitatorLoad(activity);

                //Check the criteria in the activity-specific adjustments section. This only applies to SLA100A and B , and SLA191A and B.
                if(activity.Key.Equals("SLA100A") || activity.Key.Equals("SLA100B") || activity.Key.Equals("SLA191A") || activity.Key.Equals("SLA191B"))
                {
                    activityFitness += CheckActivitySpecificAdjustments(activity);
                }
                
                Fitness += activityFitness;
            }

            return Fitness;

        }

        /**
         * Method for checking that each activity does not have a conflicitng room and time with another activity. 
         */
        private double CheckForConflictingRoomAndTime(KeyValuePair<string, Tuple<KeyValuePair<string,int>, string, string>> currentActivity)
        {
            double sum = 0;
            foreach (var comparingActivity in individualSchedule.listOfActivityAssignments)
            {
                if (currentActivity.Key != comparingActivity.Key)
                {

                    if (currentActivity.Value.Item1.Equals(comparingActivity.Value.Item1) && currentActivity.Value.Item2.Equals(comparingActivity.Value.Item2))
                    {
                        //Console.WriteLine(currentActivity.Key);
                        //Console.WriteLine(comparingActivity.Key);
                        sum -= 0.5;
                    }
                }
                //Console.WriteLine();
            }

            return sum;
        }

        /**
         * Method for checking that the room for the activity is of reasonable size.
         */
        private double CheckRoomSize(KeyValuePair<string, Tuple<KeyValuePair<string, int>, string, string>> currentActivity)
        {
            double sum = 0;

            //The activities that have an expected enrollment of 50.
            if(currentActivity.Key.Equals("SLA100A") || currentActivity.Key.Equals("SLA100B") || currentActivity.Key.Equals("SLA191A") || currentActivity.Key.Equals("SLA191B")
               || currentActivity.Key.Equals("SLA201") || currentActivity.Key.Equals("SLA291"))
            {
                if(currentActivity.Value.Item1.Value < 50)
                {
                    sum -= 0.5;
                }else if (currentActivity.Value.Item1.Value > 150 && currentActivity.Value.Item1.Value < 300)
                {
                    sum -= 0.2;
                }else if(currentActivity.Value.Item1.Value > 300)
                {
                    sum -= 0.4;
                }
                else
                {
                    sum += 0.3;
                }
            //The activites that have an expected enrollment of 60.
            }else if(currentActivity.Key.Equals("SLA303") || currentActivity.Key.Equals("SLA449"))
            {
                if (currentActivity.Value.Item1.Value < 60)
                {
                    sum -= 0.5;
                }
                else if (currentActivity.Value.Item1.Value > 180 && currentActivity.Value.Item1.Value < 360)
                {
                    sum -= 0.2;
                }
                else if (currentActivity.Value.Item1.Value > 360)
                {
                    sum -= 0.4;
                }
                else
                {
                    sum += 0.3;
                }

            //The activity with a expected enrollment of 20.
            }else if (currentActivity.Key.Equals("SLA394"))
            {
                if (currentActivity.Value.Item1.Value < 20)
                {
                    sum -= 0.5;
                }
                else if (currentActivity.Value.Item1.Value > 60 && currentActivity.Value.Item1.Value < 120)
                {
                    sum -= 0.2;
                }
                else if (currentActivity.Value.Item1.Value > 120)
                {
                    sum -= 0.4;
                }
                else
                {
                    sum += 0.3;
                }

            //The activity with an expected enrollment of 25.
            }else if (currentActivity.Key.Equals("SLA304"))
            {
                if (currentActivity.Value.Item1.Value < 25)
                {
                    sum -= 0.5;
                }
                else if (currentActivity.Value.Item1.Value > 75 && currentActivity.Value.Item1.Value < 150)
                {
                    sum -= 0.2;
                }
                else if (currentActivity.Value.Item1.Value > 150)
                {
                    sum -= 0.4;
                }
                else
                {
                    sum += 0.3;
                }
            //The activity with an expected enrollment of 100.
            }else if (currentActivity.Key.Equals("SLA451"))
            {
                if (currentActivity.Value.Item1.Value < 100)
                {
                    sum -= 0.5;
                }
                else if (currentActivity.Value.Item1.Value > 300 && currentActivity.Value.Item1.Value < 600)
                {
                    sum -= 0.2;
                }
                else if (currentActivity.Value.Item1.Value > 600)
                {
                    sum -= 0.4;
                }
                else
                {
                    sum += 0.3;
                }
            }

            //Console.WriteLine(sum);
            return sum;
        }

        /**
         * Method for checking if each activity is assigned to a preferred facilitator.
         */
        private double CheckFacilitator(KeyValuePair<string, Tuple<KeyValuePair<string, int>, string, string>> currentActivity)
        {
            double sum = 0;

            //The list of preferred and other facilitators for SLA100 and SLA191 are the same.
            if(currentActivity.Key.Equals("SLA100A") || currentActivity.Key.Equals("SLA100B") || currentActivity.Key.Equals("SLA191A") || currentActivity.Key.Equals("SLA191B"))
            {
                if(currentActivity.Value.Item3.Equals("Glen") || currentActivity.Value.Item3.Equals("Lock") || currentActivity.Value.Item3.Equals("Banks") || currentActivity.Value.Item3.Equals("Zeldin"))
                {
                    sum += 0.5;
                }else if(currentActivity.Value.Item3.Equals("Numen") || currentActivity.Value.Item3.Equals("Richards"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA201.
            }else if (currentActivity.Key.Equals("SLA201"))
            {
                if (currentActivity.Value.Item3.Equals("Glen")  || currentActivity.Value.Item3.Equals("Banks") || currentActivity.Value.Item3.Equals("Zeldin") || currentActivity.Value.Item3.Equals("Shaw"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Numen") || currentActivity.Value.Item3.Equals("Richards") || currentActivity.Value.Item3.Equals("Singer"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA291
            }else if (currentActivity.Key.Equals("SLA291"))
            {
                if (currentActivity.Value.Item3.Equals("Lock") || currentActivity.Value.Item3.Equals("Banks") || currentActivity.Value.Item3.Equals("Zeldin") || currentActivity.Value.Item3.Equals("Singer"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Numen") || currentActivity.Value.Item3.Equals("Richards") || currentActivity.Value.Item3.Equals("Shaw") || currentActivity.Value.Item3.Equals("Tyler"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA303.
            }else if (currentActivity.Key.Equals("SLA303"))
            {
                if (currentActivity.Value.Item3.Equals("Glen") || currentActivity.Value.Item3.Equals("Zeldin") || currentActivity.Value.Item3.Equals("Banks"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Numen") || currentActivity.Value.Item3.Equals("Singer") || currentActivity.Value.Item3.Equals("Shaw"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA304.
            }else if (currentActivity.Key.Equals("SLA304"))
            {
                if (currentActivity.Value.Item3.Equals("Glen") || currentActivity.Value.Item3.Equals("Banks") || currentActivity.Value.Item3.Equals("Tyler"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Numen") || currentActivity.Value.Item3.Equals("Singer") || currentActivity.Value.Item3.Equals("Shaw") || currentActivity.Value.Item3.Equals("Richards")
                         || currentActivity.Value.Item3.Equals("Uther") || currentActivity.Value.Item3.Equals("Zeldin"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA394.
            }else if (currentActivity.Key.Equals("SLA394"))
            {
                if (currentActivity.Value.Item3.Equals("Tyler") || currentActivity.Value.Item3.Equals("Singer"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Richards") || currentActivity.Value.Item3.Equals("Zeldin"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA449.
            }else if (currentActivity.Key.Equals("SLA449"))
            {
                if (currentActivity.Value.Item3.Equals("Tyler") || currentActivity.Value.Item3.Equals("Singer") || currentActivity.Value.Item3.Equals("Shaw"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Zeldin") || currentActivity.Value.Item3.Equals("Uther"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }
            //Check facilitators for SLA451.
            }else if (currentActivity.Key.Equals("SLA451"))
            {
                if (currentActivity.Value.Item3.Equals("Tyler") || currentActivity.Value.Item3.Equals("Singer") || currentActivity.Value.Item3.Equals("Shaw"))
                {
                    sum += 0.5;
                }
                else if (currentActivity.Value.Item3.Equals("Zeldin") || currentActivity.Value.Item3.Equals("Uther") || currentActivity.Value.Item3.Equals("Richards") || currentActivity.Value.Item3.Equals("Banks"))
                {
                    sum += 0.2;
                }
                else
                {
                    sum -= 0.1;
                }

            }


            return sum;
        }


        /**
         *  Method for checking the constraints on each facilitator assignment. 
         */
        private double CheckFacilitatorLoad(KeyValuePair<string, Tuple<KeyValuePair<string, int>, string, string>> currentActivity)
        {
            double sum = 0;
            int activityCount = 1;
            int drTylersActivities = 0;
            bool onlyOneActivityAtAGivenTime = true;

            //Keep track of the number of activities that Dr Tyler is assigned to
            if (currentActivity.Value.Item3.Equals("Tyler"))
            {
                drTylersActivities++;
            }

            foreach(var comparingActivity in individualSchedule.listOfActivityAssignments)
            {
                
                if(currentActivity.Key != comparingActivity.Key)
                {
                    //Checking if the facilitator is assigned to another activty at the same time.
                    if (currentActivity.Value.Item2.Equals(comparingActivity.Value.Item2) && currentActivity.Value.Item3.Equals(comparingActivity.Value.Item3))
                    {
                        onlyOneActivityAtAGivenTime = false;
                    }

                    //For every activity that the facilitator teaches, increase activityCount by 1.
                    if (currentActivity.Value.Item3.Equals(comparingActivity.Value.Item3))
                    {
                        activityCount++;
                    }

                    if(comparingActivity.Value.Item3.Equals("Tyler"))
                    {
                        drTylersActivities++;
                    }

                    //Checking if the faciliator is assigned to consecutive time slots.
                    string time1 = currentActivity.Value.Item2;
                    string time2 = comparingActivity.Value.Item2;

                    //Calculate the time between the two activities.
                    TimeSpan duration = DateTime.Parse(time1).Subtract(DateTime.Parse(time2));

                    //Get the absolute value of the time.
                    duration = duration.Duration();
                    if (duration.ToString().Equals("01:00:00") && currentActivity.Value.Item3.Equals(comparingActivity.Value.Item3))
                    {
                        if((currentActivity.Value.Item1.Key.Equals("Roman") && comparingActivity.Value.Item1.Key.Equals("Beach"))
                           || (currentActivity.Value.Item1.Key.Equals("Beach") && comparingActivity.Value.Item1.Key.Equals("Roman"))
                           || (currentActivity.Value.Item1.Key.Equals(comparingActivity.Value.Item1.Key)))
                        {
                            sum += 0.5;
                        }
                        else
                        {
                            sum -= 0.4;
                        }
                    }

                }

                
            }

            if(activityCount > 4)
            {
                sum -= 0.5;
            }

            if (onlyOneActivityAtAGivenTime)
            {
                sum += 0.2;
            }
            else
            {
                sum -= 0.2;
            }

            if(drTylersActivities > 2)
            {
                sum -= 0.4;
            }

            //Console.WriteLine(sum);
            return sum;
        }

        /*
         *  Method for checking each of the constraints under the Activity-specific adjustments. 
        */
        private double CheckActivitySpecificAdjustments(KeyValuePair<string, Tuple<KeyValuePair<string, int>, string, string>> currentActivity)
        {
            double sum = 0;

            //Check to see if both sections of SLA100 and SLA191 are more than 4 hours apart or if they're in the same time slot.
            if (currentActivity.Key.Equals("SLA100A") || currentActivity.Key.Equals("SLA191A"))
            {
                foreach(var comparingActivity in individualSchedule.listOfActivityAssignments)
                {
                    
                    if ((currentActivity.Key.Equals("SLA100A") && comparingActivity.Key.Equals("SLA100B")) 
                        || (currentActivity.Key.Equals("SLA191A") && comparingActivity.Key.Equals("SLA191B")))
                    {
                        string time1 = currentActivity.Value.Item2;
                        string time2 = comparingActivity.Value.Item2;

                        //Check if the two sections are in the same time slot.
                        if (time1.Equals(time2))
                        {
                            sum -= 0.5;
                        }
                        else
                        {
                            //Calculate the time between the two activities.
                            TimeSpan duration = DateTime.Parse(time1).Subtract(DateTime.Parse(time2));

                            //Get the absolute value of the time.
                            duration = duration.Duration();
                            TimeSpan interval = new TimeSpan(4, 0, 0);

                            //Check if the two sections are more than 4 hours apart.
                            if (duration > interval)
                            {
                                sum += 0.5;
                            }
                        }


                    }

                }
            }

            //Check the constraints between a section of SLA100 and SLA191
            if(currentActivity.Key.Equals("SLA100A") || currentActivity.Key.Equals("SLA100B") || currentActivity.Key.Equals("SLA191A") || currentActivity.Key.Equals("SLA191B"))
            {
                foreach(var comparingActivity in individualSchedule.listOfActivityAssignments)
                {
                    if((currentActivity.Key.Equals("SLA100A") || currentActivity.Key.Equals("SLA100B")) && (comparingActivity.Key.Equals("SLA191A") || comparingActivity.Key.Equals("SLA191B")))
                    {
                        string time1 = currentActivity.Value.Item2;
                        string time2 = comparingActivity.Value.Item2;

                        //Calculate the time between the two activities.
                        TimeSpan duration = DateTime.Parse(time1).Subtract(DateTime.Parse(time2));

                        //Get the absolute value of the time.
                        duration = duration.Duration();

                        //Check if a section of SLA100 and SLA191 are in the same time slot
                        if (time1.Equals(time2))
                        {
                            sum -= 0.25;
                        
                        //Check if the activities are in consecutive time slots
                        }else if (duration.ToString().Equals("01:00:00"))
                        {
                            //Check if one of the activities is in Roman and the other one is in Beach. Or if both are in the same building.
                            if((currentActivity.Value.Item1.Key.Equals("Roman") && comparingActivity.Value.Item1.Key.Equals("Beach"))
                                || (currentActivity.Value.Item1.Key.Equals("Beach") && comparingActivity.Value.Item1.Key.Equals("Roman"))
                                || (currentActivity.Value.Item1.Key.Equals(comparingActivity.Value.Item1.Key)))
                            {
                                sum += 0.5;
                            }
                            else
                            {
                                sum -= 0.4;
                            }
                        //Check if the activities are separated by 1 hour.
                        }else if (duration.ToString().Equals("02:00:00"))
                        {
                            sum += 0.25;
                        }

                    }
                }
            }

            //Console.WriteLine("The sum of the CheckActivitySpecificAdjustments function is: " + sum);

            return sum;
        }

    }
}
