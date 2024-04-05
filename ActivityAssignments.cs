using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Program2
{
    public class ActivityAssignments
    {
        //Array for storing all the activity names.
        public string[] activites { get; } = { "SLA100A", "SLA100B", "SLA191A", "SLA191B", "SLA201", "SLA291", "SLA303", "SLA304", "SLA394", "SLA449", "SLA451" };

        //Array for storing the rooms. The room name is the key and the value is the capacity.
        public KeyValuePair<string, int>[] rooms { get; } = { new KeyValuePair<string, int>("Slater 003", 45), new KeyValuePair<string, int>("Roman 216", 30), new KeyValuePair<string, int>("Loft 206", 75),
                                              new KeyValuePair<string,int>("Roman 201", 50), new KeyValuePair<string, int>("Loft 310", 108), new KeyValuePair<string, int>("Beach 201", 60),
                                              new KeyValuePair<string, int>("Beach 301", 75), new KeyValuePair<string, int>("Logos 325", 450), new KeyValuePair<string, int>("Frank 119", 60)};

        //Array for storing the times.
        public string[] times { get; } = { "10AM", "11AM", "12PM", "1PM", "2PM", "3PM" };

        //Array for storing the names of the facilitators.
        public string[] facilitators { get; } = { "Lock", "Glen", "Banks", "Richards", "Shaw", "Singer", "Uther", "Tyler", "Numen", "Zeldin" };

        //This dictionary will store key value pairs, the activity will be the key and the room, time and facilitator will be the value.
        public Dictionary<string, Tuple<KeyValuePair<string, int>, string, string>> listOfActivityAssignments  { get; set; }

        Random random;

        public ActivityAssignments(Random rand, bool shouldInitSchedule = true)
        {
            
            this.random = rand;

            listOfActivityAssignments = new Dictionary<string, Tuple<KeyValuePair<string, int>, string, string>>(11);

            if (shouldInitSchedule)
            {
                for (int i = 0; i < 11; i++)
                {
                    listOfActivityAssignments.Add(activites[i], generateRandomActivityAssignment());
                }
            }

           


        }

        /**
         *  Method for randomly generating a room, time, and faciliator for an activity. 
         */
        public Tuple<KeyValuePair<string, int>, string, string> generateRandomActivityAssignment()
        {
            int randomRoomIndex = random.Next(9);
            int randomTimeIndex = random.Next(6);
            int randomFacilitatorIndex = random.Next(10);

            Tuple<KeyValuePair<string, int>, string, string> assignment = new Tuple<KeyValuePair<string, int>, string, string>(rooms[randomRoomIndex], times[randomTimeIndex], facilitators[randomFacilitatorIndex]);

            return assignment;


        }


    }
}
