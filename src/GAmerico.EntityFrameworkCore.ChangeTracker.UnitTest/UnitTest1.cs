using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Database _db;

        [TestMethod]
        public void RegisterAnUser()
        {
            var person = new Person();
            _db.People.Add(person);
            var all = _db.SaveChanges();

            Assert.IsTrue(all == 1);
        }

        [TestInitialize]
        public void Initialize()
        {
            _db = new Database(new[] { new EntityTracker<Person>(new[] { new Tracker() }) });
        }
    }


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