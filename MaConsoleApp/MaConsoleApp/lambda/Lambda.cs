namespace MaConsoleApp.lambda;


class LambdaTest {

    public static void Test() {
        Func<string, string, int> totalLength = (s1, s2) => s1.Length + s2.Length;
        int total = totalLength("hello", "world");   // total is 10;
        Console.WriteLine($"Nombre de caractères dans 'hello' et dans 'word' : {total}");
        Test2();
    }

    static void Test2() {
        int factor = 2;
        Func<int, int> multiplier = n => n * factor; // 'factor' est capturée dans la lambda (closure).
        factor = 10;
        Console.WriteLine(multiplier(3));           // 30

        Func<int> incr = Natural();
        Console.WriteLine(incr());      // 0
        Console.WriteLine(incr());      // 1

        Func<int, int> multiplierNoClosure = static n => n * 2; // 'static' devant une lambda permet d'optimiser la mémoire car on indique qu'on ne va pas capturer d'état.
        // Func<int, int> multiplierNoClosure = static n => n * factor;  // ne compile donc pas.

        Action[] actions = new Action[3];
        for (int i = 0; i < 3; i++) {
            actions[i] = () => Console.Write(i);
        }
        foreach (Action a in actions) a(); // 333
        Console.WriteLine();

        for (int i = 0; i < 3; i++) {
            int loopScopedi = i; // Créée à chaque itération, donc la closure capture une nouvelle valeur à chaque fois.
            actions[i] = () => Console.Write(loopScopedi);
        }
        foreach (Action a in actions) a(); // 012
        Console.WriteLine();
    }

    static Func<int> Natural() {
        int seed = 0;
        return () => seed++;      // Retourne une closure
    }
}
