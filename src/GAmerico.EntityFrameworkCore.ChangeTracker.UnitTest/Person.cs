using System;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

public class Person
{
    public string Name { get; set; } = Guid.NewGuid().ToString("N");

    public int Id { get; set; }

    public override string ToString() => $"#{Id} - {Name}";
}