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
}
