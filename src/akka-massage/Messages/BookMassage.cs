namespace akka_massage.Messages
{
    public class BookMassage
    {
        public Employee Employee { get; private set; }
        public TimeSlot TimeSlot { get; private set; }
        public Masseur Masseur { get; private set; }

        public BookMassage(Employee  employee, TimeSlot timeSlot, Masseur masseur)
        {
            Employee = employee;
            TimeSlot = timeSlot;
            Masseur = masseur;
        }
    }
}