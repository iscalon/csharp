namespace MaConsoleApp.events; 


class PriceChangedEventArgs(decimal? PreviousPrice, decimal NewPrice, string Symbol) : EventArgs {

    public decimal? PreviousPrice { get; init; } = PreviousPrice;
    public decimal NewPrice { get; init; } = NewPrice;
    public string Symbol { get; init; } = Symbol;
}

class Stock(string Symbol) {

    decimal? price;

    public event EventHandler<PriceChangedEventArgs>? PriceChanged;

    protected virtual void OnPriceChanged(PriceChangedEventArgs e) {
        PriceChanged?.Invoke(this, e); // '?.' fonctionne pour un contexte multithread
    }

    public decimal? ChangePrice(decimal price) {
        decimal? oldPrice = this.price;
        this.price = price;
        if(oldPrice == price) {
            return oldPrice;
        }
        OnPriceChanged(new PriceChangedEventArgs(oldPrice, price, Symbol));
        return oldPrice;
    }

    public decimal? GetCurrentPrice() {
        return this.price;
    }
}

class StockEventTest {

    public static void Test() {
        Stock stock = new("EUR");
        stock.ChangePrice(43500);
        stock.PriceChanged += ListenPriceChange;
        stock.ChangePrice(48000);
    }

    private static void ListenPriceChange<T>(T source, PriceChangedEventArgs e) {
        Console.WriteLine($"Price changed by {source}");
        Console.WriteLine($"New price : {e.NewPrice} {e.Symbol}");
        if (e.PreviousPrice == null) {
            return;
        }
        Console.WriteLine($"Previous price : {e.PreviousPrice} {e.Symbol}");
        if (e.PreviousPrice == decimal.Zero) {
            return;
        }
        Console.WriteLine($"In/Decrease : {((e.NewPrice - e.PreviousPrice) / (e.PreviousPrice)) * 100:F2} %"); // ":F2" pour afficher seulement 2 décimales.
    }
}