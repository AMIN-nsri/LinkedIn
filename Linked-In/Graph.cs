using System;
using System.Runtime.Intrinsics.X86;

namespace Linked_In
{
	public class Graph
	{
		public Graph()
		{
		}

        public List<User> Nodes { get; set; } = new List<User>();
        private Dictionary<User, List<User>> AdjacencyList { get; set; } = new Dictionary<User, List<User>>();

        public void AddUser(User user)
        {
            if (!Nodes.Contains(user))
            {
                Nodes.Add(user);
                AdjacencyList[user] = new List<User>();
            }
        }

        public void DeleteUser(User user)
        {
            if (Nodes.Contains(user))
            {
                // Remove the node and its connections from the graph
                Nodes.Remove(user);

                // Remove connections from the adjacency list
                if (AdjacencyList.ContainsKey(user))
                {
                    foreach (User connectedUser in AdjacencyList[user])
                    {
                        AdjacencyList[connectedUser].Remove(user);
                    }
                    AdjacencyList.Remove(user);
                }

                // Remove connections to the node from other nodes' adjacency lists
                foreach (User otherUser in Nodes)
                {
                    if (AdjacencyList.ContainsKey(otherUser))
                    {
                        AdjacencyList[otherUser].Remove(user);
                    }
                }
            }
            else
            {
                Console.WriteLine("User does not exist in the graph.");
            }
        }

        public void AddConnection(User user1, User user2)
        {
            AddUser(user1);
            AddUser(user2);

            if (!AdjacencyList[user1].Contains(user2))
            {
                AdjacencyList[user1].Add(user2);
            }
            if (!AdjacencyList[user2].Contains(user1))
            {
                AdjacencyList[user2].Add(user1);
            }
        }

        public void RemoveConnection(User user1, User user2)
        {
            if (Nodes.Contains(user1) && Nodes.Contains(user2))
            {
                if (AdjacencyList.ContainsKey(user1))
                {
                    AdjacencyList[user1].Remove(user2);
                }

                if (AdjacencyList.ContainsKey(user2))
                {
                    AdjacencyList[user2].Remove(user1);
                }
            }
        }

            public List<User> GetConnections(User user)
        {
            if (AdjacencyList.ContainsKey(user))
            {
                return AdjacencyList[user];
            }
            return new List<User>();
        }

        public int CalculateDegreeOfSeparation(User user1, User user2)
        {
            if (user1 == user2)
            {
                return 0; // The degree of separation is 0 for the same user
            }

            Queue<User> queue = new Queue<User>();
            Dictionary<User, int> distances = new Dictionary<User, int>();

            // Enqueue the source node (user1) and set its distance to 0
            queue.Enqueue(user1);
            distances[user1] = 0;

            while (queue.Count > 0)
            {
                User current = queue.Dequeue();

                // Check each neighbor of the current user
                foreach (User neighbor in GetConnections(current))
                {
                    // If the neighbor has not been visited
                    if (!distances.ContainsKey(neighbor))
                    {
                        // Enqueue the neighbor and set its distance to the current distance + 1
                        queue.Enqueue(neighbor);
                        distances[neighbor] = distances[current] + 1;

                        // If the target user is found, return its distance
                        if (neighbor == user2)
                        {
                            return distances[neighbor];
                        }
                    }
                }
            }

            // If the target user is not reachable, return a high value (e.g., int.MaxValue)
            return int.MaxValue;
        }

        public double CalculateSimilarity(User targetUser, User otherUser)
        {
            // Set weights for different features
            double fieldWeight = 1.0;
            double universityWeight = 0.9;
            double expertiseWeight = 0.8;
            double workplaceWeight = 0.7;
            double degreeWeight = 0.6;
            double connectionsWeight = 0.5;

            // Calculate similarity based on prioritization
            double similarity = 0.0;

            // 1. "Field of Study"
            if (targetUser.field == otherUser.field)
            {
                similarity += fieldWeight;
            }

            // 2. "University"
            if (targetUser.universityLocation == otherUser.universityLocation)
            {
                similarity += universityWeight;
            }

            // 3. List of "Expertise"
            int commonExpertiseCount = targetUser.specialties.Intersect(otherUser.specialties).Count();
            similarity += expertiseWeight * commonExpertiseCount / Math.Max(targetUser.specialties.Count, 1);

            // 4. "Workplace"
            if (targetUser.workplace == otherUser.workplace)
            {
                similarity += workplaceWeight;
            }

            // 5. The degree of the person relative to the target person
            //int degreeDifference = Math.Abs(targetUser.Degree - otherUser.Degree);
            int degreeDifference = CalculateDegreeOfSeparation(targetUser, otherUser);
            similarity += degreeWeight / (degreeDifference + 1); // Avoid division by zero

            // 6. Number of common connections
            int commonConnectionsCount = targetUser.connectionId.Intersect(otherUser.connectionId).Count();
            similarity += connectionsWeight * commonConnectionsCount;

            return similarity;
        }

        public List<User> GenerateRecommendations(User targetUser)
        {
            List<User> recommendations = new List<User>();

            foreach (User user in Nodes)
            {
                if (user != targetUser && CalculateDegreeOfSeparation(targetUser, user) <= 5 && !targetUser.connectionId.Contains(user.ID))
                {
                    double similarity = CalculateSimilarity(targetUser, user);
                    // Adjust the threshold as needed
                    double threshold = 1.5;
                    if (similarity >= threshold)
                    {
                        recommendations.Add(user);
                    }
                }
            }

            // Sort recommendations based on similarity score
            recommendations.Sort((u1, u2) => CalculateSimilarity(targetUser, u2).CompareTo(CalculateSimilarity(targetUser, u1)));

            // Return the top 20 recommendations
            return recommendations.Take(20).ToList();
        }

        public void DisplayRecommendations(List<User> recommendations)
        {
            Console.WriteLine("Top 20 Recommendations:");
            Console.WriteLine("----------------------------");

            foreach (User user in recommendations)
            {
                Console.WriteLine($"ID: {user.ID}");
                Console.WriteLine($"Name: {user.name}");
                //Console.WriteLine($"Birthday: {user.dateOfBirth.ToShortDateString()}");
                Console.WriteLine($"Field: {user.field}");
                Console.WriteLine($"University: {user.universityLocation}");
                Console.WriteLine($"Expertise: {string.Join(", ", user.specialties)}");
                Console.WriteLine($"Workplace: {user.workplace}");
                Console.WriteLine("----------------------------");
            }
        }

        public void PrintGraph()
        {
                Console.WriteLine("Graph Representation:");
                Console.WriteLine("----------------------------");

            foreach (User user in Nodes)
            {
                Console.Write($"User {user.ID}: ");

                // Display first-degree connections
                foreach (User connection in GetConnections(user))
                {
                    Console.Write($"{connection.ID} ");
                }

                // Display second-degree connections
                foreach (User firstDegreeConnection in GetConnections(user))
                {
                    foreach (User secondDegreeConnection in GetConnections(firstDegreeConnection))
                    {
                        // Check if the second-degree connection is not already displayed
                        if (!GetConnections(user).Contains(secondDegreeConnection) && secondDegreeConnection != user)
                        {
                            Console.Write($"{secondDegreeConnection.ID} ");
                        }
                    }
                }

                Console.WriteLine();
            }
        }




        //public List<User> GenerateRecommendations(User targetUser)
        //{
        //    List<User> recommendations = new List<User>();

        //    foreach (User user in Nodes)
        //    {
        //        if (user != targetUser && CalculateDegreeOfSeparation(targetUser, user) <= 5)
        //        {
        //            double similarity = CalculateSimilarity(targetUser, user);
        //            recommendations.Add(new UserWithSimilarity { User = user, Similarity = similarity });
        //        }
        //    }

        //    // Sort recommendations based on similarity score
        //    recommendations.Sort((u1, u2) => u2.Similarity.CompareTo(u1.Similarity));

        //    // Extract the original users without the additional similarity property
        //    List<User> originalRecommendations = recommendations.Select(u => u.User).ToList();

        //    // Return the top 20 recommendations
        //    return originalRecommendations.Take(20).ToList();
        //}

        //private class UserWithSimilarity
        //{
        //    public User User { get; set; }
        //    public double Similarity { get; set; }
        //}
    }
}

