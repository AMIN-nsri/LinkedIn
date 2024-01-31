using System;
namespace Linked_In
{
	public class Messages
	{
		public Messages()
		{
		}
        public static void Program()
        {
            Console.Clear();
            Time();
            Topic();
            Loading(1);
        }
        private static void Time()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            DateTime dt = DateTime.Now;

            Console.WriteLine($"{dt.Year}/{dt.Month}/{dt.Day}                                                           {dt.ToShortTimeString()}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void Topic()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("-------------------------LinkedIn Social Network-------------------------");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("[b]ack");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void Loading(int pausetime)
        {
            Pause.Dot("Loading Please Wait", pausetime);
        }

        public static void Default()
        {
            Program();
            Console.WriteLine("Please Enter Valid Value!");
            Pause.Dot("", 3);
        }

        public static void AdminUserMenu()
		{
            Program();
			Console.WriteLine("Choose One:");
			Console.WriteLine("(A) Admin");
			Console.WriteLine("(U) User");
            Console.WriteLine("(E) Exit");
		}

        public static void AdminMenu()
        {
            Program();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome Dear Admin!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose One:");
            Console.WriteLine("(L) Show Users List");
            Console.WriteLine("(I) Search User by ID");
            Console.WriteLine("(N) Search User by Name");
        }

        public static void UserMenu(string name)
        {
            Program();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Welcome Dear {name}!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose One:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("(R) See  Recommended Users to Connect");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("(P) Your Profile");
            Console.WriteLine("(C) Show Connected Users");
            Console.WriteLine("(I) Search User by ID");
            Console.WriteLine("(N) Search User by Name");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(D) Delete account");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //public static void ConnectionMenu(string name)
        //{

        //}

    }
    public class Pause
    {
        public static void ClearLine()
        {
            if (Console.CursorTop >= 1)
            {

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
        public static void Dot(string message, int seconds)
        {
            for (int i = 1; i < 5 * seconds; i++)
            {
                switch (i % 5)
                {
                    case 1:
                        Console.WriteLine($"{message}.");
                        Thread.Sleep(300);
                        ClearLine();
                        break;
                    case 2:
                        Console.WriteLine($"{message}..");
                        Thread.Sleep(240);
                        ClearLine();
                        break;
                    case 3:
                        Console.WriteLine($"{message}...");
                        Thread.Sleep(190);
                        ClearLine();
                        break;
                    case 4:
                        Console.WriteLine($"{message}....");
                        Thread.Sleep(150);
                        ClearLine();
                        break;
                    case 0:
                        Console.WriteLine($"{message}.....");
                        Thread.Sleep(120);
                        ClearLine();
                        break;
                }
            }
        }
    }
}

