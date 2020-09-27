using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenNLP.Tools;
using OpenNLP.Tools.Chunker;
using OpenNLP.Tools.Ling;
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
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "NLP-Models",
                "EnglishChunk.nbin");

        private static readonly EnglishTreebankChunker Chunker = new EnglishTreebankChunker(ChunkModelPath);

        private static readonly string PosModelPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "NLP-Models",
                "EnglishPOS.nbin");

        private static readonly EnglishMaximumEntropyPosTagger PosTagger =
            new EnglishMaximumEntropyPosTagger(PosModelPath);

        private static readonly string[] Punct = {".", ",", "!", ":"};

        #endregion

        private static string GetRandomReply(SentenceType type, IEnumerable<SentenceChunk> chunks, int toSkip = 0)
        {
            var sentenceChunks = chunks as SentenceChunk[] ?? chunks.ToArray();

            if (sentenceChunks.Last().TaggedWords.First().Word == "?")
            {
                chunks = sentenceChunks.SkipLast(toSkip).TakeLast(Dict.GroupNames[type].Count);
            }
            else
            {
                chunks = sentenceChunks.Skip(toSkip).Take(Dict.GroupNames[type].Count);
            }

            return string.Format(Dict.Replies[type][Random.Next(Dict.Replies[type].Count)],
                chunks.Select(chunk => string.Join(" ", chunk.TaggedWords.Select(tw => tw.Word.ToLower()))).ToArray());
        }

        private static (SentenceType type, int toSkip) GetSentenceType(List<SentenceChunk> chunks)
        {
            chunks.ForEach(ch => FindPronoun(ch));
            
            if (IsGreeting(chunks))
            {
                return (SentenceType.Greeting, 0);
            }

            if (IsFarewell(chunks))
            {
                return (SentenceType.Farewell, 0);
            }

            if (chunks.Count <= 2)
            {
                return (SentenceType.Small, 0);
            }

            if (chunks.Last().TaggedWords.First().Word == "?")
            {
                foreach (var (type, names) in Dict.GroupNames)
                {
                    for (int i = 0; i < chunks.Count; i++)
                    {
                        var tags = chunks.SkipLast(i).TakeLast(names.Count).Select(chunk => chunk.Tag);
                        if (tags.SequenceEqual(names))
                        {
                            return (type, i);
                        }
                    }
                }
            }
            else
            {
                foreach (var (type, names) in Dict.GroupNames)
                {
                    for (int i = 0; i < chunks.Count; i++)
                    {
                        var tags = chunks.Skip(i).Take(names.Count).Select(chunk => chunk.Tag);
                        if (tags.SequenceEqual(names))
                        {
                            return (type, i);
                        }
                    }
                }
            }

            return (SentenceType.None, 0);
        }

        private static string Reply(List<SentenceChunk> chunks)
        {
            (var type, var toSkip) = GetSentenceType(chunks);
            return GetRandomReply(type, chunks, toSkip);
        }

        private static List<SentenceChunk> GetChunks(string msg)
        {
            var tokens = Tokenizer.Tokenize(msg)
                .Where(token => !Punct.Contains(token)).ToArray();

            return Chunker.GetChunks(tokens, PosTagger.Tag(tokens));
        }

        public static void FindPronoun(SentenceChunk chunk)
        {
            foreach (var tw in chunk.TaggedWords)
            {
                if (tw.Tag.StartsWith("PRP") || tw.Tag == "UH")
                {
                    chunk.Tag = "XYZ";
                }
            }
        }

        public static bool IsGreeting(List<SentenceChunk> chunks)
        {
            List<string> greetings = new List<string>()
            {
                "hi", "hello", "hey", "yo"
            };
            foreach (var chunk in chunks)
            {
                foreach (var word in chunk.TaggedWords.Select(tw => tw.Word))
                {
                    foreach (var greeting in greetings)
                    {
                        if (word.ToLower() == greeting) return true;
                    }
                }
            }

            return false;
        }

        public static bool IsFarewell(List<SentenceChunk> chunks)
        {
            List<string> farewells = new List<string>()
            {
                "bye", "goodbye", "goodnight"
            };
            foreach (var chunk in chunks)
            {
                foreach (var word in chunk.TaggedWords.Select(tw => tw.Word))
                {
                    foreach (var farewell in farewells)
                    {
                        if (word.ToLower() == farewell) return true;
                    }
                }
            }

            return false;
        }


        public static string Receive(string msg)
        {
            var chunks = GetChunks(msg);

            return Reply(chunks);
        }
    }
}