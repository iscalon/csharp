namespace MaConsoleApp.utils.tuples;

internal class Tuples {
    
    public static void Test() {
        // Les tuples contrairement aux types anonymes sont muables (leurs propriétés sont accessibles via 'Item1', 'Item2' etc...,
        // et ont un type, donc une méthode peut retourner un tuple.
        var tuple = ("John", 27);
        (string, int) tuple2 = ("John", 27);
        Console.WriteLine(tuple); // (John, 27)
        Console.WriteLine(tuple.Equals(tuple2)); // True
        string Name = "David";
        tuple.Item1 = Name;
        Console.WriteLine(tuple.Equals(tuple2)); // False
        var tuple3 = (Name, 27);
        Console.WriteLine(tuple3.Name); // David

        // Déconstruction de tuple
        (string nom, int age) = tuple3;
        Console.WriteLine($"Nom = {nom}, Age = {age}");
    }
}
