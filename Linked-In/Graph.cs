using System;
namespace Linked_In
{
	public class Graph
	{
		public Graph()
		{
		}

        public List<User> Nodes { get; set; } = new List<User>();

        public void AddConnection(User user1, User user2)
        {
            user1.Connections.Add(user2);
            user2.Connections.Add(user1);
        }
    }
}

