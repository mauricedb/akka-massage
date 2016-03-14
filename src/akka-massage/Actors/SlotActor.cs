using System;
using System.Threading.Tasks;
using akka_massage.Messages;
using Akka.Actor;

namespace akka_massage.Actors
{
    public class SlotActor : ReceiveActor
    {
        public DateTime StartTime { get; private set; }
        public Masseur Masseur { get; private set; }
        public Employee  Employee { get; private set; }
         
        public SlotActor()
        {
            Receive<BuildSlot>(slot => HandleBuildSlot(slot));
            Receive<BookMassage>(booking => HandleBookMassage(booking));
            Receive<Print>(p => HandlePrint(p));
        }

        private void HandlePrint(Print print)
        {
            Console.WriteLine($"{Employee?.Name} is massaged at {Masseur.Name} at {StartTime}.");
        }

        private void HandleBookMassage(BookMassage booking)
        {
            Employee = booking.Employee;
        }

        private void HandleBuildSlot(BuildSlot slot)
        {
            StartTime = slot.Date.AddMinutes(slot.TimeSlot.TotalMinutes());
            Masseur = slot.Masseur;

            HandlePrint(null);
        }
    }
}