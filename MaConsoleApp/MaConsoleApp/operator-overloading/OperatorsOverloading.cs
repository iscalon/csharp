using System.Numerics;

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

    // ========= Redéfinition statique d'opérateur (le polymorphisme statique est possible en C#)
    interface IAddable<T> where T : IAddable<T> {
        abstract static T operator + (T x, T y);
    }

    record Point(int X, int Y) : IAddable<Point> {
        public static Point operator + (Point p1, Point p2) {
            return new(p1.X + p2.X, p1.Y + p2.Y);
        }
    }

    // NB : C# 7 a introduit INumber<T> pour faciliter les opération sur les numériques, ex : 
    //
    // T Sum<T>(params T[] numbers) where T : INumber<T> {
    //   T total = T.Zero;
    //   foreach (T n in numbers) {
    //       total += n;      // Invokes addition operator for any numeric type
    //   }
    //   return total;
    // }
    //
    // int intSum = Sum(3, 5, 7);
    // double doubleSum = Sum(3.2, 5.3, 7.1);
    // decimal decimalSum = Sum(3.2m, 5.3m, 7.1m);

    // Et pour les additions on pourrait utiliser l'existant : IAdditionOperators<TSelf, TOther, TResult>
    record PointP(int X, int Y) : IAdditionOperators<PointP, PointP, PointP> {
        public static PointP operator +(PointP left, PointP right) {
            return new(left.X + right.X, left.Y + right.Y);
        }
    }
    // ==========

    public static void Test() {
        Note A = new (0);
        Note B = A + 2;
        Note Csharp = B + 2;
        Console.WriteLine($"B + 2 semi tones => C# : {Csharp.GetSemiTones()}"); // C# = 3 semi tones
        Console.WriteLine($"B < C# : {B < Csharp}"); // true

        Note n = (Note) 554.37;    // explicit conversion
        double freq = n;           // implicit conversion
        Console.WriteLine($"Note n : {n.GetSemiTones()}, frequence de n : {freq:F2}"); // ":F2" pour afficher seulement 2 décimales.

        Point p1 = new(2, -7);
        Point p2 = new(5, 18);
        Console.WriteLine($"p1 + p2 = {p1 + p2}"); // Point { X = 7, Y = 11 }
    }
}
