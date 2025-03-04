using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using QaseTestCaseGenerator.Commands;
using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Settings;
using QaseTestCaseGenerator.Static;
using RestSharp;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static async Task Main(string[] args)
    {
        AppSettings.InitializeClients();
        AppSettings.InitializeCommands();
        IOSettings.InitializeConsole();

        Console.WriteLine($"Current Output Encoding: {Console.OutputEncoding.EncodingName}");
        await Command.RunCommand("about", Array.Empty<string>());
        Console.ReadKey();
        await AppSettings.RunInterface();       
    }
}



