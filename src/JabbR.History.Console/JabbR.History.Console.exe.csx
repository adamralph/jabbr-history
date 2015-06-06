#r JabbR.History.Console.exe
using JabbR.History.Console;

// 1. replace with the path to your downloaded JabbR history file
// e.g. https://jabbr.net/#/rooms/general-chat -> Room Settings -> Download Messages
Add("FileName", @"C:\Users\Adam\Desktop\general-chat.2015-06-06.122428+00.json");

// 2. write a function which accepts a message argument and returns true if the message matches your criteria
Add("Match", (Func<Message,bool>)(message =>
    message.When > DateTime.Now.AddMonths(-6) &&
    message.Content.Contains("adamralph")
    ));

// 3. set the number messages to show before and after each matched message
Add("NumberOfMessagesToShowBeforeAndAfter", 5);
