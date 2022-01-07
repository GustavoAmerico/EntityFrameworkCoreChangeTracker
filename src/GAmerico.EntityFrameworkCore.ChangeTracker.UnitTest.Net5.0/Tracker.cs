using System;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest.Net5._0
{
    public class Tracker : IObserver<ObjectChanged<Person>>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError");
        }

        public void OnNext(ObjectChanged<Person> value)
        {
            Console.WriteLine(value);
        }
    }
}