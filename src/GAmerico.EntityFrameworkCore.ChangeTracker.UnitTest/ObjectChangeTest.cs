using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

[TestClass]
public class ObjectChangeTest
{
    private Database _db;
    private Tracker _tracker;
    [TestMethod]
    public void RegisterAnUser()
    {
        var person = new Person();
        _db.People.Add(person);
        var all = _db.SaveChanges();
        Assert.IsTrue(all == _tracker.OnNextCount);
        _tracker.ResetCount();

        person.Name = "Gustavo Américo";
        _db.People.Update(person);

        _db.People.Add(new Person() { Name = " Gustavo Américo II" });
        all = _db.SaveChanges();
        Assert.IsTrue(all == _tracker.OnNextCount);
        _tracker.ResetCount();

        _db.People.Remove(person);
        all = _db.SaveChanges();
        Assert.IsTrue(all == _tracker.OnNextCount);
    }

    [TestMethod]
    public void RegisterMultipleRows()
    {
        _tracker.ResetCount();
        _db.People.Add(new Person() { Name = "Gustavo Américo" });
        _db.People.Add(new Person() { Name = " Gustavo Américo II" });
        var all = _db.SaveChanges();
        Assert.IsTrue(all == _tracker.OnNextCount);
    }

    [TestInitialize]
    public void Initialize()
    {
        _tracker = new Tracker();
        _db = new Database(new[] { new EntityTracker<Person>(new[] { _tracker }) });
    }
}
