// See https://aka.ms/new-console-template for more information
Console.WriteLine("Checking Sites");

var upTime = new UpTimeMonitor.UpTimeMonitor();
upTime.runCheck();
