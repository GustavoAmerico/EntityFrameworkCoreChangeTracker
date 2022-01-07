using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest.Net5._0
{
    [TestClass]
    public class ObjectChangeTest
    {
        private Database _db;

        [TestMethod]
        public void RegisterAnUser()
        {
            var person = new Person();
            _db.People.Add(person);
            var all = _db.SaveChanges();
            Assert.IsTrue(all == 1);

            person.Name = "Gustavo Américo";
            _db.People.Update(person);
            all = _db.SaveChanges();
            Assert.IsTrue(all == 1);

            _db.People.Remove(person);
            all = _db.SaveChanges();
            Assert.IsTrue(all == 1);
        }

        [TestInitialize]
        public void Initialize()
        {
            _db = new Database(new[] { new EntityTracker<Person>(new[] { new Tracker() }) });
        }
    }
}
