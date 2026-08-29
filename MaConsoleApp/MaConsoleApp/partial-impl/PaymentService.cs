namespace MaConsoleApp.partial;


public partial class PaymentService {

    partial void ValiderPaiement(decimal amount) {
        Console.WriteLine("Paiement validé");
    }
}

public abstract class Asset {
    public abstract decimal NetValue { get; }

    public virtual decimal Price { get; set; } = decimal.Zero;

    public abstract Asset Clone();
}

public class Stock : Asset {
    public long SharesOwned;

    public decimal CurrentPrice;

    public override decimal Price { get => base.Price; set => base.Price = value; }

    // Override like a virtual method.
    public override decimal NetValue => CurrentPrice * SharesOwned;  // Expression-bodied property, équivalent de : public override decimal NetValue { get { return CurrentPrice * SharesOwned; } }

    public override Stock Clone() => new() { Price = Price,  CurrentPrice = CurrentPrice, SharesOwned = SharesOwned};
}