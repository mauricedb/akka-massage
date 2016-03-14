using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var slotName = SlotName(bookMassage.Masseur, bookMassage.TimeSlot);

            Context.Child(slotName).Tell(bookMassage);
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
                    var slotName = SlotName(masseur, timeSlot);
                    var slot = Context.ActorOf<SlotActor>(slotName);
                    slot.Tell(new BuildSlot(buildSchedule.Date, masseur, timeSlot));
                }
            }
        }

        protected override void PreStart()
        {
            base.PreStart();
            Console.WriteLine($"PreStart {GetType().FullName}");
        }

        private string SlotName(Masseur masseur, TimeSlot timeSlot)
        {
            return $"{masseur.Name}-{timeSlot.Hour}:{timeSlot.Minute}";
        }
    }
};