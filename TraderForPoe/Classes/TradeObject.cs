using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace TraderForPoe.Classes
{
    public enum TradeTypeEnum { BUY, SELL };

    public class TradeObject
    {
        public static ObservableCollection<TradeObject> TradeObjectList { get; set; } = new ObservableCollection<TradeObject>();

        /// <summary>
        /// Constructor for the TradeObject
        /// </summary>
        public TradeObject(string whisper)
        {
            this.whisper = whisper;
            ParseWhisper(whisper);
            TradeObjectList.Add(this);
        }

        private TradeTypeEnum tradeType;

        public TradeTypeEnum TradeType
        {
            get { return tradeType; }
            set { tradeType = value; }
        }

        private string whisper;

        public string Whisper
        {
            get { return whisper; }
            set { whisper = value; }
        }


        private string customer;

        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        private string league;

        public string League
        {
            get { return league; }
            set { league = value; }
        }

        private string stash;

        public string Stash
        {
            get { return stash; }
            set { stash = value; }
        }

        private Point position;

        /// <summary>
        /// The item position in stash as point
        /// </summary>
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        private Item item;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        private string additionalText;

        public string AdditionalText
        {
            get { return additionalText; }
            set { additionalText = value; }
        }

        private void ParseWhisper(string whisper)
        {
            Regex poeTradeRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*) [(]stash tab \"(?<stashName>.*)[\"]; position: left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeTradeUnpricedRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) in (?<league>.*) [(]stash tab \"(?<stashName>.*)[\"]; position: left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeTradeNoLocationRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*) (?<additionalText>.*)");
            Regex poeTradeCurrencyRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I'd like to buy your (?<amountItem>[0-9]*[.]?[0-9]+) (?<item>.*) for my (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*)[.](?<additionalText>.*)");

            Regex poeAppRegEx = new Regex("@(?<messageType>.*) (?<customer>.*): wtb (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*) [(]stash \"(?<stashName>.*)[\"]; left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeAppUnpricedRegex = new Regex("@(?<messageType>.*) (?<customer>.*): wtb (?<item>.*) in (?<league>.*) [(]stash \"(?<stashName>.*)[\"]; left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeAppCurrencyRegex = new Regex("@(?<messageType>.*) (?<customer>.*): I'd like to buy your (?<amountItem>[0-9]*[.]?[0-9]+) (?<item>.*) for my (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*)[.](?<additionalText>.*)");

            if (poeTradeRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.league = match.Groups["league"].Value;

                    this.stash = match.Groups["stashName"].Value;

                    this.position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (poeTradeUnpricedRegex.IsMatch(whisper) && !whisper.Contains("listed for"))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, 1);

                    this.league = match.Groups["league"].Value;

                    this.stash = match.Groups["stashName"].Value;

                    this.position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (poeTradeNoLocationRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeNoLocationRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.league = match.Groups["league"].Value;

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }

            }
            else if (poeTradeCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, decimal.Parse(match.Groups["amountItem"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.league = match.Groups["league"].Value;

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }


            }
            else if (poeAppRegEx.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppRegEx.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.league = match.Groups["league"].Value;

                    this.stash = match.Groups["stashName"].Value;

                    this.position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (!whisper.Contains("listed for") && poeAppUnpricedRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, 1);

                    this.league = match.Groups["league"].Value;

                    this.stash = match.Groups["stashName"].Value;

                    this.position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (!whisper.Contains("Hi, ") && poeAppCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    this.tradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.customer = match.Groups["customer"].Value;

                    this.item = new Item(match.Groups["item"].Value, decimal.Parse(match.Groups["amountItem"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.league = match.Groups["league"].Value;

                    this.additionalText = match.Groups["additionalText"].Value.Trim();
                }


            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private TradeTypeEnum GetTradeType(string arg)
        {
            if (arg.ToLower() == "to")
            {
                return TradeTypeEnum.BUY;
            }
            else if (arg.ToLower() == "from")
            {
                return TradeTypeEnum.SELL;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            //TODO Korrekt implementieren
            return customer + "\n" + item.ItemAsString + "\n" + tradeType + "\n" + Stash;
        }

        public static bool IsLogTradeWhisper(string arg)
        {
            if ((arg.Contains("@From") || arg.Contains("@To") && (arg.Contains("d like to buy your ") || arg.Contains(": wtb "))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsTradeWhisper(string arg)
        {
            if (arg.StartsWith("@") && (arg.Contains("d like to buy your ") || arg.Contains(": wtb ")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ItemExists(TradeObject arg)
        {
            foreach (var to in TradeObjectList)
            {
                if (to.Item == arg.Item &&
                    to.Customer == arg.Customer &&
                    to.Item.Price.Amount == arg.Item.Price.Amount &&
                    to.Position == arg.Position &&
                    to.TradeType == arg.TradeType)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RemoveItemFromList(TradeObject arg)
        {
            for (int i = 0; i < TradeObjectList.Count; i++)
            {
                if (TradeObjectList[i].Item == arg.Item &&
                    TradeObjectList[i].Customer == arg.Customer &&
                    TradeObjectList[i].Item.Price.Amount == arg.Item.Price.Amount &&
                    TradeObjectList[i].Position == arg.Position &&
                    TradeObjectList[i].TradeType == arg.TradeType)
                {
                    TradeObjectList.RemoveAt(i);
                }
            }
        }
    }







}

