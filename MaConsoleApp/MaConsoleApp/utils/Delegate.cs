namespace MaConsoleApp.utils.delegates;

class Calcul {

    internal int Start { get; init; }

    public static int Square(int x) {
        return (int)Math.Pow(x, 2);
    }

    public static int Cube(int x) {
        return (int) Math.Pow(x, 3);
    }

    public int AddToStartValue(int value) {
        return Start + value;
    }
}


delegate int Transformer(int x);


class CalculTest {

    public static void TestStaticDelegate() {
        Transformer(7, Calcul.Square); // 49
        Transformer(7, Calcul.Cube);   // 343
    }

    public static void TestInstanceDelegate() {
        Calcul calcul = new() { Start = 5 };
        Transformer(7, calcul.AddToStartValue); // 12
    }

    public static void Transformer(int x, Transformer transformer) {
        Console.WriteLine($"{x} transformé par { transformer.Method } devient : { transformer(x) }");
    }
}
