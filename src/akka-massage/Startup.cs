using System;
using akka_massage.Actors;
using akka_massage.Messages;
using Akka.Actor;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace akka_massage
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var actorSystem = StartAkka();
            services.AddInstance(actorSystem);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);


        private static ActorSystem StartAkka()
        {
            // Create a new actor system (a container for your actors)
            var system = ActorSystem.Create("MySystem");

            // Create your actor and get a reference to it.
            // This will be an "ActorRef", which is not a
            // reference to the actual actor instance
            // but rather a client or proxy to it.
            var greeter = system.ActorOf<GreetingActor>("greeter");

            // Send a message to the actor
            greeter.Tell(new Greet("World"));
            greeter.Tell(new Greet("Akka"));


            var builder = system.ActorOf<ScheduleDayActor>(
                ScheduleDayActor.ActorName(DateTime.Today));

            var timeSlots = new[]
            {
                new TimeSlot(10, 0),
                new TimeSlot(10, 20),
                new TimeSlot(10, 40),
                new TimeSlot(11, 10)
            };
            var masseurs = new[]
            {
                new Masseur("Kim"),
                new Masseur("Linda")
            };

            builder.Tell(new BuildSchedule(
                DateTime.Today,
                masseurs,
                timeSlots));

            Console.ReadLine();
            builder.Tell(new Print());

            builder.Tell(new BookMassage(
                new Employee("Maurice"),
                new TimeSlot(11, 10),
                new Masseur("Kim")));

            //Console.ReadLine();
            builder.Tell(new BookMassage(
                new Employee("Erwin"),
                new TimeSlot(10, 20),
                new Masseur("Linda")));

            builder.Tell(new BookMassage(
                new Employee("Maurice"),
                new TimeSlot(10, 20),
                new Masseur("Linda")));


            Console.ReadLine();
            builder.Tell(new Print());

            return system;
        }
    }
}