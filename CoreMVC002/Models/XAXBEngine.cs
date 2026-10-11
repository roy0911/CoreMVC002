namespace CoreMVC002.Models
{
    using System;
    using System.Linq;

    namespace CoreMVC002.Models
    {
        public class XAXBEngine
        {
            public static string GenerateAnswer()
            {
                Random rand = new Random();
                var digits = Enumerable.Range(0, 10).OrderBy(x => rand.Next()).Take(4);
                return string.Join("", digits);
            }

            public static string Check(string answer, string guess)
            {
                int a = 0, b = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (guess[i] == answer[i])
                    {
                        a++;
                    }
                    else if (answer.Contains(guess[i]))
                    {
                        b++;
                    }
                }
                return $"{a}A{b}B";
            }
        }

    }
}