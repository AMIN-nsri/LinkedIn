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
            Console.WriteLine(ID);
            Console.WriteLine(name);
            Console.WriteLine(dateOfBirth);
            Console.WriteLine(universityLocation);
            Console.WriteLine(field);
            Console.WriteLine(workplace);
            foreach(var item in specialties)
            {
                Console.WriteLine(item);
            }
            foreach (var item in connectionId)
            {
                Console.WriteLine(item);
            }
        }

    }
}

