namespace MaConsoleApp.utils.anonymous;

internal class AnonymousType {

    public static void Test() {
        // On ne peut définir une variable avec un type anonyme qu'avec le mot clé 'var'
        var dude = new { Age = 27, Nom = "Didier" };
        var dude2 = new { Age = 27, Nom = "Didier" };

        Console.WriteLine(dude);
        Console.WriteLine(dude.GetType() == dude2.GetType()); // True
        Console.WriteLine(dude.Equals(dude2)); // True
        Console.WriteLine(dude == dude2); // False

        string Nom = "John";
        var dude3 = new { Age = 28, Nom, Nom.Length }; // Equivalent de { Age = 28, Nom = Nom, Length = Nom.Length }
        Console.WriteLine(dude3); // { Age = 28, Nom = "John", Length = 4 }

        // Les types anonymes sont immuables, mais depuis C#10 on peut utiliser la 'nondetructive mutation' :
        var dude4 = dude3 with { Age = 20 };
        Console.WriteLine(dude4); // { Age = 20, Nom = "John", Length = 4 }

        var dudes = new[] { dude3, dude4 };
        Console.WriteLine(dudes); // <>f__AnonymousType1`3[System.Int32,System.String,System.Int32][]
    }
}