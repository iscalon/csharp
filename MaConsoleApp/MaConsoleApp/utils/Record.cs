namespace MaConsoleApp.utils.records;


// Version du record sans position (pas de liste d'arguments comme avec 'Product' plus bas).
record Point {

    // Backing fields
    double _x, _y;
    double? _distance; // celui-ci sera 'lazy' initialisé, une fois seulement et quand on en aura besoin.

    // Constructeur 'classique' (i.e pas 'primaire', le constructeur primaire n'existe que pour les records avec liste d'arguments, ex : record A(int x, int y){})
    public Point(double x, double y) => (X, Y) = (x, y); // Donc ça appelle les init de X et de Y bien que X et Y ne soient pas des propriétés "automatiques"
                                                         // Rappel les propriétés automatiques sont celles où on ne fournit pas d'implem au get et init, ex : public double X {get; init;} et où un backing field est créé par le compilateur

    public double X { 
        get => _x;
        init {
            _x = double.IsNaN(value) ? 0 : value;
            _distance = null;
        }
    }

    public double Y { 
        get => _y;
        init {
            _y = value;
            _distance = null;
        }
    }

    public double EuclidianDistanceFromOrigin {
        get {
            if(_distance != null) {
                return _distance.Value;
            }
            _distance = Math.Sqrt(X * X + Y * Y);
            return _distance.Value;
        }
    }
    // Version plus concise : "public double EuclidianDistanceFromOrigin => _distance ??= Math.Sqrt (X*X + Y*Y);"
}

/**
 * Version avec liste de paramètres, plus simple grâce à la redéfinition du constructeur par recopie (mais sans check du NaN sur X) : 
 * 
 * record Point (double X, double Y)
 * {
 *  double? _distance;
 *  public double DistanceFromOrigin => _distance ??= Math.Sqrt (X*X + Y*Y);
 *  protected Point (Point other) => (X, Y) = other;
 * }
 * 
 * La propriété '_distance' est bien ignorée lors de la recopie (donc mise à null, réinitialisation du cache).
 * 
 * Par contre on ne peut plus avoir de validation de paramètres avec la version en liste.
 * Il faudrait repasser sur une version qui définit un constructeur "classique", i.e. pas "primaire"
 * car si on fait qqch comme ça : 
 * 
        record Person(string Name)
        {
            string _name = Name;

            public string Name
            {
                get => _name;
                init => _name = value ?? throw new ArgumentNullException("Name");
            }
        }
 * alors le 'Name' qu'on voit à droite de "string _name = Name;" est le paramètre du constructeur primaire
 * et non pas la propriété "public string Name" plus bas.
 * La méthode init n'est pas appelée lorsqu'on fait : new Person(null) par exemple, donc pas de check sur la nullité du nom passé.
 * Ca affecte directement la valeur au field '_name'.
 */

enum ProductType {
    DIGITAL, PHYSICAL
}

record Product(string Nom, decimal Valeur, ProductType Type) {

    public string Nom {
        get;
        init {
            // C#14 permet d'utiliser le mot clé 'field' et ainsi en précisant seulement le init à ce niveau plutôt qu'au niveau de la propriété
            // cette validation sera appelée même lors de l'utilisation de la recopie avec le mot clé : 'with'
            field = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Il faut un nom pour un produit", nameof(Nom)) : value;
        }
    } = string.IsNullOrWhiteSpace(Nom) ? throw new ArgumentException("Il faut un nom pour un produit", nameof(Nom)) : Nom;

    public ProductType Type {
        get;
        init {
            // A titre d'exemple, ne devrait pas arriver
            field = value.GetTypeCode() < 0 ? throw new ArgumentOutOfRangeException(nameof(Type), "Problème de type") : value;
        }
    }

    // Ici c'est une initialisation de propriété donc non rappelée lors de l'utilisation de la recopie avec un 'with'
    public decimal Valeur { get; init; } = ValiderPrix(Valeur, Type);

    private static decimal ValiderPrix(decimal prix, ProductType typeDeProduit) {
        if(prix <= 0) {
            throw new ArgumentOutOfRangeException(nameof(prix), "Le prix doit être positif");
        }
        if (typeDeProduit == ProductType.DIGITAL) {
            return prix;
        }
        if(prix <= 10) {
            throw new ArgumentException("Un produit qui n'est pas digital doit avoir un prix supérieur à 10", nameof(prix));
        }
        return prix;
    }

    public static Product Copy(Product product) {
        return new Product(product); // Constructeur protected généré
    }
}

internal class Record {

    public static void Test() {
        Point p = new(10, -15);
        Console.WriteLine($"{p}"); // Point { X = 10, Y = -15 }
        p =  p with { Y = 5};
        Console.WriteLine($"{p}"); // Point { X = 10, Y = 5 }
        Console.WriteLine($"Distance : {p.EuclidianDistanceFromOrigin}");

        Product clavier = new("Clavier", 11, ProductType.PHYSICAL);
        Console.WriteLine(clavier);

        Product carteCadeau = new ("Carte virtuelle", 5, ProductType.DIGITAL);

        try {
            Product ecran = new ("Ecran", 7, ProductType.PHYSICAL);
        } catch (Exception ex) {
            Console.WriteLine($"Problème de création de produit : {ex}");
        }

        try {
            Product souris = carteCadeau with { Nom = "Souris", Valeur = 4, Type = ProductType.PHYSICAL };
            Console.WriteLine(souris);
        } catch (Exception ex) {
            // On ne passera pas ici, même si la Valeur est inférieure à 10 car
            // l'init n'est appelé qu'au moment de la création de l'objet.
            Console.WriteLine($"Problème de création de produit : {ex}");
        }
        // Par contre comme on a défini une vérification sur le nom au niveau du init
        // alors ici ça va passer dans le catch :
        try {
            Product souris = carteCadeau with { Nom = "", Valeur = 4, Type = ProductType.PHYSICAL };
        } catch (Exception ex) {
            Console.WriteLine($"Problème de création de produit : {ex}");
        }

        try {
            Product souris = Product.Copy(carteCadeau with { Nom = "Souris", Valeur = 4, Type = ProductType.PHYSICAL }); // Là non plus on ne passera pas par la validation du prix.
        } catch (Exception ex) {
            Console.WriteLine($"Problème de création de produit : {ex}");
        }

        Student john = new("0012", "Connord", "John");
        Console.WriteLine(john);
        john = john with { FirstName = "Toto" }; // OK
        Console.WriteLine(john);
        // john = john with { ID = "0111" }; // KO
    }

    record Student(string ID, string LastName, string FirstName) {
        public string ID /* Le ID ici est la propriété en readonly */ { get; } = ID /* Le ID ici est le paramètre du constructeur primaire */;
        readonly int _enrollmentYear = int.Parse(ID[..4]);

        // Pour redéfinir l'égalité il faut que la méthode 'Equals' soit 'virtual' et prenne le type exact du record sur lequel elle est.
        public virtual bool Equals(Student? other) {
            if(other == null) {
                return false;
            }
            if (!other.ID.Equals(ID)) {
                return false;
            }
            if (!other.LastName.Equals(LastName)) {
                return false;
            }
            if (!other.FirstName.Equals(FirstName)) {
                return false;
            }
            // On ignore par exemple "_enrollmentYear"
            return true;
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }
    }
}