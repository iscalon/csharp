using System.Net;

namespace MaConsoleApp.utils {

    internal class TryCatch {

        public static void Perform<T, U>(Func<T, U> fonction, T argument) {
            try {
                fonction.Invoke(argument);
            } catch (OverflowException) {
                Console.WriteLine("Il y a eu un dépassement de capacité");
            } catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout) {
                Console.WriteLine($"Il y a eu un timeout : {ex.Message}");
            } catch (WebException ex) when (ex.Status == WebExceptionStatus.SendFailure) {
                Console.WriteLine("Echec de l'envoi");
            } finally {
                Console.WriteLine("Cleanup");
            }
        }

        public static void TryWithResources() {
            using (StreamReader reader = File.OpenText("file.txt")) {
                Console.WriteLine("Equivalent Java d'un try-with-resources");
                Console.WriteLine(reader.ReadToEnd());
            } // reader.Dispose() réalisé ici

            using StreamReader reader2 = File.OpenText("file.txt");
            Console.WriteLine("Equivalent Java d'un try-with-resources");
            Console.WriteLine(reader2.ReadToEnd());
        } // reader2.Dispose() réalisé ici à la sortie du bloc englobant
    }
}
