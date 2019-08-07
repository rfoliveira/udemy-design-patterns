using Autofac;
using Execution.Adapter;
using Execution.Bridge;
using Execution.Builder;
using Execution.ChainOfResponsibility;
using Execution.Command;
using Execution.Composite;
using Execution.Decorator;
using Execution.Factory;
using Execution.Flyweight;
using Execution.Interpreter;
using Execution.Iterator;
using Execution.Prototype;
using Execution.Proxy;
using Execution.Singleton;
using System;
using System.Text;
using static Execution.Factory.HotDrinkMachine;
using static System.Console;

namespace Execution
{
    class Program
    {
        static void Main(string[] args)
        {
            //BuilderWithoutBuilder();
            //BuilderWithBuilder();
            //BuilderWithBuilderFluent();
            //BuilderWithFluentAndRecursiveGenerics();
            //BuilderWithFacetedBuilder();
            //BuilderExercise();

            //FactoryTests();
            //AbstractFactoryTest();
            //AbstractFactoryWithOCP();
            //FactoryExerciseTest();

            //PrototypeTest();
            //PrototypeTestWithCopyConstructor();
            //PrototypeCopyThroughSerialization();

            //MonoStateSingleonBizarreAlternative();

            //AdapterDemo1();
            //AdapterWithCaching();

            //BridgeDemo();
            //BridgeDemoWithContainer();

            //CompositeDemo();
            //CompositeNeuralNetworkDemos();

            //DecoratorDemos();

            //FlyweightDemos();
            //ProxyDemos();

            //ChainOfResponsibilityDemos();
            //CommandDemos();

            //InterpreterDemos();

            IteratorDemos();
        }

        private static void IteratorDemos()
        {
            IteratorExamples.Demo1();
        }

        private static void InterpreterDemos()
        {
            //InterpreterExamples.HandmadeInterpreter_Lex();
            //InterpreterExamples.HandmadeInterpreter_Parsing();
            InterpreterExamples.Exercise();
        }

        private static void CommandDemos()
        {
            //CommandExamples.Demo1();
            //CommandExamples.UndoOperations();
            Command.CommandExamples.Exercise();
        }

        private static void ChainOfResponsibilityDemos()
        {
            //ChainOfResponsabilityExamples.MethodChain();
            ChainOfResponsabilityExamples.BrokerChain();
        }

        private static void ProxyDemos()
        {
            //ProxyExamples.ProtetorProxy();
            ProxyExamples.PropertyProxy();
        }

        private static void FlyweightDemos()
        {
            //FlyweightExamples.SizeInBytes();
            //FlyweightExamples.TextFormatting();
            FlyweightExercise.Result();
        }

        private static void DecoratorDemos()
        {
            //DecoratorExamples.Demo1();
            //DecoratorExamples.AdapterDecoratorExample();
            //DecoratorExamples.MultipleInheritanceExample();
            //DecoratorExamples.DynamicDecoratorComposition();
            DecoratorExamples.StaticDecoratorComposition();
        }

        private static void CompositeNeuralNetworkDemos()
        {
            NeuralNetworksExample.Demo1();
        }

        private static void CompositeDemo()
        {
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new SquareComposite { Color = "Red" });
            drawing.Children.Add(new CircleComposite { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new CircleComposite { Color = "Blue" });
            group.Children.Add(new SquareComposite { Color = "Blue" });

            drawing.Children.Add(group);

            WriteLine(drawing);
        }

        private static void BridgeDemoWithContainer()
        {
            var cb = new ContainerBuilder();

            cb.RegisterType<VectorRenderer>()
                .As<IRenderer>()
                .SingleInstance();

            cb.Register((c, p) =>
                new Circle(
                    c.Resolve<IRenderer>(),
                    p.Positional<float>(0)
                )
            );

            using (var c = cb.Build())
            {
                var circle = c.Resolve<Circle>(
                    new PositionalParameter(0, 5.0f)
                );

                circle.Draw();
                circle.Resize(2.0f);
                circle.Draw();
            }
        }

        private static void BridgeDemo()
        {
            //IRenderer renderer = new RasterRenderer();
            IRenderer renderer = new VectorRenderer();
            var circle = new Circle(renderer, 5);

            circle.Draw();
            circle.Resize(2);
            circle.Draw();
        }

        private static void AdapterWithCaching()
        {
            AdapterExamples.DrawWithCaching();
            AdapterExamples.DrawWithCaching();
        }

        private static void AdapterDemo1()
        {
            // Adapter without caching...
            AdapterExamples.Draw();
            AdapterExamples.Draw();
        }

        private static void MonoStateSingleonBizarreAlternative()
        {
            var ceo = new CEO();
            ceo.Name = "Adam Smith";
            ceo.Age = 55;

            var ceo2 = new CEO();

            WriteLine(ceo2);
        }

        private static void PrototypeCopyThroughSerialization()
        {
            Foo foo = new Foo
            {
                Stuff = 42,
                Whatever = "abc"
            };

            //Foo foo2 = foo.DeepCopy();  // crashes without [Serializable]
            Foo foo2 = foo.DeepCopyXml();

            foo2.Whatever = "xyz";

            WriteLine(foo);
            WriteLine(foo2);
        }

        private static void PrototypeTestWithCopyConstructor()
        {
            var john = new Employee("John", new Address("Augusto Nunes", "Rio de Janeiro", "Brazil"));
            var chris = new Employee(john);

            chris.Name = "Chris";

            WriteLine(john);
            WriteLine(chris);
        }

        //private static void PrototypeTest()
        //{
        //    var john = new Person(new[] { "Rafael", "Oliveira" }, new Address("Rua Augusto Nunes", 221));
        //    var jane = (Person)john.Clone();

        //    jane.Address.HouseNumber = 123;

        //    // this doesn't work...
        //    // var jane = john;

        //    // but clone is typically shallow copy
        //    jane.Names[0] = "Jane";

        //    WriteLine(john);
        //    WriteLine(jane);
        //}

        private static void FactoryExerciseTest()
        {
            var personList = new[] { "Fulano", "Beltrano", "Ciclano" };
            var factory = new PersonFactory();

            foreach (var p in personList)
            {
                factory.CreatePerson(p);
            }

            factory.Show();
        }

        private static void AbstractFactoryWithOCP()
        {
            var machine = new HotDrinkMachineWithOCP();
            IHotDrink drink = machine.MakeDrink();
            drink.Consume();
        }

        private static void AbstractFactoryTest()
        {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(AvaliableDrink.Coffee, 100);
            drink.Consume();
        }

        private static void FactoryTests()
        {
            
            //var point = PointFactory.NewPolarPoint(1.0, Math.PI / 2);
            //WriteLine(point);

            // the above way is good but I can use Point with instantiation yet, like "var p = new Point(1.0, 2.0);....
            // to avoid this I need to use Point.Factory.NewPolarPoint(...)
            var p = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(p);
        }

        private static void BuilderExercise()
        {
            PrintMethodName("BuilderExercise");
            var cb = new CodeBuilder("Person").AddField("Name", "string").AddField("Age", "int");
            WriteLine(cb);
        }

        private static void BuilderWithFacetedBuilder()
        {
            PrintMethodName("BuilderWithFacetedBuilder");
            var pb = new PersonBuilder2();
            Person2 person = pb
                .Works
                    .At("SomeCompany")
                    .AsA("Programmer")
                    .Earning(7500)
                .Lives
                    .At("123 London Road")
                    .In("London")
                    .WithPostcode("Sw12Ac");

            WriteLine(person);
        }

        private static void BuilderWithFluentAndRecursiveGenerics()
        {
            PrintMethodName("BuilderWithFluentAndRecursiveGenerics");
            // FLuent Builder inheritance with recursive Generics...
            var p = Person.New
                .Called("Rafael")
                .WorksAsA("Developer")
                .Build();

            WriteLine(p);
        }

        private static void BuilderWithBuilderFluent()
        {
            PrintMethodName("BuilderWithBuilderFluent");
            var builder = new HtmlBuilder("ul");
            var words = new[] { "hello", "world" };

            builder.AddChild("li", words[0]).AddChild("li", words[1]);
            WriteLine(builder.ToString());
        }

        private static void BuilderWithBuilder()
        {
            PrintMethodName("BuilderWithBuilder");
            var builder = new HtmlBuilder("ul");
            var words = new[] { "hello", "world" };

            foreach (var word in words)
            {
                builder.AddChild("li", word);
            }

            WriteLine(builder.ToString());
        }

        private static void BuilderWithoutBuilder()
        {
            PrintMethodName("BuilderWithoutBuilder");
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            WriteLine(sb);

            var words = new[] { "hello", "world" };
            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", words);
            }
            sb.Append("</ul>");
            WriteLine(sb);
        }

        private static void PrintMethodName(string methodName)
        {
            WriteLine($"\n{methodName} ...");
        }
    }
}
