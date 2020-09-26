using System.Collections.Generic;

namespace PhaticBot.Models
{
    public static class Dict
    {
        private static readonly List<string> AnswerGreeting = new List<string>
        {
            "Hello! How do you do? ☺️", "Oh, nice to meet you! How are you? 😊", "Hey, bro! Wassup? 😎",
            "Hi, tell something about you 😊"
        };

        private static readonly List<string> AnswerFarewell = new List<string>
        {
            "See you 😊️", "Goodbye, see you soon 🙃", "Bye, bye!"
        };

        private static readonly List<string> AnswerUnknownADVP = new List<string>
        {
            "🤪 {0}, {0}, everyone ask {0}. Ask something else!", "Don't ask {0}, if you don't know half of answer ☝️",
            "Everyone ask me {0}, but I'll not say it 😷", "Ha-ha, you ask {0}, it's a secret 👽",
            "Ask {0} to yourself ☝️🐺",
        };

        //0 - NP (apple), 1 - VP (is), 2 - ADJP (very tasty)
        private static readonly List<string> AnswerAdjectives = new List<string>
        {
            "Why {1} {0} {2} 🤔?", "Mmm... couldn`t agree more. 👾 {1} {0} really {2} ?",
            "Are you sure that {0} {1} {2} 😲?",
            "As for me, {0} {1} not {2}, are you sure 🧐?", "I can't agree that {0} {1} {2}, can you argue? 😎",
            "Why do you think that {0} {1} {2}? 🙄", "Yes, {0} {1} {2}, can you tell more about it? 😊",
            "Ha-ha, yes, {0} {1} {2}, can you tell more about it? 😅", "Can you argue that {0} {1} {2}? 🤓",
            "{0} {1} {2}, really?! 😱", "{0} {1} not {2} as for me, are you joking? 😳",
            "Hmmm, {0} {1} {2}, {0} {1} {2}, do you think so? 🤨"
        };

        //Small sentence from user
        private static readonly List<string> AnswerSmall = new List<string>
        {
            "Tell more about it 🤔", "Do not be shy, say more 😉", "You are not talkative today, tell me more 🙃",
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
        private static readonly List<string> Asking = new List<string>
        {
            "Sure, I {0}, what about you?",
            "Ohh, I wonder if you think I {0}",
            "It doesn't matter if I {0}, quantum physics is important",
            "Let's go drink beer than talk about whether I {0}",
            "Do you really need to know if I {0}",
            "I'm ashamed to say it, but I don't {0}"
        };

        // 0 - NP
        private static readonly List<string> AnswerNP = new List<string>
        {
            "{0} is an interesting topic to discuss, btw!",
            "I have a bad experience with {0}, let's change the topic",
            "Oh, {0}, I talked about it today. Maybe change topic?", "Ha-ha, {0}, classic. What do you think about it?",
            "Hmmm, {0}, you are romantic. Continue...", "Okay, you really want to speak about {0}?",
            "Yes, {0}, but what the next?", "Don't ask me about {0}.",
            "Hmmm, {0}, interesting, my father told me about it...",
            "Look around! Now is 2020! And you talk about {0}.", "Oh, {0}, this topic not for me.",
            "Okay, bro, let's talk about {0}",
            "Yes, we can speak about {0}, but for what?"
        };
        
        private static readonly List<string> AnswerADJP = new List<string>
        {
            "Yeees, I think it's {0} too! But what do you want to say?", "Oh, yes, it's {0}, and what?",
            "Okay, bro, all of us know what this is {0}, and what do you want to add?",
            "I don't agree that this is {0}, can you argue?", "Yes, it's {0}, you didn't know it?",
            "Oh myyy, I didn't know that this is {0}! Tell me more about it!", 
            "Ha-ha, at night I'm {0} too, and what?"
        };
        
        
        private static readonly List<string> AnswerVP = new List<string>
        {
            "I'm very curious that you {0}, tell me more", 
            " You should be proud that you {0}",
            "It's great that you shared with me that you {0}",
            "I never thought about {0}, so I don't really understand it",
            "Somehow it is not usual that we talk about it",
            "Dude, it's very cool that you {0} continue at the same pace",
            "You never know what {0} is next, you have to be alert",
        };

        public static readonly Dictionary<SentenceType, List<string>> Replies =
            new Dictionary<SentenceType, List<string>>
            {
                [SentenceType.NP_VP_ADJP] = AnswerAdjectives,
                [SentenceType.PP_NP] = Asking,
                [SentenceType.VP_NP] = Asking,
                [SentenceType.NP] = AnswerNP,
                [SentenceType.VP] = AnswerVP,
                [SentenceType.ADVP] = AnswerUnknownADVP,
                [SentenceType.ADJP] = AnswerADJP,
                [SentenceType.Greeting] = AnswerGreeting,
                [SentenceType.Farewell] = AnswerFarewell,
                [SentenceType.None] = AnswerNone,
                [SentenceType.Small] = AnswerSmall,
            };

        public static readonly Dictionary<SentenceType, List<string>> GroupNames =
            new Dictionary<SentenceType, List<string>>
            {
                [SentenceType.NP_VP_ADJP] = new List<string> {"NP", "VP", "ADJP"},
                // [SentenceType.PP_NP] = new List<string> {"PP", "NP"},
                // [SentenceType.VP_NP] = new List<string> {"VP", "NP"},
                [SentenceType.ADVP] = new List<string> {"ADVP"},
                [SentenceType.ADJP] = new List<string> {"ADJP"},
                [SentenceType.NP] = new List<string> {"NP"},
                [SentenceType.VP] = new List<string> {"VP"},
                [SentenceType.Greeting] = new List<string>(),
                [SentenceType.Farewell] = new List<string>(),
                [SentenceType.None] = new List<string>(),
                [SentenceType.Small] = new List<string>(),
            };
    }
}