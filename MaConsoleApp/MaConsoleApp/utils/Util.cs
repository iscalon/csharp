namespace MaConsoleApp.utils // ou pour éviter les accolades depuis C#10 on pourrait faire : "namespace MaConsoleApp.utils;"
{

    public static class Util
    {
        public static void Split(string fullName, out string firstName, out string lastName)
        {
            string[] parts = fullName.Split(' ');
            firstName = parts[0];
            lastName = "";
            if (parts.Length >= 2)
            {
                lastName = parts[1];
            }
        }
    }
}