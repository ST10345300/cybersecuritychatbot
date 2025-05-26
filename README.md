# 🛡️ CyberSecurityChatbot update part  2 prog 6221

A beginner-friendly C# console chatbot that teaches users about various cybersecurity topics. The bot responds to greetings, questions, and keywords like "phishing", "password", or "malware" with helpful advice. It also plays a welcome sound and simulates natural typing in responses.

## 📌 Features

- ✅ Answers cybersecurity-related questions (e.g., passwords, VPNs, 2FA, phishing).
- 🎵 Plays a WAV audio greeting at startup.
- 🎨 Uses colorful console text and ASCII borders for enhanced user experience.
- 💬 Recognizes greetings, thanks, sentiment (happy/sad), and follow-up requests like "tell me more".
- 🔁 Tracks previous topic mentions and cycles through different responses for each topic.
- 🚪 Users can type `exit`, `quit`, or `bye` to end the conversation.

## 🧱 Topics Covered

- Passwords  
- Phishing  
- Two-Factor Authentication (2FA)  
- Malware  
- Ransomware  
- VPNs  
- Firewalls  
- Encryption  
- Data Breaches  
- Social Engineering  
- Safe Browsing  

## 🛠️ Requirements

- .NET SDK (e.g., .NET 6 or higher)
- A console/terminal that supports color output
- `poee.wav` audio file in the same directory as the executable

## 🚀 How to Run

1. **Clone or Download** the repository.
2. Make sure the `poee.wav` file is in the same folder as your `.cs` file or compiled `.exe`.
3. Compile the code:
   ```bash
   csc CyberSecurityChatbot.cs
