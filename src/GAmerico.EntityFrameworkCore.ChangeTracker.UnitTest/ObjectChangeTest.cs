using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

[TestClass]
public class ObjectChangeTest
{
    private Database _db;
    private Tracker _trackerAdding;
    [TestMethod]
    public void RegisterAnUser()
    {
        var person = new Person();
        _db.People.Add(person);
        var all = _db.SaveChanges();
        Assert.IsTrue(all == _trackerAdding.OnNextCount);
        _trackerAdding.ResetCount();

        person.Name = "Gustavo Américo";
        _db.People.Update(person);

        _db.People.Add(new Person() { Name = " Gustavo Américo II" });
        all = _db.SaveChanges();
        Assert.IsTrue(all == _trackerAdding.OnNextCount);
        _trackerAdding.ResetCount();

        _db.People.Remove(person);
        all = _db.SaveChanges();
        Assert.IsTrue(all == _trackerAdding.OnNextCount);
    }

    [TestMethod]
    public void RegisterMultipleRows()
    {
        _trackerAdding.ResetCount();
        _db.People.Add(new Person() { Name = "Gustavo Américo" });
        _db.People.Add(new Person() { Name = " Gustavo Américo II" });
        var all = _db.SaveChanges();
        Assert.IsTrue(all == _trackerAdding.OnNextCount);
    }

    [TestInitialize]
    public void Initialize()
    {
        _trackerAdding = new Tracker();
        _db = new Database(new[] { new EntityTracker<Person>(new[] { _trackerAdding }) });
    }
}
