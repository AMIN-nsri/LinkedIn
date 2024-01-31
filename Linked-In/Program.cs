using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Linked_In;
using System.Text;
using System.Reflection;
//using System.Text.Json;

namespace LinkedIn
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Loading Data from JSON file
            List<User> usersList = LoadData("users(99).json");

            // Getting instance to create graph
            Graph socialNetwork = new Graph();

            // Adding Nodes to graph
            foreach(var user in usersList)
            {
                socialNetwork.AddUser(user);
            }

            // Adding Connections
            foreach (var user in usersList)
            {
                var x = user.convertIdToUser(usersList);
                foreach(var user2 in x)
                {
                    socialNetwork.AddConnection(user, user2);
                }
            }
            var y = usersList[0];
            PrintFullInfo(y, socialNetwork);
            Console.ReadKey();
            Menu(usersList, socialNetwork);
            

        }

        public static List<User> LoadData(string filePath)
        {
            try
            {

                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List < User>>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Graph Data: {ex.Message}");
                return null;
            }
        }

        public static void WriteData(User user, List<User> usersList, string filePath)
        {
            try
            {
                // Find the specific item you want to update
                var specialItem = usersList.Find(i => i == user);
                if (specialItem != null)
                {
                    // Modify the special item
                    specialItem = user;
                    //specialItem.Property1 = "New Value1";
                    //specialItem.Property2 = 789; // Assuming you want to change this
                                                 // ...make other changes as needed
                }
                else
                {
                    // Handle the case where the item was not found
                    Console.WriteLine("user not found");
                    // For example, you can add a new item if it does not exist
                    //items.Add(new YourDataObject { Property1 = "New Value1", Property2 = 789 });
                }

                // Serialize the list back to JSON including the updated item
                string jsonString = JsonConvert.SerializeObject(usersList, Formatting.Indented);

                // Write the new JSON string back to the file
                File.WriteAllText(filePath, jsonString);

                Console.WriteLine("JSON file updated successfully!");


                //string jsonString = JsonConvert.SerializeObject(usersList, Formatting.Indented);
                //File.WriteAllText(filePath, jsonString);
                //Console.WriteLine("JSON file updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Writing Graph Data: {ex.Message}");
            }
        }
        
        // Showing Recommendation by User ID
        public static void ShowRecommendation(int targetUserID, Graph socialNetwork)
        {
            User targetUser = socialNetwork.Nodes.FirstOrDefault(u => u.ID == targetUserID);

            if (targetUser != null)
            {
                List<User> recommendations = socialNetwork.GenerateRecommendations(targetUser);
                socialNetwork.DisplayRecommendations(recommendations);
            }
            else
            {
                // Handle the case where the target user is not found
                Console.WriteLine("the target user is not found!");
            }
        }

        public static void Menu(List<User> usersList, Graph socialNetwork)
        {
            string input;
            do
            {
                Messages.AdminUserMenu();
                input = Console.ReadLine();
                AdminUser(input, usersList, socialNetwork);
            } while (input != "E");
        }
        public static void AdminUser(string s, List<User> usersList, Graph socialNetwork)
        {
            Messages.Program();
            switch(s)
            {
                case "A":
                    Console.WriteLine("Enter your Username and Password below");
                    Console.Write("User Name: ");
                    string input = Console.ReadLine();
                    string username = input;
                    Console.Write("Password: ");
                    input = GetPassword();
                    string password = input;
                    if (new Admin().LoginCheck(username, password))
                    {
                        AdminMenu(usersList, socialNetwork);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong Username or Password!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Pause.Dot("", 3);
                    }
                    break;
                case "U":
                    Console.WriteLine("Enter your ID and Password below(your password is your Date of birth in {YYYY/MM/DD} format)");
                    Console.Write("ID: ");
                    string input2 = Console.ReadLine();
                    int id;
                    if(!int.TryParse(input2, out id))
                    {
                        Pause.Dot("Wrong ID Format",3);
                        break;
                    }
                    Console.Write("Password: ");
                    input = GetPassword();
                    string password2 = input;
                    User foundUserByBirth = usersList.FirstOrDefault(User => User.dateOfBirth == password2);
                    User foundUserByID = usersList.FirstOrDefault(User => User.ID == id);
                    if (foundUserByBirth == foundUserByID && foundUserByID != null)
                    {
                        UserMenu(foundUserByID, usersList, socialNetwork);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Pause.Dot("ID & Password Doesn't Match", 3);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                default:
                    Messages.Default();
                    break;

            }
        }

        public static void UserMenu(User user, List<User> userList, Graph socialNetwork)
        {
            string s;
            bool Deleted = false;
            do
            {
                Messages.UserMenu(user.name);
                s = Console.ReadLine();
                Messages.Program();
                switch (s)
                {
                    case "P":
                        PrintFullInfo(user, socialNetwork);
                        TurnBackPause();
                        break;
                    case "R":
                        Console.WriteLine("Here's your List of Recommendation:");
                        ShowRecommendation(user.ID, socialNetwork);
                        // Make an option to follow instantly!
                        Console.WriteLine("Enter User's ID to Connect or \"b\" to Turn Back");
                        string input3 = Console.ReadLine();
                        int ID2;
                        if (input3 == "b")
                            break;
                        Messages.Program();
                        if (int.TryParse(input3, out ID2))
                        {
                            User foundUser = userList.FirstOrDefault(User => User.ID == ID2);

                            if (foundUser != null)
                            {
                                user.Connect(foundUser);
                                socialNetwork.AddConnection(user, foundUser);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Connected Successfuly!");
                                Console.ForegroundColor = ConsoleColor.White;
                                WriteData(user, userList, "users(99).json");
                                TurnBackPause();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Pause.Dot("User Not Found", 3);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Pause.Dot("Incorrect ID Format", 3);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        //TurnBackPause();
                        break;
                    case "C":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("Your Connections:");
                        Console.ForegroundColor = ConsoleColor.White;
                        var connections = socialNetwork.GetConnections(user);
                        foreach(var connection in connections)
                        {
                            PrintFullInfo(connection);
                            Console.WriteLine("----------------------------");
                        }
                        //PrintConnections(user, socialNetwork);
                        TurnBackPause();
                        break;
                    case "I":
                        Console.WriteLine("Enter User's ID:");
                        string input2 = Console.ReadLine();
                        int ID;
                        Messages.Program();
                        if (int.TryParse(input2, out ID))
                        {
                            User foundUser = userList.FirstOrDefault(User => User.ID == ID);

                            if (foundUser != null)
                            {
                                PrintFullInfo(foundUser);
                                if(user.isConnected(foundUser))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Connected!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Wanna Disconnect?(Y/N)");
                                    string x = Console.ReadLine();
                                    Pause.ClearLine();
                                    Pause.ClearLine();
                                    if (x=="Y")
                                    {
                                        Pause.ClearLine();
                                        user.Disconnect(foundUser);
                                        socialNetwork.RemoveConnection(user, foundUser);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Not Connected!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Not Connected!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Wanna Connect?(Y/N)");
                                    string x = Console.ReadLine();
                                    Pause.ClearLine();
                                    Pause.ClearLine();
                                    if (x == "Y")
                                    {
                                        Pause.ClearLine();
                                        user.Connect(foundUser);
                                        socialNetwork.AddConnection(user, foundUser);
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Connected!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                TurnBackPause();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Pause.Dot("User Not Found", 3);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Pause.Dot("Incorrect ID Format", 3);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "N":
                        Messages.Program();
                        Console.WriteLine("Enter User's Name:");
                        string name = Console.ReadLine();
                        Messages.Program();
                        User foundUser2 = userList.FirstOrDefault(User => User.name == name);
                        if (foundUser2 != null)
                        {
                            PrintFullInfo(foundUser2);
                            if (user.isConnected(foundUser2))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Connected!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Wanna Disconnect?(Y/N)");
                                string x = Console.ReadLine();
                                Pause.ClearLine();
                                Pause.ClearLine();
                                if (x == "Y")
                                {
                                    Pause.ClearLine();
                                    user.Disconnect(foundUser2);
                                    socialNetwork.RemoveConnection(user, foundUser2);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Not Connected!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Not Connected!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Wanna Connect?(Y/N)");
                                string x = Console.ReadLine();
                                Pause.ClearLine();
                                Pause.ClearLine();
                                if (x == "Y")
                                {
                                    Pause.ClearLine();
                                    user.Connect(foundUser2);
                                    socialNetwork.AddConnection(user, foundUser2);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Connected!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            TurnBackPause();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Pause.Dot("User Not Found", 3);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "D":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Are You Sure You Want Delete Your Account?");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("if yes type \"CONFIRM\"");
                        string input = Console.ReadLine();
                        if(input == "CONFIRM")
                        {
                            Messages.Program();
                            Pause.Dot("Your Account Will be Deleted at the moment", 4);
                            user.DeleteUser(userList);
                            userList.Remove(user);
                            socialNetwork.DeleteUser(user);
                            Messages.Program();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Account Deleted Successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                            Deleted = true;
                        }
                        break;
                    default:
                        if (s != "b")
                            Messages.Default();
                        break;
                }
            } while (s != "b" && !Deleted);
        }

        

        public static void AdminMenu(List<User> userList, Graph socialNetwork)
        {
            string s;
            do
            {
                Messages.AdminMenu();
                s = Console.ReadLine();
                switch (s)
                {
                    case "L":
                        Messages.Program();
                        Console.WriteLine("Choose User by Index to See Complete Information");
                        PrintInList(userList);
                        string input = Console.ReadLine();
                        int index;
                        Messages.Program();
                        if (int.TryParse(input, out index))
                        {
                            PrintFullInfo(userList[index - 1], socialNetwork);
                        }
                        else Console.WriteLine("Wrong Index Format!");
                        TurnBackPause();
                        break;
                    case "I":
                        Messages.Program();
                        Console.WriteLine("Enter User's ID:");
                        string input2 = Console.ReadLine();
                        int ID;
                        Messages.Program();
                        if (int.TryParse(input2, out ID))
                        {
                            User foundUser = userList.FirstOrDefault(User => User.ID == ID);

                            if (foundUser != null)
                            {
                                PrintFullInfo(foundUser, socialNetwork);
                                TurnBackPause();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Pause.Dot("User Not Found", 3);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Pause.Dot("Incorrect ID Format", 3);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "N":
                        Messages.Program();
                        Console.WriteLine("Enter User's Name:");
                        string name = Console.ReadLine();
                        Messages.Program();
                        User foundUser2 = userList.FirstOrDefault(User => User.name == name);
                        if (foundUser2 != null)
                        {
                            PrintFullInfo(foundUser2, socialNetwork);
                            TurnBackPause();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Pause.Dot("User Not Found", 3);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    default:
                        if(s != "b")
                            Messages.Default();
                        break;
                }
            } while (s != "b");
        }

        public static void PrintInList(List<User> userList)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{"",-3} {"ID",-4}    {"Name",-12}");
            Console.ForegroundColor = ConsoleColor.White;
            var i = 1;
            foreach (var user in userList)
            {
                Console.WriteLine($"{i + "-",-3} {user.ID,-4}->  {user.name,-12}");
                i++;
            }
        }
        public static void PrintFullInfo(User user, Graph socialNetwork)
        {
            user.PrintData();
            PrintConnections(user, socialNetwork);
        }
        public static void PrintFullInfo(User user)
        {
            user.PrintData();
        }
        public static void PrintConnections(User user, Graph socialNetwork)
        {
            var connections = socialNetwork.GetConnections(user);
            List<string> names = new List<string>();
            foreach (var item in connections)
                names.Add(item.name);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Connections:");
            Console.ForegroundColor = ConsoleColor.White;
            string result = string.Join(", ", names);
            Console.WriteLine(result);
        }

        public static void TurnBackPause()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key to turn back.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        // Password-Hide by '*'
        private static string GetPassword()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.SetCursorPosition(x - 1, y);
                    Console.Write(" ");
                    Console.SetCursorPosition(x - 1, y);
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return input.ToString();
        }

    }
}