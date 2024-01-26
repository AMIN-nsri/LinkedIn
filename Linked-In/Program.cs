using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Linked_In;

namespace LinkedIn
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<User> usersList = new List<User>();
            Graph socialNetwork = LoadGraphData("users(99).json");


        }

        public static Graph LoadGraphData(string filePath)
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                List<User> userDataList = JsonConvert.DeserializeObject<List<User>>(jsonData);

                Graph socialNetwork = new Graph();

                foreach (var userData in userDataList)
                {
                    User user = new User
                    {
                        ID = int.Parse(userData.ID),
                        FirstName = userData.FirstName.Split(' ')[0],
                        LastName = userData.LastName.Split(' ')[1],
                        BirthDate = userData.BirthDate,
                        // ... other user information

                        Connections = new List<User>()
                    };

                    foreach (var connectionId in userData.connectionId)
                    {
                        int connectionID = int.Parse(connectionId);
                        User connectionUser = socialNetwork.Nodes.Find(u => u.ID == connectionID);

                        if (connectionUser != null)
                        {
                            socialNetwork.AddConnection(user, connectionUser);
                        }
                    }

                    socialNetwork.Nodes.Add(user);
                }

                return socialNetwork;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading graph data: {ex.Message}");
                return null;
            }
        }

        public User convertIntToUser(int userID, List<User> usersList)
        {
            foreach(var user in usersList)
            {
                if(user.ID == userID)
                {
                    return user;
                }
            }
            return null;
        }
    }
}