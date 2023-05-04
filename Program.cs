using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace part_8_and_but_two
{

    //FOR THE HANGMAN GAME, YOU ARE ALSO HANGED.  YOU FIGHT AGAINST THE JOKE MONKEY IN A HANGMAN BATTLE TO KEEP YOUR LIFE.
    //MAKE IT SCROLL DOWM
    internal class Program
    {
        const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        public static List<string> all = new List<string>();
        public static string[] main = { 
            "[After a long, monkey-induced coma, you finally awaken]",
        "[Right when you open your eyes, you spot multimedia and business tycoon, Gargolomew the Greedy, haning the Joke Monkey on a gallows]",
        "[Gargolomew notices you and freezes, his mouth agape in a display of disbelief]",
        "[Before you can react, the crow grabs you from behind and hangs you on a gallows]",
        };
        public static string[] garg =
        {
            "\"How exuberant!  A human!\" Gargolomew's aghast visage is quickly replaced with one of curiosity and giddiness as he begins to speak.  \"I'm thrilled!  You just gave me an epiphany!  A brilliant idea was \"",
            "[Gargolomew the Greedy's smirk evolves into a gargantuan beast, its girth so vast it threatens to devour all]",
            "\"We shall have a duel to see which is smarter: The human, or the mutated monkey!\" Gargolomew chortles.",
            "\"You shall both participate in a game to guess the word that goes in the blank spaces.  Each failed guess shall result in a loss of a limb, until only a head remains.  The winner will get to go free, and will receive bionic Gargolomew limbs as replacements, and whoever loses will be left to rot on the gallows.\"",
            "[The crow wheels in a whiteboard, and Gargolomew draws the spaces for each letter, signalling the start of the game]"
        };
        public static string write, invalid, wascii;
        public static bool space;

        //ASCII
        public static string[] player =
        {
"██████╗░██╗░░░░░░█████╗░██╗░░░██╗███████╗██████╗░",
"██╔══██╗██║░░░░░██╔══██╗╚██╗░██╔╝██╔════╝██╔══██╗",
"██████╔╝██║░░░░░███████║░╚████╔╝░█████╗░░██████╔╝",
"██╔═══╝░██║░░░░░██╔══██║░░╚██╔╝░░██╔══╝░░██╔══██╗",
"██║░░░░░███████╗██║░░██║░░░██║░░░███████╗██║░░██║",
"╚═╝░░░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░╚══════╝╚═╝░░╚═╝"
        };
        public static string[] monkey =
        {
"░░░░░██╗░█████╗░██╗░░██╗███████╗  ███╗░░░███╗░█████╗░███╗░░██╗██╗░░██╗███████╗██╗░░░██╗",
"░░░░░██║██╔══██╗██║░██╔╝██╔════╝  ████╗░████║██╔══██╗████╗░██║██║░██╔╝██╔════╝╚██╗░██╔╝",
"░░░░░██║██║░░██║█████═╝░█████╗░░  ██╔████╔██║██║░░██║██╔██╗██║█████═╝░█████╗░░░╚████╔╝░",
"██╗░░██║██║░░██║██╔═██╗░██╔══╝░░  ██║╚██╔╝██║██║░░██║██║╚████║██╔═██╗░██╔══╝░░░░╚██╔╝░░",
"╚█████╔╝╚█████╔╝██║░╚██╗███████╗  ██║░╚═╝░██║╚█████╔╝██║░╚███║██║░╚██╗███████╗░░░██║░░░",
"░╚════╝░░╚════╝░╚═╝░░╚═╝╚══════╝  ╚═╝░░░░░╚═╝░╚════╝░╚═╝░░╚══╝╚═╝░░╚═╝╚══════╝░░░╚═╝░░░"
        };
        public static string[] guess =
        {
"░██████╗░██╗░░░██╗███████╗░██████╗░██████╗",
"██╔════╝░██║░░░██║██╔════╝██╔════╝██╔════╝",
"██║░░██╗░██║░░░██║█████╗░░╚█████╗░╚█████╗░",
"██║░░╚██╗██║░░░██║██╔══╝░░░╚═══██╗░╚═══██╗",
"╚██████╔╝╚██████╔╝███████╗██████╔╝██████╔╝",
"░╚═════╝░░╚═════╝░╚══════╝╚═════╝░╚═════╝░"
        };
        public static string[] crowscii =
        {
"                        ██████                      ",
"                      ████████████████              ",
"                    ████████████████████            ",
"        ██████      ████████████████████████        ",
"    ██████▒▒████    ██████████████████████████      ",
"      ████████████████████████████████████████      ",
"        ████████████████████████████████████████    ",
"          ████████████████████████████████████████  ",
"  ████████████████████████████████████████████████  ",
"██████████████████████████████████  ██  ████████████",
"██████████████████████████████████            ██████",
"████████████████████████████████                    ",
"  ████████████████████████████                      ",
"  ████████████████████████████                      ",
"  ██████████████████████████████                    ",
"  ██████████████████████████████████                ",
"    ██████████████        ████████████              ",
"      ████████████          ████████████            ",
"      ████████████          ██████████████          ",
"      ████████████            ██████████            ",
"        ██████████              ████████            ",
"          ████████                ████              ",
"            ████                                    ",
"              ██                                    "
        };
        public static string[] monkscii =
        {
            "   .-\"-.  ",
" _/.-.-.\\_",
"( ( o o ) )",
" |/  \"  \\|",
"  \\ .-. / ",
"  /`\"\"\"`\\ ",
" /       \\",
        };
        public static string[] hang1 = {
            "     +---+",
            "     |   |",
            "         |",
            "         |",
            "         |",
            "     O   |",
            "==========" 
        };
        public static string[] hang2 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "         |",
            "         |",
            "         |",
            "=========="
        };
        public static string[] hang3 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "     |   |",
            "         |",
            "         |",
            "=========="
        };
        public static string[] hang4 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "     |\\  |",
            "         |",
            "         |",
            "=========="
        };
        public static string[] hang5 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "    /|\\  |",
            "         |",
            "         |",
            "=========="
        };
        public static string[] hang6 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "    /|\\  |",
            "      \\  |",
            "         |",
            "=========="
        };
        public static string[] hang7 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "    /|\\  |",
            "    / \\  |",
            "         |",
            "=========="
        };
        public static string[] hang8 =
        {
            "     +---+",
            "     |   |",
            "     O   |",
            "    /|\\  |",
            "\\_/‾/ \\  |",
            "         |",
            "=========="
        };

        static void Main(string[] args)
        {
            /*foreach (string line in File.ReadLines(@"word.txt", Encoding.UTF8))
            {
                all.Add(line);
            }*/
            LockWindowSize();
            Initialise();
            MonkeyGuess();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.CursorVisible = false;
            space = true;
            for (int i = 0; i < 4; i++)
            {
                write = main[i];
                Write();
            }

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 5; i++)
            {
                write = garg[i];
                Write();
            }
        }

        public static void Initialise()
        {
            space = false;

            //hangmen
            /*
            for (int i = 0; i < hang8.Length; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 2, 2 + i);
                write = hang8[i];
                Write();
            }
            for (int i = 0; i < hang7.Length; i++)
            {
                Console.SetCursorPosition(2, 2 + i);
                write = hang7[i];
                Write();
            }*/

            //speech box

            for (int i = 0; i < 9; i++)
            {
                Console.SetCursorPosition(4, Console.WindowHeight - 11 + i);
                wascii = "|";
                ASCIIDraw();
                Console.SetCursorPosition(Console.WindowWidth - 5, Console.WindowHeight - 11 + i);
                ASCIIDraw();
            }
            for (int i = 0; i < Console.WindowWidth - 10; i++)
            {
                Console.SetCursorPosition(5 + i, Console.WindowHeight - 12);
                wascii = "_";
                ASCIIDraw();
                Console.SetCursorPosition(5 + i, Console.WindowHeight - 3);
                ASCIIDraw();
            }
        }

        public static void MonkeyGuess()
        {
            space = false;
            for (int i = 0; i < monkscii.Length; i++)
            {
                Console.SetCursorPosition(7, Console.WindowHeight - 10 + i);
                wascii = monkscii[i];
                ASCIIDraw();
            }

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < monkey.Length; i++)
                {
                    if (j == 0)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        break;
                    }
                    else if (j == 1)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        if (i==1)
                        break;
                    }
                    else if (j == 2)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        if (i == 2)
                            break;
                    }
                    else if (j == 3)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        if (i == 3)
                            break;
                    }
                    else if (j == 4)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        if (i == 4)
                            break;
                    }
                    else if (j == 5)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), j - i);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                        if (i == 5)
                            break;
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[i].Length / 2), 5 - i + j);
                        wascii = monkey[monkey.Length - 1 - i];
                        ASCIIDraw();
                    }
                    Thread.Sleep(100);
                }
                if (j > 5)
                {
                        invalid = monkey[0];
                        Console.SetCursorPosition(Console.WindowWidth / 2 - (monkey[0].Length / 2), Console.CursorTop - 1);
                        Invalid();
                }
            }

            Console.ReadKey();
        }

        public static void Write()
        {
            for (int i = 0; i < write.Length; i++)
            {
                Console.Write(write[i]);
                Thread.Sleep(15);
            }
            Thread.Sleep(300);

            if (space)
            {
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        public static void ASCIIDraw()
        {
            for (int i = 0; i < wascii.Length; i++)
            {
                Console.Write(wascii[i]);
            }
        }

        public static void Invalid()
        {
            for (int i = 0; i < invalid.Length; i++)
            {
                Console.Write(" ");
            }
        }

        private static void LockWindowSize()
        {
            Console.WindowHeight = 30;
            Console.WindowWidth = 90;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }
    }
}
