namespace Execution.Proxy
{
    /*
     * TODO
     * -----
     * You are given the Person class and asked to write a ResponsiblePerson proxy that does the following:
     * - Allows person to drink unless they are younger than 18 (in that case, retun "too young")
     * - Allows person to drive unless they are younger than 16 (otherwise, "too young")
     * - In case of driving while drinking, returns "dead"
     * 
     * */

    // my solution...
    public interface IPersonProxy
    {
        int Age { get; set; }
        string Drink();
        string Drive();
        string DrinkAndDrive();
    }
    public class PersonProxy : IPersonProxy
    {
        public int Age { get; set; }

        public string Drink()
        {
            return "drinking";
        }

        public string Drive()
        {
            return "driving";
        }

        public string DrinkAndDrive()
        {
            return "driving while drunk";
        }
    }

    public class ResponsiblePersonProxy : IPersonProxy
    {
        private PersonProxy person;
        private bool driving, drinking;

        public ResponsiblePersonProxy(PersonProxy person)
        {
            // todo
            this.person = person;
        }

        public int Age { get; set; }

        public string Drink()
        {
            var result = string.Empty;

            if (Age < 18)
                result = "too young";
            else
            {
                result = "drinking";
                drinking = true;
            }

            return result;
        }

        public string Drive()
        {
            var result = string.Empty;

            if (Age < 16)
                result = "too young";
            else
            {
                result = "driving";
                driving = true;
            }

            return result;
        }

        public string DrinkAndDrive()
        {
            return "dead";
        }
    }

    class ProxyExercise
    {
    }
}