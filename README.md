# Entity Framework Change Tracker

This package adds objects to track changes made to certain types of objects. The goal is to identify the properties that have changed on each object attached to DBContext.


# How do I use

```CSharp

   //Define de trackable types
   public static IServiceCollection ConfigureEntityLoggerBuilder(this IServiceCollection services)
    {
      services
        .AddEntityFrameworkTracker<MyEntity01>()
        .AddEntityFrameworkTracker<MyEntity02>()
        .AddScoped<MyLogger>()        
        .AddTrackObserver<MyEntity01>(p => p.GetService<MyLogger>())
        .AddTrackObserver<MyEntity02>(p => p.GetService<MyLogger>());

      return services;
    }

```

```CSharp

    internal class MyLogger : IObserver<ObjectChanged<MyEntity01>>, IObserver<ObjectChanged<MyEntity02>>
    {

     
        public void OnNext(ObjectChanged<MyEntity01> value)
        {

        //Write your code 

        }

         
        public void OnNext(ObjectChanged<MyEntity02> value)
        {
        //Write your code 
        }

        public void OnError(Exception error)
        {
        //Write your code 
		}
    }

```