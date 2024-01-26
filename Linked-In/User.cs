using System;
namespace Linked_In
{
	public class User
	{
		public User()
		{
		}

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }

        public string University { get; set; }
        public string StudyField { get; set; }
        public string WorkPlace { get; set; }

        public List<string> Expertises { get; set; } = new List<string>();
        public List<User> Connections { get; set; } = new List<User>();

        
    }
}

