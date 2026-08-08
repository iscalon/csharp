using MaConsoleApp;
using StringUtil = MaConsoleApp.utils.Util; // On peut mettre un alias à la classe 'Util' importée
using DoubleArray = double[]; // On peut même mettre un alias sur les tableaux de double.

// Version : p.124

static class Program
{

    static int FeetToInches(int mesureInFeet)
    {
        return (int)new UnitConverter(12).Convert(mesureInFeet);
    }

    private static void Main()
    {
        const int a = 5;
        const int b = 6;
        const int sum = a + b;
        DoubleArray array = [a, b, sum];
        double[] tableau = [sum, .. array];
        Console.WriteLine(string.Join(", ", tableau));
        Console.WriteLine($"Somme de {array[0]} et {array[1]} = {array[2]}");
        Console.WriteLine($"Hello, World! {FeetToInches(sum)}");
        Console.WriteLine("Hello\nWorld!");
        Console.WriteLine(@"Hello\nWorld! {FeetToInches(sum)} pure");
        Console.WriteLine($"255 in hex is {byte.MaxValue:X2}"); // X2 = 2-digit hexadecimal
        Console.WriteLine($"77 in hex is {77:X2}"); // X2 = 2-digit hexadecimal
        Console.WriteLine($$"""{ "TimeStamp": "{{DateTime.Now}}" }"""); // 2 symboles $ = il faut interpoler 2 accolades. Va donner : { "TimeStamp": "07/07/2026 16:46:24" }
        Console.WriteLine(new Panda("Pandi", "Panda").ToString());
        Panda pandAndi = new ("Pandi McDonald"); // On peut omettre le nom de la classe après le new, il est inféré
        Console.WriteLine(pandAndi.ToString());
        // Déconstruisons ce panda
        (_, string firstName, string lastName) = pandAndi;
        Console.WriteLine($"X. {firstName} {lastName}");

        Console.WriteLine(CardGame.GetCardNameFor(77, "hearts")); // "Joker"
        Console.WriteLine(CardGame.GetCardNameFor(12)); // "Queen of spades"

        // Object Initializers (besoin que les champs soient 'public' et pas readonly)
        // Note parameterless constructors can omit empty parentheses
        Bunny b1 = new () { Name = "Bo", LikesCarrots = true, LikesHumans = false }; // Appel constructeur par défaut
        Bunny b2 = new ("Bo") { LikesCarrots = true, LikesHumans = false }; // Appel constructeur prennant une string en paramètre
        // Versus
        // Optional Parameters
        Bunny b3 = new (name: "Bby", likesCarrots: true);
        Console.WriteLine($"Bunnies : {b1}\n{b2}\n{b3}");

        var note = new Note("C#") { Pitch = 50 };
        Console.WriteLine($"{note.GetName()}={note.Description} : {note.Pitch}");
        // note.Pitch = 10; // Erreur
        var note2 = new Note("G") { Pitch = 40, Description = "sol"};
        Console.WriteLine($"{note2.GetName()}={note2.Description} : {note2.Pitch}");

        var phrase = new Phrase("Le hamster passe sa vie à stocker");
        Console.WriteLine(phrase[1]); // "hamster"
        phrase[0] = "La";
        phrase[1] = "fourmi";
        Console.WriteLine(phrase); // "La fourmi passe sa vie à stocker"
        phrase[3, '*'] = "son";
        Console.WriteLine(phrase); // "La fourmi passe son* vie à stocker"
        Console.WriteLine(phrase[^1]); // "stocker"
        Console.WriteLine(string.Join(" ", phrase[2..5])); // "passe son* vie"
    }
}


namespace MaConsoleApp
{

    public class UnitConverter(double ratio)
    {
        public double Convert(double value)
        {
            return ratio * value;
        }
    }

    public class Panda
    {

        private readonly /* equivalent de final en Java */ string title;
        private readonly FullName fullName;


        public Panda(string fullName) : this(fullName, "")
        {
            Console.WriteLine("1er constructeur !");
        }


        public Panda(String firstName, String lastName, String title = "M." /* valeur optionnelle */)
        {
            // "Destructuring assignment" pour simplifier légèrement l'écriture du constructeur
            (this.title, this.fullName) = (title, new FullName
            {
                firstName = firstName,
                lastName = lastName
            });
        }


        public String GetName()
        {
            string fullName = this.fullName.firstName + " " + this.fullName.lastName;
            StringUtil.Split(fullName, out string fName, out string lName); // Déclaration de fName et lName en même temps que l'appel
            // Si on se fichait de la 2ème chaine par exemple, on pouvait faire : Util.Split(fullName, out string fName, out _)
            return this.title + " " + fName + " " + lName;
        }

        override public String ToString()
        {
            return GetName();
        }

        public void Deconstruct(out string title, out string firstName, out string lastName)
        {
            title = this.title;
            firstName = this.fullName.firstName;
            lastName = this.fullName.lastName;
        }
    }

    struct FullName { public String firstName; public String lastName; }


    public class Bunny(string name,
          bool likesCarrots = false,
          bool likesHumans = false)
    {
        public string Name = name;
        public bool LikesCarrots = likesCarrots, LikesHumans = likesHumans;
 
        public Bunny() : this("") {}
        public Bunny(string n) : this(n, false, false) {}

        public override string ToString()
        {
            return $"{name}, likes carrots : {LikesCarrots}, likes humans : {LikesHumans}";
        }
    }

    public class CardGame
    {
        private const string SPADES_SUITE = "spades";
        private const string DEFAULT_SUITE = SPADES_SUITE;

        public static string GetCardNameFor(int value, string suite = DEFAULT_SUITE) => (value, suite) switch // On peut faire des 'switch expressions' sur des tuples
        {
            (13, SPADES_SUITE) => "King of spades",
            (12, SPADES_SUITE) => "Queen of spades",
            (11, SPADES_SUITE) => "Jack of spades",
            _ => "Joker"
        };
    }

    public class Note(string name)
    {
        public int Pitch { get; init; } = 20;   // “Init-only” property C#9
        public int Duration { get; init; } = 100;  // “Init-only” property

        private readonly string name = name;
        private readonly string _description;

        public string Description { get => _description; init => _description = value; }



        public string GetName()
        {
            return name;
        }
    }


    public class Phrase(string valeur)
    {

        // Définition de l'index, ici ça retournera/modifiera un mot de la phrase
        // On peut mettre plusieurs argument, dont des arguments par défaut
        public string this[int indexMot, char special = '\0']
        {
            get => valeur.Split()[indexMot];
            set {
                string[] temp = valeur.Split();
                temp[indexMot] = value + special;
                valeur = string.Join(" ", temp);
            }
        }

        // Ajout des indices et intervalles
        // Ex: phrase[^1] retourne le dernier mot
        public string this[Index index] => valeur.Split()[index];
        // Ex: phrase[..2] retourne les 2 premiers mots (indice de fin exclusif)
        public string[] this[Range range] => valeur.Split()[range];

        public override string ToString()
        {
            return valeur;
        }
    }
}