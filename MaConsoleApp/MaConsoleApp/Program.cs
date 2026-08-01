using MaConsoleApp;

// Version : p.82

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
        Console.WriteLine($"Hello, World! {FeetToInches(sum)}");
        Console.WriteLine("Hello\nWorld!");
        Console.WriteLine(@"Hello\nWorld! {FeetToInches(sum)} pure");
        Console.WriteLine($"255 in hex is {byte.MaxValue:X2}"); // X2 = 2-digit hexadecimal
        Console.WriteLine($"77 in hex is {77:X2}"); // X2 = 2-digit hexadecimal
        Console.WriteLine($$"""{ "TimeStamp": "{{DateTime.Now}}" }"""); // 2 symboles $ = il faut interpoler 2 accolades. Va donner : { "TimeStamp": "07/07/2026 16:46:24" }
        Console.WriteLine(new Panda("Pandi", "Panda").ToString());
        Console.WriteLine(new Panda("Pandi McDonald").ToString());
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


        public Panda(string fullName) : this(fullName, "") {
            Console.WriteLine("1er constructeur !"); // Appelé avant d'appeler l'autre constructeur
        }


        public Panda(String firstName, String lastName, String title = "M." /* valeur optionnelle */)
        {
            this.title = title;
            this.fullName = new FullName
            {
                firstName = firstName,
                lastName = lastName
            };
        }


        public String GetName()
        {
            string fullName = this.fullName.firstName + " " + this.fullName.lastName;
            Util.Split(fullName, out string fName, out string lName); // Déclaration de fName et lName en même temps que l'appel
            // Si on se fichait de la 2ème chaine par exemple, on pouvait faire : Util.Split(fullName, out string fName, out _)
            return this.title + " " + fName + " " + lName;
        }

        override public String ToString()
        {
            return GetName();
        }


        
    }

    struct FullName { public String firstName; public String lastName; }

    public class Util
    {
        public static void Split(string fullName, out string firstName, out string lastName)
        {
            string[] parts = fullName.Split(' ');
            firstName = parts[0];
            lastName = "";
            if (parts.Length >= 2)
            {
                lastName = parts[1];
            }
        }
    }

}