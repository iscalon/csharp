namespace MaConsoleApp.utils.enumerations {

    /**
     * Exemple d'énumération combinée : 
     * 
     * BorderSides leftRight = BorderSides.Left | BorderSides.Right
     * 
     *              Bottom Top Right Left
     *                  ↓   ↓    ↓    ↓
     *     leftRight =  0   0    1    1
     *     
     *     
     *     
     * BorderSides s = BorderSides.Left | BorderSides.Right;
     *  // s = 0011
     *  // s ^= BorderSides.Right;  // Toggles 'Right'
     * 
     *  //   0011
     *  // ^ 0010
     *  //   ----
     *  //   0001
     *  
     *  Résultat : s = Left
     *  
     *  (Mais pour retirer 'Right', le mieux est de faire : s &= ~BorderSides.Right)
     */
    [Flags]
    enum BorderSides { None = 0, Left = 1, Right = 1 << 1, Top = 1 << 2, Bottom = 1 << 3,
        LeftAndRight = Left | Right,
        TopAndBottom = Top | Bottom,
        All = LeftAndRight | TopAndBottom
    }

    class BorderSidesTest {

        public static void Ecrire(BorderSides side) {
            switch(side) { 
                case BorderSides.Left:
                    Console.WriteLine("Côté gauche");
                    break;
                case BorderSides.Right:
                    Console.WriteLine("Côté droit");
                    break;
                case BorderSides.Top:
                    Console.WriteLine("Haut");
                    break;
                case BorderSides.Bottom:
                    Console.WriteLine("Bas");
                    break;
                default: throw new ArgumentException($"On ne sait pas afficher le côté : { side }", nameof(side));
            }
        }
    }
}
