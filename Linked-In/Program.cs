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
                Console.WriteLine($"Error loading graph data: {ex.Message}");
                return null;
            }
        }
        
        // Showing Recommendation by User ID
        public void ShowRecommendation(int targetUserID, Graph socialNetwork)
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
                    if (foundUserByBirth == foundUserByID)
                    {
                        UserMenu(foundUserByID, usersList, socialNetwork);
                    }
                    else Pause.Dot("ID & Password Doesn't Match", 3);
                    break;
                default:
                    Messages.Default();
                    break;

            }
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
        public static void PrintConnections(User user, Graph socialNetwork)
        {
            var connections = socialNetwork.GetConnections(user);
            List<string> names = new List<string>();
            foreach (var item in connections)
                names.Add(item.name);
            Console.ForegroundColor = ConsoleColor.Green;
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