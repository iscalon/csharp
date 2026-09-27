namespace MaConsoleApp.operator_overloading;

internal class OperatorsOverloading {

    class Note(int semiTones) : IComparable<Note> {

        public int CompareTo(Note? other) {
            if(other == null) return 1;
            if(this == other) return 0;

            return this.GetSemiTones().CompareTo(other.GetSemiTones());
        }

        public int GetSemiTones() {
            return semiTones;
        }

        public static Note operator + (Note x, int semiTones) {
            return new Note(x.GetSemiTones() + semiTones);
        }

        public static bool operator < (Note a, Note b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator > (Note a, Note b) {
            return a.CompareTo(b) > 0;
        }


        // ========= Implicit & Explicit conversions (ne sont pas prise en compte pour les opérateurs 'is' et 'as', ex:  '(554.37 is Note)' // false ou 'Note n = 554.37 as Note;' // erreur
        // Convert to hertz
        public static implicit operator double(Note x)
          => 440 * Math.Pow(2, (double)x.GetSemiTones() / 12);

        // Convert from hertz (accurate to the nearest semitone)
        public static explicit operator Note(double x)
          => new ((int)(0.5 + 12 * (Math.Log(x / 440) / Math.Log(2))));
    }

    public static void Test() {
        Note A = new (0);
        Note B = A + 2;
        Note Csharp = B + 2;
        Console.WriteLine($"B + 2 semi tones => C# : {Csharp.GetSemiTones()}"); // C# = 3 semi tones
        Console.WriteLine($"B < C# : {B < Csharp}"); // true

        Note n = (Note) 554.37;    // explicit conversion
        double freq = n;           // implicit conversion
        Console.WriteLine($"Note n : {n.GetSemiTones()}, frequence de n : {freq:F2}"); // ":F2" pour afficher seulement 2 décimales.
    }
}
