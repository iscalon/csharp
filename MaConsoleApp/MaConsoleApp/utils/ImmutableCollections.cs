using System.Collections.Immutable;

namespace MaConsoleApp.utils.immmutable.collections;

internal class ImmutableCollections {

    public static void Test() {
        ImmutableArray<int> array = [1, 2, 3];
        Console.WriteLine(array);

        var list = new[] { "one", "two" }.ToImmutableList();
        Console.WriteLine(list);

        var newList = list.Add("three");
        Console.WriteLine(newList.Count);
        Console.WriteLine(list.Count); // Pas altérée, marche aussi avec les Remove

        // Builders
        ImmutableArray<int>.Builder builder = ImmutableArray.CreateBuilder<int>();
        builder.Add(1);
        builder.Add(2);
        builder.Add(3);
        builder.RemoveAt(0);
        ImmutableArray<int> myImmutable = builder.ToImmutable(); // 2,3
        myImmutable.Print(); // extension method :)

        var builder2 = myImmutable.ToBuilder();
        builder2.Add(4);      // Efficient
        builder2.Remove(2);   // Efficient
        // Return a new immutable collection with all the changes applied:
        ImmutableArray<int> myImmutable2 = builder2.ToImmutable(); // 3,4
        myImmutable2.Print();
    }
}
