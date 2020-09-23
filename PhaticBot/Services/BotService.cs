using System;
using System.Collections.Generic;
using System.Linq;
using OpenNLP.Tools.Tokenize;

namespace PhaticBot.Services
{
    public class BotService
    {
        private static readonly EnglishRuleBasedTokenizer Tokenizer = new EnglishRuleBasedTokenizer(false);

        private static readonly Random random = new Random();

        private static readonly List<string> Greeting = new List<string>
            {
                "hi" , "hey",  "hello" , "good morning", "welcome", "how do you do", "good evening", "good afternoon", 
                "It's nice to meet you", "G'day mate", "Hey", "Hi", "Good to see you",
            };
        
        private static readonly List<string> Farewell = new List<string>
        {
            "goodbye" , "bye", "see you"
        };
        private static readonly string[] Shy =
        {
            "Tell more about it", "Can you explain?", "Do not be shy, say more", "You are not talkative today",
            "Tell me more", "Say more"
        };

        private List<string> expression = new List<string>()
        {
            "agree", "decide", "deserve", "expect", "hope", "learn", "need", "offer", "plan", "promise",
            "seem", "wait", "want", "admit", "advise", "avoid", "consider", "deny", "involve", "mention",
            "recommend", "like", "hate", "love", "know", "feel", "risk", "suggest"
        };

        private List<string> Asking = new List<string>()
        {
            "Oh, it's interesting, tell more about it", "i don't quite agree with you, argue your point",
            "Are you sure about this?"
        };
        

        public string Receive(string msg)
        {
            string[] tokens = Tokenizer.Tokenize(msg);

            foreach (var token in tokens)
            {
                if (Greeting.Contains(token))
                {
                    return Greeting[random.Next(Greeting.Count)];
                }
                else if (Farewell.Contains(token))
                {
                    return Farewell[random.Next(Farewell.Count)];
                }
                else if (tokens.Length <= 3)
                {
                    return Shy[random.Next(Shy.Length)];
                }
                else if (tokens.Last() == "?" && MsgCheck(tokens, "you" ) != "")
                {
                    if (MsgCheck(tokens, expression) != "")
                    {
                        //return "Are you really interesting in it?";
                        return "Why you ask about me? Let's speak about you";
                    }
                    else
                    {
                        return "I'm Ok, and you?";
                    }
                }
                else if (tokens.Last() != "?")
                {
                    if (MsgCheck(tokens, expression) != "")
                    {
                        return "Why do u love/hate too?";
                    }
                    else
                    {
                        return Asking[random.Next(Asking.Count)];
                    }
                }
            }

            return $"I am not yet able to answer the question {msg}. Wait for updates:-)";
        }

        private string MsgCheck(string[] tokens, string word)
        {
            foreach (var token in tokens)
            {
                if (token == word)
                {
                    return token;
                }
            }

            return "";
        }
        
        private string Checking(string token, List<string> search)
        {
            foreach (var word in search )
            {
                if (word == token)
                {
                    return token;
                }
            }

            return "";
        }

        private string MsgCheck(string[] tokens, List<string> anyList)
        {
            foreach (var word in anyList)
            {
                foreach (var token in tokens)
                {
                    if (token == word)
                    {
                        return token;
                    }
                }
            }

            return "";
        }

        public string IReceive(string msg)
        {
            string[] tokens = Tokenizer.Tokenize(msg);

            if (tokens.Last().ToLower() == "?")
            {
                if (tokens.Length <= 3)
                {
                    return GetRandomElement(Shy);
                }
            }

            foreach (var word in expression)
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].ToLower() == word)
                    {
                        return $"Why do you {tokens[i]} {tokens[i + 1]}";
                    }
                }
            }

            return $"{msg} - Currently this is a difficult question for me and I don't know the answer, wait for updates:-)";
        }

        private string GetRandomElement(string[] arr)
        {
            return arr[random.Next(0, arr.Length)];
        }
    }
}