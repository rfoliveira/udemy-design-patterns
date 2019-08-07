using System;

namespace Execution.Singleton
{
    class SingletonExercise
    {
    }

    /*
     * Since implementing a singleton is easy, you have a different challenge: 
     * write a method called IsSingleton(). 
     * This method takes a factory method that returns an object and it's up to you to determine
     * whetheror not that object is a singleton instance
     * 
     * Obs.: It's ridiculous, but the site says that is right.
     * */
    public class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var obj1 = func();
            var obj2 = func();

            return obj1.Equals(obj2);
        }
    }        
}
