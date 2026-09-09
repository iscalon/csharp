namespace MaConsoleApp.utils.patterns;

internal class Patterns {


    public static void Test() {
        object obj = "Je suis une chaine";

        if (obj is string) {
            Console.WriteLine(obj + " de taille : " + ((string)obj).Length);
        }

        // ==== Type pattern
        if (obj is string chaine) {
            Console.WriteLine(chaine + " de taille : " + chaine.Length);
        }

        // ==== Property pattern
        if (obj is string { Length: 18 }) {
            Console.WriteLine(obj + " de taille : 18");
        }

        if (obj is string { Length: 18 } s
                                            && s.StartsWith("Je")) {
            Console.WriteLine(obj + " de taille : 18");
        }

        if (obj is string s2 and { Length: int l }
                                            && l == 18
                                            && s2.StartsWith("Je")) {
            Console.WriteLine(obj + " de taille : 18");
        }

        // Plus évolué :
        bool ShouldAllow(Uri uri) => uri switch {
            { Scheme: "http", Port: 80 } => true,
            { Scheme.Length: 4, Port: 80 } => true,
            { Scheme: "https", Port: 443 } => true,
            { Scheme: "ftp", Port: 21 } => true,
            { IsLoopback: true } => true,
            { Host.Length: < 1000, Port: > 0 } => true, // Avec properties imbriquées + relational pattern (v. plus bas)
            { Scheme: "http" } when string.IsNullOrWhiteSpace(uri.Query) => true, // Avec clause when
            Uri { Scheme: "ssh", Port: 24 } httpUri => httpUri.Host.Length < 1000, // Type (teste si 'uri' est de type 'Uri') + property pattern avec variable ('httpUri')
            { Scheme: "ssh", Port: 25, Host: string host } => host.Length < 1000, // Property pattern + introduction de variable au niveau de la propriété
            _ => false
        };

        // ==== Var pattern
        static bool IsJanetOrJohn(string nom) =>
            nom.ToUpper() is var upper &&
                        ("JANET".Equals(upper) || "JOHN".Equals(upper));

        // ==== Constant pattern
        object o = 3;
        if(o is 3) {
            Console.WriteLine("Je suis 3");
        }

        // ==== Relational pattern (utile avec les switch)
        decimal imcRef = 20m;
        var GetWeightCategory = (decimal imc) => { // Closure
            if (imc.Equals(imcRef)) {
                Console.WriteLine("Perfect !");
            }
            // Attention au type de la valeur avec laquelle 'imc' est comparé (ne pas oublier de mettre 'm' après les valeurs pour dire qu'on teste par rapport à un decimal)
            return imc switch {
                < 18.5m => "underweight", 
                < 25m => "normal",
                < 30m => "overweight",
                _ => "obese"
            };
        };
        Console.WriteLine(GetWeightCategory(20m)); // Perfect ! normal
        imcRef = 24m;
        Console.WriteLine(GetWeightCategory(20m)); // normal

        // ==== Pattern combinators
#pragma warning disable CS8321 // La fonction locale est déclarée mais jamais utilisée

        static bool IsJohnOrDavid(string name) => name.ToLower() is "john" or "david";
        static bool IsVoyelle(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y';
        static bool IsBetween1And9(int n) => n is >= 1 and <= 9;
        static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

#pragma warning restore CS8321 // La fonction locale est déclarée mais jamais utilisée

        object notString = 777;
        Console.WriteLine($"{notString} n'est pas une string ? : {notString.IsNotString()}");

        // ==== Positional pattern (tuple pattern est un cas spécifique de positional pattern)
        var p = new Point(2, 3);
        Console.WriteLine(p is (2, 3)); // True, grâce à la deconstruction du record et au tuple pattern
        Console.WriteLine(p is (var x, var y) && x == y); // False, on peut déconstruire tout en matchant
        
        // Type pattern + positional pattern
        static string Print(object obj) => obj switch {
            Point(0, 0) => "Empty point",
            Point(var x, var y) when x == y => "Diagonal",
            _ => throw new ArgumentException("On ne sait pas imprimer", nameof(obj))
        };
        Console.WriteLine(Print(new Point(4, 4))); // Diagonal

        // ==== List pattern (C#11)
        int[] numbers = [0, 1, 2, 3, 4];
        Console.WriteLine(numbers is [0, 1, 2, 3, 4]);                             // True
        Console.WriteLine(numbers is [0, 1, _, _, 4]);                             // True
        Console.WriteLine(numbers is [0, 1, int /* ou var */ t, 3, 4] && t > 1);   // True
        Console.WriteLine(numbers is [0, .., 4]);                                  // True
        Console.WriteLine(numbers is [0, .. var mid, 4] && mid.Contains(2));       // True
    }


    record Point(int X, int Y) {}
}

internal static class Extensions {

    extension(object o) {

        public bool IsNotString() {
            // Tips pour écrire plus joliment : "if (!(obj is string))", on peut faire :
            return o is not string;
        }
    }
}