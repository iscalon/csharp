namespace MaConsoleApp.utils.generiques {
    internal class Generique {

        public static T Max<T>(T a, T b) where T : IComparable<T> {
            return a.CompareTo(b) > 0 ? a : b;
        }

        public class Bob<T> { public static int count; }
        // Les attributs statiques sont uniques pour chaque type fermé. (Bob<T> : 'T' ouvert, Bob<int> : 'int' fermé)
        // Console.WriteLine(++Bob<int>.Count);     // 1
        // Console.WriteLine(++Bob<int>.Count);     // 2
        // Console.WriteLine(++Bob<string>.Count);  // 1
        // Console.WriteLine(++Bob<object>.Count);  // 1

        /*
            where T : base-class   // Base-class constraint
            where T : interface    // Interface constraint
            where T : class        // Reference-type constraint
            where T : class?       // "Nullable Reference Types"
            where T : struct       // Value-type constraint (excludes Nullable types)
            where T : unmanaged    // Unmanaged constraint (is a stronger version of a struct constraint: T must be a simple value type or a struct that is (recursively) free of any reference types, ex : Buffer<int> b1 = new() est OK, Buffer<double> b2 = new() est OK, mais pas Buffer<string> b3 = new())
            where T : new()        // Parameterless constructor constraint (un constructeur par défaut doit exister sur le type)
            where U : T            // Naked type constraint (U doit être un sous-type de T, ex : DoSomething<T, U>, ok pour DoSomething<Animal, Dog>, mais ko pour DoSomething<Dog, Animal>)
            where T : notnull      // Non-nullable value type, or (from C# 8) a non-nullable reference type
         */

        /* Exemple :
         * class Something<T>
                 where T : Animal, IRunnable, new() {
                    public void DoStuff() {
                        T animal = new T();
                        animal.Eat();
                        animal.Run();
                    }
           }
         */
    }

    interface IAnimal {

        string GetName();
    }

    interface IWasheable<out T> where T: IAnimal {

        void WashMe() {
            Console.WriteLine("Je suis en train d'être lavé");
        }

        T Me();
    }

    class Bear : IAnimal, IWasheable<Bear> {

        public virtual string GetName() {
            return "Ours";
        }

        public virtual Bear Me() {
            return this;
        }
    }

    class Grizzly : Bear, IWasheable<Grizzly> {

        public override string GetName() {
            return "Grizzly";
        }

        public override Grizzly Me() {
            return this;
        }
    }

    class Camel : IAnimal, IWasheable<Camel> {

        public string GetName() {
            return "Chameau";
        }

        public Camel Me() {
            return this;
        }
    }

    class Zoo {


        public static void WashNoCovariance(List<IAnimal> animals) {
            foreach (var animal in animals) {
                Console.WriteLine($"On lave : { animal.GetName() }");
            }
        }

        public static void Wash<T>(List<T> animals) where T : IAnimal {
            foreach (var animal in animals) {
                Console.WriteLine($"On lave : { animal.GetName() }");
            }
        }

        public static void WashCovariantAvecList(/* List<U> est invariant */ List<IWasheable<IAnimal>> animals) {
            foreach (var animal in animals) {
                Console.WriteLine($"On lave : { animal.Me().GetName() }");
            }
        }


        public static void WashFullCovariant(/* IEnumerable<out U> est covariant */ IEnumerable<IWasheable<IAnimal>> animals) {
            foreach (var animal in animals) {
                Console.WriteLine($"On lave : { animal.Me().GetName() }");
            }
        }

        public static void ZooTestNoCovariance() {
            Console.WriteLine("<========= Test méthode sans covariance =========");
            List<IAnimal> animals = [new Camel(), new Bear(), new Grizzly()];
            WashNoCovariance(animals);

            List<Bear> bears = [new Bear(), new Bear(), new Grizzly()];
            // WashNoCovariance(bears); // Erreur de compilation. Car le compilateur empêche de pouvoir ajouter d'autres animaux qui ne sont pas des ours à la liste passée à WashNoCovariance.
            Console.WriteLine("========= Fin test méthode sans covariance =========>");
        }

        public static void ZooTest() {
            Console.WriteLine("<========= Test méthode sans covariance, mais avec contrainte sur <T> =========");
            List<IAnimal> animals = [new Camel(), new Bear(), new Grizzly()];
            Wash(animals);

            List<Bear> bears = [new Bear(), new Bear(), new Grizzly()];
            Wash(bears);
            Console.WriteLine("========= Fin test méthode sans covariance, mais avec contrainte sur <T> =========>");
        }

        public static void ZooTestCovariant() {
            Console.WriteLine("<========= Test méthode avec covariance mais sur une List =========");
            List<IWasheable<IAnimal>> animals = [new Camel(), new Bear(), new Grizzly()];
            WashCovariantAvecList(animals);
            Console.WriteLine("========= Fin test méthode avec covariance mais sur une List =========>");

            Console.WriteLine("<========= Test méthode avec covariance avec un IEnumerable =========");
            WashFullCovariant(animals);

            List<IWasheable<Bear>> bears = [new Bear(), new Bear(), new Grizzly()];
            // WashCovariantAvecList(bears); // Ne compile par car List<U> en C# est invariant, pour empêcher d'ajouter des animaux qui ne sont pas des ours à notre liste d'ours,
            // donc pas de covariance possible. Il faut utiliser plutôt dans la méthode WashXXXX l'interface IEnumerable<out U> qui permet la covariance.

            WashFullCovariant(bears); // Là, avec IEnumerable comme paramètre, ça devient possible.
            Console.WriteLine("========= Fin test méthode avec covariance avec un IEnumerable =========>");
        }
    }


}
