using System;
namespace Linked_In
{
	public class User
	{
		public User()
		{
		}

        public int ID { get; set; }
        public string name { get; set; }
        public string dateOfBirth { get; set; }

        public string universityLocation { get; set; }
        public string field { get; set; }
        public string workplace { get; set; }

        public List<string> specialties { get; set; } = new List<string>();
        public List<int> connectionId { get; set; } = new List<int>();

        //public List<User> connectedUser

        public void DeleteUser(List<User> allUsers)
        {
            // Remove the user ID from other users' ConnectionId
            foreach (User otherUser in allUsers)
            {
                if (otherUser != this)
                {
                    otherUser.connectionId.RemoveAll(id => id == this.ID);
                }
            }
        }

        public List<User> convertIdToUser(List<User> usersList)
        {
            List<User> users = new List<User>();
            foreach (var targetID in connectionId)
            {
                foreach (var user in usersList)
                {
                    
                    if (targetID == user.ID)
                    {
                        //Console.WriteLine(user.ID);
                        users.Add(user);
                        break;
                    }
                }
            }
            return users;
        }

        public void PrintData()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"ID:                  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ID);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Name:                ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(name);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Date of Birth:       ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(dateOfBirth);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"University Location: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(universityLocation);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Field of Study:      ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(field);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Workplace:           ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(workplace);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Specialties:");
            Console.ForegroundColor = ConsoleColor.White;
            string result = string.Join(", ", specialties);
            Console.WriteLine(result);
            //foreach (var item in connectionId)
            //{
            //    Console.WriteLine(item);
            //}
        }

        public bool isConnected(User targetUser)
        {
            if (connectionId.Contains(targetUser.ID))
                return true;
            else return false;
        }

        public void Connect(User targetUser)
        {
            if(!connectionId.Contains(targetUser.ID))
                connectionId.Add(targetUser.ID);
            if(!targetUser.connectionId.Contains(this.ID))
                targetUser.connectionId.Add(this.ID);
            Pause.Dot("Connecting", 2);
        }
        public void Disconnect(User targetUser)
        {
            if (connectionId.Contains(targetUser.ID))
                connectionId.Remove(targetUser.ID);
            if (targetUser.connectionId.Contains(this.ID))
                targetUser.connectionId.Remove(this.ID);
            Pause.Dot("Disconnecting", 2);
        }

    }
}

