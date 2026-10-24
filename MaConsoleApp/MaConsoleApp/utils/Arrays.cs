using System.ComponentModel;
using System.Xml.Linq;

namespace MaConsoleApp.utils.arrays;

public class Arrays {


    public static void Test() {
        // easiest (& modern) way i.e. collection way
        // other way was : int[] myArray = {1, 2, 3}
        int[] myArray = [1, 2, 3];
        int first = myArray[0];
        Console.WriteLine(first);

        // Dynamic creation
        int size = 2;
        Array a = Array.CreateInstance(typeof(string), size);
        a.SetValue("hello", 0);
        a.SetValue("world", 1);
        Console.WriteLine($"{a.GetValue(0)} {a.GetValue(1)}"); // "hello world"

        // We can also cast to a C# array as follows:
        string[] cSharpArray = (string[])a;
        string s2 = cSharpArray[0];
        Console.WriteLine(s2); // "hello"

        DemoIndexers();

        Array.ForEach([1, 2, 3], Console.WriteLine /* Action delegate */);

        string[] names = { "Rodney", "Jack", "Jill" };
        string? matchA = Array.Find(names, ContainsA);
        string? matchL = Array.Find(names, n => n.Contains('l'));
        if (matchA is not null) {
            Console.WriteLine(matchA);     // Jack
        }
        if (matchL is not null) {
            Console.WriteLine(matchL);     // Jill
        }
    }

    static bool ContainsA(string name) { return name.Contains('a'); }

    static void WriteFirstValue(Array a) {
        Console.Write(a.Rank + "-dimensional; ");
        // The indexers array will automatically initialize to all zeros, so
        // passing it into GetValue or SetValue will get/set the zero-based
        // (i.e., first) element in the array.
        // ex : int[] indexers = new int[2] ==> [0, 0]
        int[] indexers = new int[a.Rank];
        Console.WriteLine("First value is " + a.GetValue(indexers));
    }

    static void DemoIndexers() {
        int[] oneD = [1, 2, 3];
        int[,] twoD = { { 5, 6 }, { 8, 9 } }; // cannot use collection way, must use {}
        WriteFirstValue(oneD);   // 1-dimensional; first value is 1 ==> indexers = [0] // 1st element index
        WriteFirstValue(twoD);   // 2-dimensional; first value is 5 ==> indexers = [0, 0] // 1st array index, then 1st element index in the array
    }
}