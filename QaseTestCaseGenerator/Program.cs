using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Settings;

class Program
{
    static async Task Main(string[] args)
    {
        #if !DEBUG
            await AppSettings.CheckForUpdates();
        #endif
        AppSettings.InitializeClients();
        AppSettings.InitializeCommands();
        IOSettings.InitializeConsole();
        await Command.RunCommand("about", Array.Empty<string>());
        Console.ReadKey();
        await AppSettings.RunInterface();       
    }
}



