// Cybersecurity Awareness Chatbot - PROG6221 POE Part 1
// Author: Kunta

using System;
using System.Media;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace CyberAwarenessBot
{
    class Program
    {
        static Dictionary<string, string> knowledgeBase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"how are you", "I'm just code, but I'm running great! Let's talk cybersecurity."},
            {"what's your purpose", "I'm here to help you understand cybersecurity threats and how to avoid them."},
            {"what can i ask you about", "You can ask about phishing, passwords, safe browsing, malware, identity theft, and more."},
            {"phishing", "🔍 Great choice! Let's dive into phishing.\nPhishing is a type of cyberattack where attackers impersonate legitimate institutions to steal sensitive information like usernames, passwords, and credit card numbers.\n✅ Tip: Always verify links and never click attachments from unknown sources.\n💡 Fun Fact: 90% of data breaches start with a phishing email."},
            {"passwords", "🔐 Strong pick! Let's talk about passwords.\nUsing weak or reused passwords is one of the most common security flaws. Create strong passwords using a mix of letters, numbers, and symbols.\n✅ Tip: Use a password manager to remember complex passwords for you.\n💡 Fun Fact: The most common password in 2023 was '123456'. Avoid it!"},
            {"safe browsing", "🌐 Browsing smart is key!\nSafe browsing means avoiding harmful websites and suspicious popups. Look for the HTTPS padlock on sites that require sensitive information.\n✅ Tip: Don't save passwords on public computers.\n💡 Fun Fact: Over 30% of users click pop-up ads thinking they’re legitimate."},
            {"malware", "🛡️ Let's break down malware.\nMalware is malicious software that can infect your devices. It includes viruses, spyware, ransomware, and more.\n✅ Tip: Keep your antivirus software up to date and avoid shady downloads.\n💡 Fun Fact: The first malware ever created was called 'Creeper' in the 1970s!"},
            {"identity theft", "🧾 Important topic!\nIdentity theft happens when someone uses your personal info to commit fraud. It can ruin credit scores and finances.\n✅ Tip: Regularly check your financial statements and use strong privacy settings online.\n💡 Fun Fact: Children are often targets for identity theft due to clean credit histories."},
            {"social engineering", "🎭 Interesting pick!\nSocial engineering tricks people into giving up confidential data. It's more about manipulating human behavior than hacking systems.\n✅ Tip: Always verify the identity of someone requesting information.\n💡 Fun Fact: The term 'social engineering' was popularized by hackers in the 1990s."},
            {"why is cybersecurity important", "🌍 Let’s get into it.\nCybersecurity is important because it protects your personal data, devices, and networks from attacks and unauthorized access.\n✅ Fact: A cyberattack happens every 39 seconds globally.\n💡 Fun Fact: The global cybersecurity market is expected to reach $500 billion by 2030!"},
            {"how do i know if i've been hacked", "🚨 Let's uncover the signs.\nYou may have been hacked if you notice password changes, unknown apps, or suspicious activity on your accounts.\n✅ Tip: Set up alerts for unusual login activity and enable two-factor authentication.\n💡 Fun Fact: 60% of small businesses go out of business within six months of a cyberattack."}
        };

        static string[] tips = new string[]
        {
            "💡 Tip: Never share your passwords with anyone, even people you trust.",
            "💡 Tip: Use a password manager to store your passwords securely.",
            "💡 Tip: Always log out of your accounts on shared computers.",
            "💡 Tip: Don’t click suspicious links in emails or texts.",
            "💡 Tip: Keep your software and operating system updated."
        };

        static void Main(string[] args)
        {
            PlayVoiceGreeting();
            ShowAsciiArt();
            GreetUser();
            StartChatLoop();
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "poee.wav");
                using (SoundPlayer player = new SoundPlayer(path))
                {
                    player.PlaySync();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error playing greeting audio] " + e.Message);
                Console.ResetColor();
            }
        }

        static void ShowAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           🔐 CYBERSECURITY AWARENESS BOT                      ║");
            Console.WriteLine("║   " + DateTime.Now.ToString("dddd, dd MMM yyyy").PadRight(56) + "║");
            Console.WriteLine("║        Stay Alert. Stay Safe. Stay Smart.                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void GreetUser()
        {
            Console.Write("\nWhat's your name? ");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Please enter a valid name: ");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            Console.WriteLine($"\nWelcome, {name}! Let's talk about staying safe online.\n");
        }

        static void StartChatLoop()
        {
            string input;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Ask me something about cybersecurity (or type 'exit'): ");
                Console.ResetColor();

                input = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();

                if (input == "exit")
                {
                    TypeResponse("Goodbye! Stay safe online.");
                    break;
                }

                if (knowledgeBase.ContainsKey(input))
                {
                    TypeResponse(knowledgeBase[input]);
                    ShowRandomTip();
                    SuggestFollowUps();
                }
                else
                {
                    TypeResponse("I'm not trained to answer that, but I can help with cybersecurity topics. Here are some suggestions:");
                    SuggestFollowUps();
                }

                Console.WriteLine();
            } while (true);
        }

        static void SuggestFollowUps()
        {
            string[] suggestions = new string[]
            {
                "phishing", "passwords", "safe browsing",
                "malware", "identity theft", "social engineering",
                "why is cybersecurity important", "how do I know if I've been hacked"
            };

            Console.WriteLine("Here are some suggestions you can ask:");
            for (int i = 0; i < suggestions.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {suggestions[i]}");
            }
            Console.Write("\nPick a number or ask your own question: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(choice, out int index) && index > 0 && index <= suggestions.Length)
            {
                string chosenQuestion = suggestions[index - 1];
                if (knowledgeBase.ContainsKey(chosenQuestion))
                {
                    TypeResponse(knowledgeBase[chosenQuestion]);
                    ShowRandomTip();
                    SuggestFollowUps();
                }
            }
            else if (!string.IsNullOrWhiteSpace(choice) && knowledgeBase.ContainsKey(choice))
            {
                TypeResponse(knowledgeBase[choice]);
                ShowRandomTip();
                SuggestFollowUps();
            }
        }

        static void ShowRandomTip()
        {
            Random rand = new Random();
            int index = rand.Next(tips.Length);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + tips[index] + "\n");
            Console.ResetColor();
        }

        static void TypeResponse(string message)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(25);
            }
            Console.WriteLine();
        }
    }
}
