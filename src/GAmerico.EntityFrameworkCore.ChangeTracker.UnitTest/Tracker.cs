using System;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

public class Tracker : IObserver<ObjectChanged<Person>>
{

    public int OnNextCount { get; private set; }

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
        OnNextCount++;
        Console.WriteLine(value);
    }

    public void ResetCount()
    {
        OnNextCount = 0;
    }
}