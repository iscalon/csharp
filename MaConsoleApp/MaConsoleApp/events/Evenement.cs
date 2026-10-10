using System.Globalization;

namespace MaConsoleApp.events; 


class PriceChangedEventArgs(decimal? PreviousPrice, decimal NewPrice, CultureInfo cultureInfo) : EventArgs {

    public decimal? PreviousPrice { get; init; } = PreviousPrice;
    public decimal NewPrice { get; init; } = NewPrice;
    public CultureInfo CultureInfo { get; init; } = cultureInfo;
}

class Stock(CultureInfo cultureInfo) {

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
        OnPriceChanged(new PriceChangedEventArgs(oldPrice, price, cultureInfo));
        return oldPrice;
    }

    public decimal? GetCurrentPrice() {
        return this.price;
    }
}

class StockEventTest {

    public static void Test() {
        Stock stock = new(CultureInfo.GetCultureInfo("fr-FR"));
        stock.ChangePrice(43500);
        stock.PriceChanged += ListenPriceChange;
        stock.ChangePrice(48000);
    }

    private static void ListenPriceChange<T>(T source, PriceChangedEventArgs e) {
        Console.WriteLine($"Price changed by {source}");
        Console.WriteLine($"New price : {e.NewPrice.ToString("C", e.CultureInfo)}");
        if (e.PreviousPrice == null) {
            return;
        }
        Console.WriteLine($"Previous price : {e.PreviousPrice?.ToString("C", e.CultureInfo)}");
        if (e.PreviousPrice == decimal.Zero) {
            return;
        }
        Console.WriteLine($"In/Decrease : {((e.NewPrice - e.PreviousPrice) / (e.PreviousPrice)):P2}"); // ":P2" pour afficher un pourcentage avec 2 décimales.
    }
}