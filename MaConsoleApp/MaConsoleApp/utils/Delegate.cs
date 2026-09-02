namespace MaConsoleApp.utils.delegates;

class Calcul {

    internal int Start { get; init; }

    public static int Square(int x) {
        return (int)Math.Pow(x, 2);
    }

    public static int Cube(int x) {
        return (int) Math.Pow(x, 3);
    }

    public static decimal Cube(decimal x) {
        return (decimal) Math.Pow((double) x, 3);
    }

    public int AddToStartValue(int value) {
        return Start + value;
    }
}


delegate int Transformer(int x);

delegate T TransformerGenerique<T>(T x);

/*
 * Mais il existe déjà des délégués prédéfinis en C#, comme Func et Action : 
 * 
 * delegate TResult Func <out TResult>                ();
 * delegate TResult Func <in T, out TResult>          (T arg);
 * delegate TResult Func <in T1, in T2, out TResult>  (T1 arg1, T2 arg2);
 * ... and so on, up to T16
 * 
 * ou
 * 
 * delegate void Action                 ();
 * delegate void Action <in T>          (T arg);
 * delegate void Action <in T1, in T2>  (T1 arg1, T2 arg2);
 * ... and so on, up to T16
 */


class CalculTest {

    public static void TestStaticDelegate() {
        Transformer(7, Calcul.Square); // 49
        Transformer(7, Calcul.Cube);   // 343
        TransformerGenerique(7, Calcul.Cube);   // 343
        TransformerGenerique(9m, Calcul.Cube);   // 729
    }

    public static void TestInstanceDelegate() {
        Calcul calcul = new() { Start = 5 };
        Transformer(7, calcul.AddToStartValue); // 12
    }

    public static void Transformer(int x, Transformer transformer) {
        Console.WriteLine($"{x} transformé par { transformer.Method } devient : { transformer(x) }");
    }

    public static void TransformerGenerique<T>(T x, TransformerGenerique<T> transformer) {
        Console.WriteLine($"{x} transformé (generique) par { transformer.Method } devient : { transformer(x) }");
    }
}
