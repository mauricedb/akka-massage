using System;
using akka_massage.Messages;
using Akka.Actor;

namespace akka_massage.Actors
{
    // Create the actor class
    public class GreetingActor : ReceiveActor
    {
        public GreetingActor()
        {
            // Tell the actor to respond
            // to the Greet message
            Receive<Greet>(greet =>
                Console.WriteLine("Hello {0}", greet.Who));
        }
    }
}