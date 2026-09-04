namespace MaConsoleApp.utils.iterators;

internal class TestIterateurs {

    public static void Test() {
        Console.WriteLine("Suite de Fibonacci :");
        foreach(int terme in Fibonacci(6)) {
            Console.Write($"{terme} ");
        }
        Console.WriteLine();

        foreach(string valeur in Foo()) {
            Console.WriteLine(valeur);
        }

        foreach (string valeur in Foo(true)) {
            Console.WriteLine(valeur);
        }

        string? firstElement = null;
        var sequence = TryFoo();
        using var enumerator = sequence.GetEnumerator();
        if (enumerator.MoveNext()) {
            firstElement = enumerator.Current;
        }
        Console.WriteLine(firstElement);

        // On peut composer les itérateurs
        Console.WriteLine(
            $"Les termes pairs de la suite de Fibonnacci sont : { string.Join(" ", ConserverSeulementNombresPairs(Fibonacci(10))) }"); // 2, 8 et 34


        /**
         *      Consommateur                 Enumerateur conservant valeurs paires             Enumerateur des termes de la suite de Fibonacci 
         *      ------------                 -------------------------------------             -----------------------------------------------
         *           |                                        |                                                        |
         *           | ------- Next -------------------->     |                                                        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  1  ----------------------        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  1  ----------------------        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  2  ----------------------        |
         *           | <-------------- 2 ----------------     |                                                        |
         *           | ------- Next -------------------->     |                                                        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  3  ----------------------        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  5  ----------------------        |
         *           |                                        |   ----------------- Next --------------------->        |
         *           |                                        |                                                        |
         *           |                                        |   <-----------------  8  ----------------------        |
         *           | <-------------- 8 ----------------     |                                                        |
         *           |                                        |                                                        |
         * 
         */
    }

    static IEnumerable<int> Fibonacci(int n) {
        for (int i = 0, prev = 1, current = 1; i < n; i++) {
            // L'instruction : 'yield return' retourne la valeur en cours, n'attend pas d'avoir calculé toutes les valeurs.
            // C'est cette valeur qui sera consommée par le 'foreach' plus haut.
            // Idéal pour créer des énumérateurs.
            yield return prev;
            // Puis se met en pause et reprendra à l'instruction ci-dessous une fois que l'appelant demandera le prochain élément, et ainsi de suite.

            int next = prev + current;
            prev = current;
            current = next;
        }
    }

    // On peut mettre plusieurs instructions : 'yield return' dans une méthode (ou propriété, ou indexer).
    static IEnumerable<string> Foo() {
        yield return "One";
        yield return "Two";
        yield return "Three";
    }

    // On ne peut pas utiliser l'instruction 'return' seule dans un block d'itérateur.
    // Si on doit sortir avant la fin, il faut utiliser : 'yield break'
    static IEnumerable<string> Foo(bool breakEarly) {
        yield return "One";
        yield return "Two";
        if (breakEarly) {
            yield break;
        }
        yield return "Three";
    }

    // On peut utiliser 'yield return' dans les try/finally (mais sans catch).
    // C'est pour nettoyer les ressources dans le cas où on n'aurait pas fini l'énumération.
    // TryFoo pourra être appelé avec le mot clé 'using'.
    static IEnumerable<string> TryFoo() {
        try { yield return "One"; }
        finally { Console.WriteLine("clean up"); }
    }

    static IEnumerable<int> ConserverSeulementNombresPairs(IEnumerable<int> sequence) {
        foreach (int x in sequence) {
            if ((x % 2) == 0) {
                yield return x;
            }
        }
    }

}