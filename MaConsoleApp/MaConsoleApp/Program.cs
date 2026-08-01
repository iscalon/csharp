using MaConsoleApp;

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
        Console.WriteLine(new Panda("Pandi").ToString());
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

    public class Panda(String name)
    {

        public String GetName()
        {
            return name;
        }

        override public String ToString()
        {
            return GetName();
        }
    }
}