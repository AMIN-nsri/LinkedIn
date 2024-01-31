using System;
namespace Linked_In
{
	public class Admin
	{
		public Admin()
		{
		}
        private string MainUsername = "AMIN";
        private string MainPassword = "12345a";
        public bool LoginCheck(string username, string password)
        {
            if (username == MainUsername && password == MainPassword)
                return true;

            return false;
        }
    }
}

