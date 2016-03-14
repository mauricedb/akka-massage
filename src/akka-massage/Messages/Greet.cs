using System;

namespace akka_massage.Messages
{
    public class Greet
    {
        public Greet(string who)
        {
            Who = who;
        }

        public string Who { get; private set; }
    }

    public class BuildSchedule
    {
        public DateTime Date { get; private set; }
        public Masseur[] Masseurs { get; private set; }
        public TimeSlot[] TimeSlots { get; private set; }

        public BuildSchedule(DateTime date, Masseur[] masseurs, TimeSlot[] timeSlots)
        {
            Date = date;
            Masseurs = masseurs;
            TimeSlots = timeSlots;
        }
    }

    public class BuildSlot
    {
        public DateTime Date { get; private set; }
        public Masseur Masseur { get; private set; }
        public TimeSlot TimeSlot { get; private set; }

        public BuildSlot(DateTime date, Masseur masseur, TimeSlot timeSlot)
        {
            Date = date;
            Masseur = masseur;
            TimeSlot = timeSlot;
        }
    }
}