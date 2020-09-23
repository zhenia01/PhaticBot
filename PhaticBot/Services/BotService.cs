using System;
using System.Collections.Generic;
using System.Linq;
using OpenNLP.Tools.Tokenize;

namespace PhaticBot.Services
{
    public class BotService
    {
        private static readonly EnglishRuleBasedTokenizer Tokenizer = new EnglishRuleBasedTokenizer(false);

        private static readonly Random Random = new Random();

        private static readonly List<string> Greeting = new List<string>
            {
                "hi" , "hey",  "hello" , "good morning", "welcome", "how do you do", "good evening", "good afternoon", 
                "It's nice to meet you", "Good day mate", "Hey", "Hi", "Good to see you",
            };
        
        private static readonly List<string> Farewell = new List<string>
        {
            "goodbye" , "bye", "see you"
        };
        private static readonly List<string> Shy = new List<string>()
        {
            "Tell more about it", "Can you explain?", "Do not be shy, say more", "You are not talkative today",
            "Tell me more", "Say more"
        };

        private static readonly List<string> Expression = new List<string>()
        {
            "agree", "decide", "deserve", "expect", "hope", "learn", "need", "offer", "plan", "promise",
            "seem", "wait", "want", "admit", "advise", "avoid", "consider", "deny", "involve", "mention",
            "recommend", "like", "hate", "love", "know", "feel", "risk", "suggest"
        };

        private static readonly List<string> Asking = new List<string>()
        {
            "Oh, it's interesting, tell more about it", "i don't quite agree with you, argue your point",
            "Are you sure about this?"
        };
        

        public string Receive(string msg)
        {
            var tokens = Tokenizer.Tokenize(msg);

            foreach (var token in tokens)
            {
                if (Greeting.Contains(token))
                {
                    return Greeting[Random.Next(Greeting.Count)];
                }
                else if (Farewell.Contains(token))
                {
                    return Farewell[Random.Next(Farewell.Count)];
                }
                else if (tokens.Length <= 3)
                {
                    return Shy[Random.Next(Shy.Count)];
                }
                else if (tokens.Last() == "?" && WordCheck(tokens, "you" ) != "")
                {
                    if (MsgCheck(tokens, Expression) != "")
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
                    if (MsgCheck(tokens, Expression) != "")
                    {
                        return "Why do u love/hate too?";
                    }
                    else
                    {
                        return Asking[Random.Next(Asking.Count)];
                    }
                }
            }
            return $"I am not yet able to answer the question {msg}. Wait for updates:-)";
        }

        private static string WordCheck(IEnumerable<string> tokens, string word)
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
        
        private static string MsgCheck(string[] tokens, List<string> anyList)
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
    }
}