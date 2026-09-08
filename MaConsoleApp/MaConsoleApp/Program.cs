using MaConsoleApp;
using MaConsoleApp.events;
using MaConsoleApp.Heritage;
using MaConsoleApp.lambda;
using MaConsoleApp.partial;
using MaConsoleApp.utils;
using MaConsoleApp.utils.anonymous;
using MaConsoleApp.utils.delegates;
using MaConsoleApp.utils.enumerations;
using MaConsoleApp.utils.generiques;
using MaConsoleApp.utils.iterators;
using MaConsoleApp.utils.records;
using MaConsoleApp.utils.tuples;
using DoubleArray = double[]; // On peut même mettre un alias sur les tableaux de double.
using StringUtil = MaConsoleApp.utils.Util; // On peut mettre un alias à la classe 'Util' importée

// Version : p.238

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
        Bunny b1 = new (){ Name = "Bo", LikesCarrots = true, LikesHumans = false }; // Appel constructeur par défaut
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

        PaymentService paiements = new ();
        paiements.Payer(7.52m);

        TestInterface.Test();

        BorderSidesTest.Ecrire(BorderSides.Left);
        // BorderSidesTest.Ecrire((BorderSides) 18); // Unhandled exception. System.ArgumentException: On ne sait pas afficher le côté : 18 (Parameter 'side')

        Zoo.ZooTestNoCovariance();
        Zoo.ZooTest();
        Zoo.ZooTestCovariant();

        CalculTest.TestStaticDelegate();
        CalculTest.TestInstanceDelegate();

        StockEventTest.Test();

        LambdaTest.Test();

        TestIterateurs.Test();

        ExtensionMethod.Test();

        AnonymousType.Test();

        Tuples.Test();

        Record.Test();
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
        // On peut mettre plusieurs arguments, dont des arguments par défaut
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


namespace MaConsoleApp.Heritage {

    using static IUndoable;

    public class BaseClass {
        public virtual void Foo() { Console.WriteLine("BaseClass.Foo"); }

        public virtual void ImpossibleARedefinir() {
            Console.WriteLine("Je peux être redéfinie et je peux être 'hidden'");
        }
    }

    public class Overrider : BaseClass {
        public override void Foo() { Console.WriteLine("Overrider.Foo"); }

        public override sealed void ImpossibleARedefinir() {
            Console.WriteLine("Je ne peux pas être redéfinie (car je suis 'sealed') mais je peux être 'hidden'");
        }
    }

    public class Hider : BaseClass {
        public new void Foo() { Console.WriteLine("Hider.Foo"); } // 'new' pour dire qu'on masque intentionnellement la méthode Foo de la classe de base


    }

    public class Hidoverrider : Overrider {
        public override void Foo() { Console.WriteLine("Hidoverrider.Foo"); }

        public new void ImpossibleARedefinir() {
            Console.WriteLine("Je ne suis pas redéfinie (car je suis 'new') et je suis 'hidden'");
        }
    }

    //Overrider over = new Overrider();
    //BaseClass b1 = over;
    //over.Foo();                         // Overrider.Foo
    //b1.Foo();                           // Overrider.Foo
    //Hider h = new Hider();
    //BaseClass b2 = h;
    //h.Foo();                           // Hider.Foo
    //b2.Foo();                          // BaseClass.Foo
    //Hidoverrider ho = new();
    //Overrider o2 = ho;
    //BaseClass base3 = ho;
    //ho.ImpossibleARedefinir();    // Je ne suis pas redéfinie(car je suis 'new') et je suis 'hidden'
    //o2.ImpossibleARedefinir();    // Je ne peux pas être redéfinie(car je suis 'sealed') mais je peux être 'hidden'
    //base3.ImpossibleARedefinir(); // Je ne peux pas être redéfinie(car je suis 'sealed') mais je peux être 'hidden'

    public interface IUndoable {

        internal static readonly string UNDO = "Undo";
        internal static readonly string REDO = "Redo";

        void Undo();

        void Redo() {
            Console.WriteLine($"Performing : { REDO }");
        }
    }

    public class TextBox : IUndoable {
        public virtual void Undo() => Console.WriteLine($"TextBox.{ UNDO }"); // Il faut mettre 'virtual' sinon c'est 'sealed' par défaut.
    }

    public class RichTextBox : TextBox {
        public override void Undo() => Console.WriteLine($"RichTextBox.{ UNDO /* On peut utilise 'UNDO' sans préfixer par le nom de l'interface grâce au 'using static' au début du namespace */}");
    }

    public interface IUndoable2 { void Undo(); }

    public class TextBox2 : IUndoable2 {

        void IUndoable2.Undo() => Console.WriteLine("TextBox2.Undo"); // On peut préciser explicitement ce qu'on implémente, pas de 'public' => 'private'.

        protected void PMethod() {
            Console.WriteLine("protected method");
        }
    }

    public class RichTextBox2 : TextBox2, IUndoable2 {

        public void Undo() {
            // base.PMethod(); // OK
            // base.Undo();    // KO
            Console.WriteLine("RichTextBox2.Undo");
        }
    }

    public class TextBox3 : IUndoable2 {
        public void Undo() => Console.WriteLine("TextBox3.Undo"); // 'sealed' par défaut.
    }

    public class RichTextBox3 : TextBox3, IUndoable2 {
        public void Undo() => Console.WriteLine("RichTextBox3.Undo"); // Masque TextBox3.Undo()
    }

    public class TestInterface {

        public static void Test() {
            RichTextBox rrr = new();
            rrr.Undo();                 // RichTextBox.Undo
            ((IUndoable)rrr).Undo();    // RichTextBox.Undo
            ((TextBox)rrr).Undo();      // RichTextBox.Undo

            RichTextBox2 r = new ();
            r.Undo();                   // RichTextBox2.Undo
            ((IUndoable2)r).Undo();     // RichTextBox2.Undo
            //((TextBox2)r).Undo();     // Pas possible car Undo private dans TextBox2

            RichTextBox3 rr = new ();
            rr.Undo();                  // RichTextBox3.Undo
            ((IUndoable2)rr).Undo();    // RichTextBox3.Undo
            ((TextBox3)rr).Undo();      // TextBox3.Undo
        }
    }

    // Static virtual/abstract interface members depuis C# 11
    interface ITypeDescribable {
        static abstract string Description { get; } // Propriété en lecture seule
        static virtual string GetCategory() => "";  // Méthode
    }

    class CustomerTest : ITypeDescribable {
        public static string Description => "Customer tests";       // Obligatoirement à implémenter (forcément 'sealed' car statique)
        public static string GetCategory() => "Unit testing";       // Redéfinition optionnelle
    }

    // ITypeDescribable.GetCategory(); // KO car un membre statique ou abstrait n'est disponible que sur un paramètre de type.
}