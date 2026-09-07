namespace MaConsoleApp.utils;

static class ExtensionMethod {

    /**
     * Une méthode d'extension doit être une méthode statique
     * qui prend comme 1er argument (et le modifier 'this') le type dont on souhaite étendre une méthode,
     * et doit faire partie d'une classe statique.
     * (Fonctionne aussi avec les interfaces).
     */
    public static bool IsCapitalized(this string s) {
        if (string.IsNullOrWhiteSpace(s)) {
            return false;
        }
        return char.IsUpper(s[0]);
    }

    // 2ème façon d'écrire une méthode d'extension
    extension<T>(IEnumerable<T> sequence) {
        public T First() {
            foreach (T item in sequence) {
                return item;
            }
            throw new ArgumentException("La séquence est vide", nameof(sequence));
        }

        // Ajoutons une nouvelle méthode sur les IEnumerable
        public T Nth(int index) {
            if(index < 0) {
                throw new ArgumentException("L'index voulu doit être positif ou nul", nameof(index));
            }
            if(index > sequence.Count()) {
                throw new ArgumentException("L'index voulu dépasse la taille de la séquence", nameof(index));
            }
            int cpt = 0;
            foreach (T item in sequence) {
                if (cpt++ == index) {
                    return item;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }

    public static void Test() {
        string s = "Toto";
        Console.WriteLine(s.IsCapitalized()); // True
        Console.WriteLine(s.First()); // 'T'
        Console.WriteLine(s.Nth(2)); // 't'
    }
}