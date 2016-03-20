using System;
using akka_massage.Messages;
using Akka.Actor;
using Akka.Util.Internal;

namespace akka_massage.Actors
{
    public class ScheduleDayActor : ReceiveActor
    {
        public ScheduleDayActor()
        {
            Receive<BuildSchedule>(m => HandleBuildSchedule(m));
            Receive<BookMassage>(p => HandleBookMassage(p));
            Receive<Print>(p => HandlePrint(p));
            ReceiveAny(m => Console.WriteLine(m));
        }

        private void HandleBookMassage(BookMassage bookMassage)
        {
            var slotName = TimeSlotActor.ActorName(bookMassage.Masseur, bookMassage.TimeSlot);

            Context.Child(slotName).Forward(bookMassage);
        }

        private void HandlePrint(Print print)
        {
            Console.WriteLine("");
            Console.WriteLine("\tSchedule");
            Context
                .GetChildren()
                .ForEach(child => child.Tell(print));
        }

        private void HandleBuildSchedule(BuildSchedule buildSchedule)
        {
            foreach (var masseur in buildSchedule.Masseurs)
            {
                foreach (var timeSlot in buildSchedule.TimeSlots)
                {
                    var slotName = TimeSlotActor.ActorName(masseur, timeSlot);
                    var slot = Context.ActorOf<TimeSlotActor>(slotName);
                    slot.Tell(new BuildSlot(buildSchedule.Date, masseur, timeSlot));
                }
            }
        }

        protected override void PreStart()
        {
            base.PreStart();
            Console.WriteLine($"PreStart {GetType().FullName}");
        }

        public static string ActorName(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}