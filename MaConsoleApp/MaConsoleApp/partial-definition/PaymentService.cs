namespace MaConsoleApp.partial; // Il faut que toutes les parties aient le même namespace

public partial class PaymentService {

    partial void ValiderPaiement(decimal amount); // partial = private par défaut

    public void Payer(decimal amount) {
        ValiderPaiement(amount);
        Console.WriteLine($"Montant payé : {amount}");
    }
}
