﻿using AuroraEmu.DI.Database.DAO;
using AuroraEmu.DI.Game.Wordfilter;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuroraEmu.Game.Wordfilter
{
    public class WordfilterController : IWordfilterController
    {
        private readonly List<Models.Wordfilter> _filteredWords;
        public IWordfilterDao Dao { get; }

        public WordfilterController(IWordfilterDao dao)
        {
            Dao = dao;
            _filteredWords = new List<Models.Wordfilter>();

            Init();
        }

        public void Init()
        {
            Dao.WordfilterData(_filteredWords);
            Engine.Logger.Info($"Loaded {_filteredWords.Count} filtered words.");
        }

        public string CheckString(string message)
        {
            foreach (Models.Wordfilter filter in _filteredWords.ToList())
            {
                if (message.ToLower().Contains(filter.Word) || message == filter.Word)
                {
                    message = Regex.Replace(message, filter.Word, filter.ReplacementWord, RegexOptions.IgnoreCase);
                }
            }
            return message;
        }
    }
}