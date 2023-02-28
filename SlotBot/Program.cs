using System.Linq.Expressions;

namespace SlotBot
{
    internal class Program
    {
        static void Main()
        {
            RunSlots();

            Console.ReadLine();
        }
        public static async void RunSlots()
        {
            Console.WriteLine("---\n Remember to copy your discord token to the 'token.txt' file that *should* be in the same direcctory as this executable.\n---");

            ConsoleWriteWithTime(" - SlotBot Online.");

            string token = DefineToken();
            if (token == null || token == "")
            {
                ConsoleWriteWithTime("You didn't provide a token. Add your token to the file and then relaunch the program.");
            }

            int slottingTotal = 0;
            while (slottingTotal == 0)
            {
                Console.Write("How many times would you like to slot? (720 is ~ 1 hour of slotting): ");
                try
                {
                    slottingTotal = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Not a valid integer. Use a positive whole number.");
                }
            }
            int slotValue = 0;
            while (slotValue == 0)
            {
                Console.Write("How much would you like to slot per time? (Must be divisable by 5): ");
                try
                {
                    slotValue = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception)
                {
                    Console.WriteLine("What you provided is not a positive whole number.");
                }
                if (slotValue % 5 != 0 || slotValue == 0)
                {
                    Console.WriteLine("What you provided is not divisable by 5.");
                    slotValue = 0;
                }
            }
            ConsoleWriteWithTime(" - OK. Creating https client...");

            var slotBotValues = new Dictionary<string, string>
                { { "content", $"!!slot {slotValue}" } };
            var data = new FormUrlEncodedContent(slotBotValues);
            var url = "https://discord.com/api/v9/channels/1076747239164219493/messages";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            ConsoleWriteWithTime(" - https client created, starting slotting...");
            int slottingTotalOG = slottingTotal;
            while (slottingTotal > 0)
            {
                await Task.Delay(5500);
                Console.WriteLine($"Sending !!slot {slotValue}... ({slottingTotal} remaining)");
                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                Console.ForegroundColor = ConsoleColor.Green;
                ConsoleWriteWithTime(" - Sent!");
                Console.ResetColor();

                slottingTotal--;
            }
            Console.WriteLine("Slotting finished. Relaunch program to start again.");
        }
        public static void ConsoleWriteWithTime(string message)
        {
            Console.WriteLine(DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + message);
        }
        public static string DefineToken()
        {
            return File.ReadAllText("./token.txt");
        }
    }
}