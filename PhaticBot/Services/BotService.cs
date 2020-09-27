﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenNLP.Tools;
using OpenNLP.Tools.Chunker;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Tokenize;
using PhaticBot.Models;

namespace PhaticBot.Services
{
    public class BotService
    {
        #region Fields

        private static readonly EnglishRuleBasedTokenizer Tokenizer = new EnglishRuleBasedTokenizer(false);

        private static readonly Random Random = new Random();

        private static readonly string ChunkModelPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "NLP-Models", "EnglishChunk.nbin");

        private static readonly EnglishTreebankChunker Chunker = new EnglishTreebankChunker(ChunkModelPath);

        private static readonly string PosModelPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "NLP-Models", "EnglishPOS.nbin");

        private static readonly EnglishMaximumEntropyPosTagger PosTagger =
            new EnglishMaximumEntropyPosTagger(PosModelPath);

        private static readonly string[] Punct = {".", ",", "!", ":"};
        
        //0 - NP (apple), 1 - VP (is), 2 - ADJP (very tasty)
        private static readonly List<string> AnswerAdjectives = new List<string>
        {
            "Why {1} {0} {2} 🤔?", "Mmm... couldn`t agree more. 👾 {1} {0} really {2} ?", "Are you sure that {0} {1} {2} 😲?", 
            "As for me, {0} {1} not {2}, are you sure 🧐?", "I can't agree that {0} {1} {2}, can you argue? 😎", 
            "Why do you think that {0} {1} {2}? 🙄", "Yes, {0} {1} {2}, can you tell more about it? 😊",
            "Ha-ha, yes, {0} {1} {2}, can you tell more about it? 😅", "Can you argue that {0} {1} {2}? 🤓", 
            "{0} {1} {2}, really?! 😱", "{0} {1} not {2} as for me, are you joking? 😳",
            "Hmmm, {0} {1} {2}, {0} {1} {2}, do you think so? 🤨"
        };
        
        //Small sentence from user
        private static readonly List<string> AnswerSmall = new List<string>
        {
            "Tell more about it 🤔", "Do not be shy, say more 😉",  "You are not talkative today, tell me more 🙃",
            "Okay, okay, and what? 😏", "Thats all what you want to say? 🙃", "Ok, tell me more about it 😏"
        };
        
        //None
        private static readonly List<string> AnswerNone = new List<string>
        {
            "Sorry, but this is exciting topic for me, can we change it? 🥶", 
            "Sure, we can talk about it. But maybe better you will tell me how you spend your day? 🙄",
            "Yes, we can speak about it. But maybe you will tell me why you do nothing and chat with me now? 🤗",
            "Oh, came on. I haven't mood to talk about it, tell me what you plan to do tomorrow? 📆",
            "Okay,okay, I understand your interest in this topic, but can we speak about more attract topics? 😅",
            "You are fourth person who raises this topic today with me 😮, can we change theme? 🤠", 
            "Ha-ha, classic! 😂 Can you invent something else?",
            "No, no, no! I don't want to hear this. Don't waste my time, next question? 😌",
            "I don't want to upset you, but this is boring. 🤦‍♂️ Can you tell something else? "
        };
        
        // 0 - PP or VP (like, help), 1 - NP (me, beer, pizza)
        private static readonly List<string> Asking = new List<string>()
        {
            "Sure, I {0} {1}, what about you?",
            "Ohh, I wonder if you think I {0} {1}",
            "It doesn't matter if I {0} {1}, quantum physics is important",
            "Let's go drink beer than talk about whether I {0} {1}",
            "Do you really need to know if I {0} {1}",
            "I'm ashamed to say it, but I don't {0} {1}"
        };
        
        private static readonly Dictionary<SentenceType, List<string>> Replies = 
            new Dictionary<SentenceType, List<string>>
        {
            [SentenceType.NP_VP_ADJP] = AnswerAdjectives,
            [SentenceType.None] = AnswerNone,
            [SentenceType.Small] = AnswerSmall,
            [SentenceType.PP_NP] = Asking,
            [SentenceType.VP_NP] = Asking
        };
        
        private static readonly Dictionary<SentenceType, List<string>> GroupNames = 
            new Dictionary<SentenceType, List<string>>
        {
            [SentenceType.NP_VP_ADJP] = new List<string> {"NP", "VP", "ADJP"},
            [SentenceType.None] = new List<string>(),
            [SentenceType.Small] = new List<string>(),
            [SentenceType.PP_NP] = new List<string>() {"PP", "NP"},
            [SentenceType.PP_NP] = new List<string>() {"VP", "NP"},
        };
        
        #endregion

        private static string GetRandomReply(SentenceType type, IEnumerable<SentenceChunk> chunks)
        {
            chunks = chunks.Take(GroupNames[type].Count);

            return string.Format(Replies[type][Random.Next(Replies[type].Count)],
                chunks.Select(chunk => string.Join(" ", chunk.TaggedWords.Select(tw => tw.Word.ToLower()))).ToArray());
        }

        private static SentenceType GetSentenceType(List<SentenceChunk> chunks)
        {
            if (chunks.Count <= 2)
            {
                return SentenceType.Small;
            }

            foreach (var (type, names) in GroupNames)
            {
                for (int i = 0; i < chunks.Count; i++)
                {
                    if (chunks.Skip(i).Take(names.Count).Select(chunk => chunk.Tag).SequenceEqual(names))
                    {
                        return type;
                    }
                }
            }

            return SentenceType.None;
        }
        
        private static string Reply(List<SentenceChunk> chunks)
        {
            return GetRandomReply(GetSentenceType(chunks), chunks);
        }

        private static List<SentenceChunk> GetChunks(string msg)
        {
            var tokens = Tokenizer.Tokenize(msg)
                .Where(token => !Punct.Contains(token)).ToArray();
  
            return Chunker.GetChunks(tokens, PosTagger.Tag(tokens));
        }

        public static string Receive(string msg)
        {
            var chunks = GetChunks(msg);

            return Reply(chunks);
        }
    }
}