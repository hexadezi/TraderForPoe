using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TraderForPoe
{
    public class TradeItem
    {
        public enum TradeTypes{BUY, SELL};

        Regex poeTradeRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) listed for (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeUnpricedRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeCurrencyRegex = new Regex("@(.*) (.*): Hi, I'd like to buy your (.*) for my (.*) in (.*).(.*)");

        Regex poeAppRegEx = new Regex("@(.*) (.*): wtb (.*) listed for (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppUnpricedRegex = new Regex("@(.*) (.*): wtb (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppCurrencyRegex = new Regex("@(.*) (.*): I'd like to buy your (.*) for my (.*) in (.*).(.*)");

        public TradeItem(string whisper)
        {
            WhisperMessage = whisper;

            if (whisper.Contains("@To"))
            {
                this.TradeType = TradeTypes.BUY;
            }
            else if (whisper.Contains("@From"))
            {
                this.TradeType = TradeTypes.SELL;
            }

            ParseWhisper(WhisperMessage);
        }


        private void ParseWhisper(string whisper)
        {

            if (poeTradeRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeRegex.ToString());

                foreach (Match match in matches)
                {
                    // 
                    this.Customer = match.Groups[2].Value;
                    
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set price
                    this.Price = match.Groups[4].Value;

                    // Set league
                    this.League = match.Groups[5].Value;

                    // Set stash
                    this.Stash = match.Groups[6].Value;

                    // Set stash position
                    this.StashPosition = new Point(Convert.ToDouble(match.Groups[7].Value), Convert.ToDouble(match.Groups[8].Value));

                    this.AdditionalText = match.Groups[9].Value;


                }
            }
            else if (poeTradeUnpricedRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set league
                    this.League = match.Groups[4].Value;

                    // Set stash
                    this.Stash = match.Groups[5].Value;

                    // Set stash position
                    this.StashPosition = new Point(Convert.ToDouble(match.Groups[6].Value), Convert.ToDouble(match.Groups[7].Value));

                    //this.AdditionalText = match.Groups[8].Value;
                }
            }
            else if (poeTradeCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set price
                    this.Price = match.Groups[4].Value;

                    // Set league
                    this.League = match.Groups[5].Value;

                    this.AdditionalText = match.Groups[6].Value;

                }
            }
            else if (poeAppRegEx.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppRegEx.ToString());

                foreach (Match match in matches)
                {
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set price
                    this.Price = match.Groups[4].Value;

                    // Set league
                    this.League = match.Groups[5].Value;

                    // Set stash
                    this.Stash = match.Groups[6].Value;

                    // Set stash position
                    this.StashPosition = new Point(Convert.ToDouble(match.Groups[7].Value), Convert.ToDouble(match.Groups[8].Value));

                    this.AdditionalText = match.Groups[9].Value;
                }
            }
            else if (!whisper.Contains("listed for") && poeAppUnpricedRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set league
                    this.League = match.Groups[4].Value;

                    // Set stash
                    this.Stash = match.Groups[5].Value;

                    // Set stash position
                    this.StashPosition = new Point(Convert.ToDouble(match.Groups[6].Value), Convert.ToDouble(match.Groups[7].Value));

                    this.AdditionalText = match.Groups[8].Value;
                }
            }
            else if (!whisper.Contains("Hi, ") && poeAppCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    // Set customer
                    this.Customer = match.Groups[2].Value;

                    // Set item
                    this.Item = match.Groups[3].Value;

                    // Set price
                    this.Price = match.Groups[4].Value;

                    // Set league
                    this.League = match.Groups[5].Value; ;

                    this.AdditionalText = match.Groups[6].Value;
                }
            }
            else
            {
                throw new Exception("No RegEx match");
            }
        }

        public TradeTypes TradeType
        {
            get;
            set;
        }
        public string Customer
        {
            get;
            set;
        }

        public string Item
        {
            get;
            set;
        }

        public string Price
        {
            get;
            set;
        }

        public string AdditionalText
        {
            get;
            set;
        }

        private string league;
        public string League
        {
            get { return league; }
            set { league = FirstCharToUpper(value); }
        }

        public string Stash
        {
            get;
            set;
        }

        public Point StashPosition
        {
            get;
            set;
        }

        public string WhisperMessage
        {
            get;
            set;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

    }

}
