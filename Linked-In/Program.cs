using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Linked_In;
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

            Menu();
            

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

        public static void Menu()
        {
            string input;
            do
            {

                input = Console.ReadLine();

            } while (input != "E");
        }
    }
}