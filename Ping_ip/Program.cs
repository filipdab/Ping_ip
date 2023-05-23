using System;
using System.Net.NetworkInformation;

class Program
{
    static void Main()
    {
        Console.Write("Podaj adres początkowy: ");
        string startAddress = Console.ReadLine();

        Console.Write("Podaj adres końcowy: ");
        string endAddress = Console.ReadLine();

        string[] startParts = startAddress.Split('.');
        string[] endParts = endAddress.Split('.');

        if (startParts.Length != 4 || endParts.Length != 4)
        {
            Console.WriteLine("Nieprawidłowy format adresów.");
            return;
        }

        int[] startIP = new int[4];
        int[] endIP = new int[4];

        for (int i = 0; i < 4; i++)
        {
            if (!int.TryParse(startParts[i], out startIP[i]) || startIP[i] < 0 || startIP[i] > 255 ||
                !int.TryParse(endParts[i], out endIP[i]) || endIP[i] < 0 || endIP[i] > 255)
            {
                Console.WriteLine("Nieprawidłowy format adresów.");
                return;
            }
        }

        Ping pingSender = new Ping();
        PingOptions options = new PingOptions();
        options.DontFragment = true;

        byte[] buffer = System.Text.Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        int timeout = 120;

        Console.WriteLine("Rozpoczynanie pingowania...");

        for (int i = startIP[3]; i <= endIP[3]; i++)
        {
            string ipAddress = $"{startIP[0]}.{startIP[1]}.{startIP[2]}.{i}";

            try
            {
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Host {ipAddress} jest dostępny.");
                }
                else
                {
                    Console.WriteLine($"Brak odpowiedzi z hosta {ipAddress}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pingowania {ipAddress}: {ex.Message}");
            }
        }

        Console.WriteLine("Pingowanie zakończone.");
    }
}
