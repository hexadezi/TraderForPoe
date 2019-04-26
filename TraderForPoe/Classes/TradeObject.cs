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
            this.Whisper = whisper;
            ParseWhisper(whisper);
            TradeObjectList.Add(this);
        }

        public TradeTypeEnum TradeType { get; set; }

        public string Whisper { get; set; }

        public string Customer { get; set; }

        public string League { get; set; }

        public string Stash { get; set; }

        /// <summary>
        /// The item position in stash as point
        /// </summary>
        public Point Position { get; set; }

        public Item Item { get; set; }

        public string AdditionalText { get; set; }

        private void ParseWhisper(string whisper)
        {
            Regex poeTradeRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*) [(]stash tab \"(?<stashName>.*)[\"]; position: left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeTradeUnpricedRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) in (?<league>.*) [(]stash tab \"(?<stashName>.*)[\"]; position: left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeTradeNoLocationRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I would like to buy your (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>[^\\s]+)(?<additionalText>.*)");
            Regex poeTradeCurrencyRegex = new Regex("@(?<messageType>.*) (?<customer>.*): Hi, I'd like to buy your (?<amountItem>[0-9]*[.]?[0-9]+) (?<item>.*) for my (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*)[.](?<additionalText>.*)");

            Regex poeAppRegEx = new Regex("@(?<messageType>.*) (?<customer>.*): wtb (?<item>.*) listed for (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*) [(]stash \"(?<stashName>.*)[\"]; left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeAppUnpricedRegex = new Regex("@(?<messageType>.*) (?<customer>.*): wtb (?<item>.*) in (?<league>.*) [(]stash \"(?<stashName>.*)[\"]; left (?<stashPositionX>[0-9]*), top (?<stashPositionY>[0-9]*)[)](?<additionalText>.*)");
            Regex poeAppCurrencyRegex = new Regex("@(?<messageType>.*) (?<customer>.*): I'd like to buy your (?<amountItem>[0-9]*[.]?[0-9]+) (?<item>.*) for my (?<amountPrice>[0-9]*[.]?[0-9]+) (?<itemPrice>.*) in (?<league>.*)[.](?<additionalText>.*)");

            if (poeTradeRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.League = match.Groups["league"].Value;

                    this.Stash = match.Groups["stashName"].Value;

                    this.Position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (poeTradeUnpricedRegex.IsMatch(whisper) && !whisper.Contains("listed for"))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, 1);

                    this.League = match.Groups["league"].Value;

                    this.Stash = match.Groups["stashName"].Value;

                    this.Position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (poeTradeNoLocationRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeNoLocationRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.League = match.Groups["league"].Value;

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }

            }
            else if (poeTradeCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeTradeCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, decimal.Parse(match.Groups["amountItem"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.League = match.Groups["league"].Value;

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }


            }
            else if (poeAppRegEx.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppRegEx.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, 1)
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.League = match.Groups["league"].Value;

                    this.Stash = match.Groups["stashName"].Value;

                    this.Position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (!whisper.Contains("listed for") && poeAppUnpricedRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppUnpricedRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, 1);

                    this.League = match.Groups["league"].Value;

                    this.Stash = match.Groups["stashName"].Value;

                    this.Position = new Point(Convert.ToInt16(match.Groups["stashPositionX"].Value), Convert.ToInt16(match.Groups["stashPositionY"].Value));

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }
            }
            else if (!whisper.Contains("Hi, ") && poeAppCurrencyRegex.IsMatch(whisper))
            {
                MatchCollection matches = Regex.Matches(whisper, poeAppCurrencyRegex.ToString());

                foreach (Match match in matches)
                {
                    this.TradeType = GetTradeType(match.Groups["messageType"].Value);

                    this.Customer = match.Groups["customer"].Value;

                    this.Item = new Item(match.Groups["item"].Value, decimal.Parse(match.Groups["amountItem"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    {
                        // Set price
                        Price = new ItemBase(match.Groups["itemPrice"].Value, decimal.Parse(match.Groups["amountPrice"].Value, NumberStyles.Any, CultureInfo.InvariantCulture))
                    };

                    this.League = match.Groups["league"].Value;

                    this.AdditionalText = match.Groups["additionalText"].Value.Trim();
                }


            }
            else
            {
                //TODO Korrekt implementieren
                throw new NotImplementedException();
            }
        }

        private TradeTypeEnum GetTradeType(string arg)
        {
            if (arg.ToLower().StartsWith("to"))
            {
                return TradeTypeEnum.BUY;
            }
            else if (arg.ToLower().StartsWith("from"))
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
            return Customer + "\n" + Item.ItemAsString + "\n" + TradeType + "\n" + Stash;
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
            if (arg.StartsWith("@") && (arg.Contains("d like to buy your ") || arg.Contains(" wtb ")))
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

