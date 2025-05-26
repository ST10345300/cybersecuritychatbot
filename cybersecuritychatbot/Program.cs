// Cybersecurity Awareness Chatbot - PROG6221 POE Part 2
// Author: rubben shisso

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Media;

class CyberSecurityChatbot
{
    // Knowledge base with cybersecurity topics and multiple info snippets per topic
    static Dictionary<string, List<string>> topics = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "password", new List<string> {
            "Always use strong, unique passwords for each account.",
            "Consider using a password manager to store your credentials securely.",
            "Never share your password with anyone.",
            "Use a mix of letters, numbers, and special characters in your password."
        }},
        { "phishing", new List<string> {
            "Phishing attacks often appear as legitimate emails or texts. Be cautious!",
            "Never click on suspicious links in emails.",
            "Always verify the sender's address before responding to messages.",
            "Look out for urgent messages that pressure you to act quickly."
        }},
        { "2fa", new List<string> {
            "2FA adds an extra layer of security to your accounts.",
            "Use an authenticator app like Microsoft or Google Authenticator for 2FA.",
            "Enable 2FA wherever possible, especially for important accounts.",
            "Avoid using SMS-based 2FA if more secure options are available."
        }},
        { "malware", new List<string> {
            "Malware can be hidden in attachments or links — always scan files.",
            "Keep your software and antivirus up to date to prevent malware.",
            "Avoid downloading software from untrusted sources.",
            "Use a reputable antivirus program and scan regularly."
        }},
        { "ransomware", new List<string> {
            "Ransomware encrypts your data and demands payment for the key.",
            "Back up your files regularly to protect against ransomware attacks.",
            "Never pay the ransom; report the attack to authorities instead.",
            "Keep your software updated to reduce vulnerabilities."
        }},
        { "vpn", new List<string> {
            "A VPN encrypts your internet traffic and hides your IP address.",
            "Use a trusted VPN to protect your privacy on public Wi-Fi.",
            "Avoid free VPNs that might sell your data.",
            "VPNs help secure your connection but don’t make you invincible."
        }},
        { "firewall", new List<string> {
            "Firewalls monitor and control incoming and outgoing network traffic.",
            "Enable your firewall to block unauthorized access.",
            "Both hardware and software firewalls provide essential security layers.",
            "Keep firewall rules updated to protect against new threats."
        }},
        { "encryption", new List<string> {
            "Encryption scrambles data so only authorized parties can read it.",
            "Use encryption for sensitive files, emails, and communication.",
            "HTTPS websites use encryption to protect your browsing data.",
            "Strong encryption is a key defense against data breaches."
        }},
        { "data breach", new List<string> {
            "A data breach is when sensitive information is accessed without permission.",
            "Change passwords immediately if you suspect a data breach.",
            "Monitor your accounts for suspicious activity regularly.",
            "Use credit monitoring services to detect identity theft early."
        }},
        { "social engineering", new List<string> {
            "Social engineering relies on manipulating people — stay alert.",
            "Be skeptical of unexpected phone calls asking for private info.",
            "Think before you click — even if the source looks familiar.",
            "Double-check any requests for urgent actions involving money or data."
        }},
        { "safe browsing", new List<string> {
            "Always check the website URL before entering sensitive info.",
            "Look for HTTPS in the address bar when browsing securely.",
            "Avoid clicking on ads or pop-ups from unfamiliar sites.",
            "Use privacy-focused browsers or plugins to enhance safety."
        }}
    };

    // Default responses when topic not found
    static List<string> defaultResponses = new List<string>
    {
        "I'm not trained on that yet. Try asking about passwords, phishing, 2FA, malware, ransomware, VPNs, or other cybersecurity topics.",
        "Hmm... that topic isn't in my database. You can ask me about these topics: " + string.Join(", ", topics.Keys) + ".",
        "Interesting! But I only respond to cybersecurity questions for now. Topics I know include: " + string.Join(", ", topics.Keys) + "."
    };

    // Intro phrases before info to vary responses
    static List<string> introPhrases = new List<string>
    {
        "Ahh, sure! Let’s get into it...",
        "Great question! Here’s what I know...",
        "Okay, here’s the scoop...",
        "Let me tell you about that...",
        "Alright, here’s some info for you..."
    };

    // Greeting inputs and responses
    static List<string> greetingInputs = new List<string>
    {
        "hey", "hi", "hello", "how are you", "what's up", "sup"
    };

    static List<string> greetingResponses = new List<string>
    {
        "Hey there! How can I help you with cybersecurity today?",
        "Hello! Ready to learn some cybersecurity tips?",
        "Hi! What would you like to know about staying safe online?",
        "Hey! I'm here to help you with cybersecurity questions."
    };

    // Inputs that mean "tell me more"
    static List<string> tellMeMoreInputs = new List<string>
    {
        "tell me more", "can you explain more", "more info", "explain that", "elaborate"
    };

    static List<string> tellMeMoreResponses = new List<string>
    {
        "Sure! What topic do you want me to explain in more detail?",
        "I'd be happy to explain further. Which area interests you?",
        "Of course! Let me know which cybersecurity topic you'd like to dive deeper into.",
        "Absolutely! Just ask me about passwords, phishing, malware, or anything else."
    };

    // Thanks / thank you inputs and responses
    static List<string> thanksInputs = new List<string>
    {
        "thank you", "thanks", "thx", "thank", "ty"
    };

    static List<string> thanksResponses = new List<string>
    {
        "You're welcome! Happy to help.",
        "No problem! Let me know if you have more questions.",
        "Glad I could assist you!",
        "Anytime! Stay safe out there.",
        "My pleasure! Ask me anything else you want to know."
    };

    // Simple sentiment keywords
    static List<string> positiveWords = new List<string>
    {
        "happy", "good", "great", "awesome", "fantastic", "cool", "well", "fine", "nice", "thankful"
    };

    static List<string> negativeWords = new List<string>
    {
        "sad", "angry", "frustrated", "upset", "bad", "tired", "annoyed", "worried", "confused", "hate"
    };

    // Track last topics user asked about to provide "tell me more"
    static List<string> detectedTopics = new List<string>();
    // Track which info snippet index to provide next per topic
    static Dictionary<string, int> topicInfoIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    static Random rand = new Random();

    static void Main()
    {
        // Play welcome sound once at startup (make sure poee.wav exists)
        try
        {
            using (SoundPlayer player = new SoundPlayer("poee.wav"))
            {
                player.PlaySync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"(Audio file error: {ex.Message})");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║            Welcome to the Cybersecurity Awareness Bot!         ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║    I can answer questions about cybersecurity topics such as   ║");
        Console.WriteLine("║ passwords, phishing, malware, 2FA, ransomware, VPNs, and more!  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine("💬 Type your question or enter 'exit' to quit.\n");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("You: ");
            Console.ResetColor();
            string userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                TypeResponse("I didn’t catch that. Could you rephrase?");
                continue;
            }

            string input = userInput.ToLower();
            string cleanedInput = Regex.Replace(input, @"[^\w\s]", "");  // remove punctuation

            if (input.Contains("bye") || input.Contains("exit") || input.Contains("quit"))
            {
                TypeResponse("Thanks for chatting! Stay cyber safe. 👋");
                break;
            }

            // Sentiment detection - positive or negative words
            string sentimentResponse = DetectSentimentResponse(cleanedInput);
            if (sentimentResponse != null)
            {
                TypeResponse(sentimentResponse);
                continue;
            }
            // Check for greetings
            if (greetingInputs.Any(g => cleanedInput.Contains(g)))
            {
                var greetResp = greetingResponses[rand.Next(greetingResponses.Count)];
                TypeResponse(greetResp);
                continue;
            }

            // Check for thanks
            if (thanksInputs.Any(t => cleanedInput.Contains(t)))
            {
                var thanksResp = thanksResponses[rand.Next(thanksResponses.Count)];
                TypeResponse(thanksResp);
                continue;
            }

            // Check for "tell me more" type phrases
            if (tellMeMoreInputs.Any(tmm => cleanedInput.Contains(tmm)))
            {
                if (detectedTopics.Count > 0)
                {
                    string lastTopic = detectedTopics.Last();
                    ProvideMoreInfo(lastTopic);
                }
                else
                {
                    var tellMoreResp = tellMeMoreResponses[rand.Next(tellMeMoreResponses.Count)];
                    TypeResponse(tellMoreResp);
                }
                continue;
            }

            // Check if user asked about a known topic
            bool topicFound = false;
            foreach (var topic in topics.Keys)
            {
                if (cleanedInput.Contains(topic.ToLower()))
                {
                    detectedTopics.Add(topic);
                    ProvideMoreInfo(topic);
                    topicFound = true;
                    break;
                }
            }

            if (!topicFound)
            {
                // Not found, respond with default
                var defaultResp = defaultResponses[rand.Next(defaultResponses.Count)];
                TypeResponse(defaultResp);
            }
        }
    }

    static void ProvideMoreInfo(string topic)
    {
        if (!topicInfoIndex.ContainsKey(topic))
        {
            topicInfoIndex[topic] = 0;
        }
        int idx = topicInfoIndex[topic];
        var infoList = topics[topic];
        string intro = introPhrases[rand.Next(introPhrases.Count)];
        string response = $"{intro} {infoList[idx]}";

        TypeResponse(response);

        topicInfoIndex[topic] = (idx + 1) % infoList.Count;
    }

    static string DetectSentimentResponse(string input)
    {
        foreach (var word in positiveWords)
        {
            if (input.Contains(word))
            {
                return "Glad to hear that! How else can I assist you?";
            }
        }

        foreach (var word in negativeWords)
        {
            if (input.Contains(word))
            {
                return "Sorry you're feeling that way. How can I help?";
            }
        }

        return null;
    }

    static void TypeResponse(string response)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (char c in response)
        {
            Console.Write(c);
            Thread.Sleep(40);  // typing delay, adjust for speed
        }
        Console.WriteLine();
        Console.ResetColor();
    }
}

