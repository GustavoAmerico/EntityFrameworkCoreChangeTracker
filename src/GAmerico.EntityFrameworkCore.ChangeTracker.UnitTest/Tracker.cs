using Microsoft.EntityFrameworkCore;
using System;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

public class Tracker : IObserver<ObjectChanged<Person>>
{
    public int OnNextCount { get; private set; }

    public int OnAddCount { get; private set; }

    public int ONUpdateCount { get; private set; } = 0;

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
        if (value.EventId == (int)EntityState.Added)
            OnAddCount++;
        else if (value.EventId == (int)EntityState.Modified)
            ONUpdateCount++;
        Console.WriteLine(value);
    }

    public void ResetCount()
    {
        OnNextCount = 0;
    }
}