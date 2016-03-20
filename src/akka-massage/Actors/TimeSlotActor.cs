using System;
using akka_massage.Messages;
using Akka.Actor;
using Akka.Event;

namespace akka_massage.Actors
{
    public class TimeSlotActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public TimeSlotActor()
        {
            Receive<BuildSlot>(slot => HandleBuildSlot(slot));
            Unbooked();
        }

        public DateTime StartTime { get; private set; }
        public Masseur Masseur { get; private set; }
        public Employee Employee { get; private set; }

        public static string ActorName(Masseur masseur, TimeSlot timeSlot)
        {
            return $"{masseur.Name}-{timeSlot.Hour}:{timeSlot.Minute}";
        }

        private void Unbooked()
        {
            Receive<Print>(p => HandlePrint(p));
            Receive<BookMassage>(booking => HandleBookMassage(booking));
        }

        private void Booked()
        {
            Receive<Print>(p => HandlePrint(p));
            ReceiveAny(m => { _log.Warning("Unhandled message {0}", m); });
        }

        private void HandlePrint(Print print)
        {
            Console.WriteLine($"{Employee?.Name} is massaged at {Masseur.Name} at {StartTime}.");
        }

        private void HandleBookMassage(BookMassage booking)
        {
            Employee = booking.Employee;

            _log.Info("Massage booked for {0}, at {1} by {2}",
                Employee.Name, StartTime, Masseur.Name);

            Become(Booked);
        }

        private void HandleBuildSlot(BuildSlot slot)
        {
            StartTime = slot.Date.AddMinutes(slot.TimeSlot.TotalMinutes());
            Masseur = slot.Masseur;

            HandlePrint(null);
        }
    }
}