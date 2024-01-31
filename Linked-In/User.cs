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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"ID:                  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ID);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Name:                ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(name);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Date of Birth:       ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(dateOfBirth);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"University Location: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(universityLocation);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Field of Study:      ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(field);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Workplace:           ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(workplace);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Specialties:");
            Console.ForegroundColor = ConsoleColor.White;
            string result = string.Join(", ", specialties);
            Console.WriteLine(result);
            //foreach (var item in connectionId)
            //{
            //    Console.WriteLine(item);
            //}
        }

    }
}

