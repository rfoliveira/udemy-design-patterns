namespace Execution.Decorator
{
    public class BirdExercise
    {
        public int Age { get; set; }

        public string Fly()
        {
            return (Age < 10) ? "flying" : "too old";
        }
    }

    public class LizardExercise
    {
        public int Age { get; set; }

        public string Crawl()
        {
            return (Age > 1) ? "crawling" : "too young";
        }
    }

    public class DragonExercise // no need for interfaces
    {
        private readonly BirdExercise bird = new BirdExercise();
        private readonly LizardExercise lizard = new LizardExercise();

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                bird.Age = value;
                lizard.Age = value;
            }
        }

        public string Fly()
        {
            // todo
            return bird.Fly();
        }

        public string Crawl()
        {
            // todo
            return lizard.Crawl();
        }
    }

    class DecoratorExercise
    {
    }
}
