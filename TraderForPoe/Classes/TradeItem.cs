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
        public enum TradeTypes { BUY, SELL };

        public enum PriceCurrency { CHAOS, ALCHCHEMY, ALTERATION, ANCIENT, ANNULMENT, APPRENTICE_SEXTANT, ARMOUR_SCRAP, AUGMENTATION, BAUBLE, BESTIARY_ORB, BINDING_ORB, BLACKSMITH_WHETSTONE, BLESSING_CHAYULAH, BLESSING_ESH, BLESSING_TUL, BLESSING_UUL, BLESSING_XOPH, BLESSE, CHANCE, CHISEL, CHROM, DIVINE, ENGINEER, ETERNAL, EXALTED, FUSING, GEMCUTTERS, HARBINGER_ORB, HORIZON_ORB, IMPRINTED_BESTIARY, JEWELLER, JOURNEYMAN_SEXTANT, MASTER_SEXTANT, MIRROR, PORTAL, REGAL, REGRET, SCOUR, SILVER, SPLINTER_CHAYULA, SPLINTER_ESH, SPLINTER_TUL, SPLINTER_UUL, SPLINTER_XOPH, TRANSMUTE, VAAL, WISDOM };

        Regex poeTradeRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) listed for (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeUnpricedRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeCurrencyRegex = new Regex("@(.*) (.*): Hi, I'd like to buy your (.*) for my (.*) in (.*).(.*)");

        Regex poeAppRegEx = new Regex("@(.*) (.*): wtb (.*) listed for (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppUnpricedRegex = new Regex("@(.*) (.*): wtb (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppCurrencyRegex = new Regex("@(.*) (.*): I'd like to buy your (.*) for my (.*) in (.*).(.*)");




        public TradeItem(string whisper)
        {
            WhisperMessage = whisper;

            SetTradeType(whisper);

            ParseWhisper(WhisperMessage);
        }

        private void SetTradeType(string whisper)
        {
            if (whisper.Contains("@To"))
            {
                this.TradeType = TradeTypes.BUY;
            }
            else if (whisper.Contains("@From"))
            {
                this.TradeType = TradeTypes.SELL;
            }
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

                    this.GetPriceCurrency = ParseAndGetCurrency(this.Price);

                    // Set league
                    this.League = match.Groups[5].Value;

                    // Set stash
                    this.Stash = match.Groups[6].Value;

                    // Set stash position
                    this.StashPosition = new Point(Convert.ToDouble(match.Groups[7].Value), Convert.ToDouble(match.Groups[8].Value));

                    this.AdditionalText = match.Groups[9].Value;


                }
            }
            else if (poeTradeUnpricedRegex.IsMatch(whisper) && !whisper.Contains("listed for"))
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

                    this.GetPriceCurrency = ParseAndGetCurrency(this.Price);

                    // Set league
                    this.League = match.Groups[5].Value;

                    this.AdditionalText = match.Groups[6].Value;

                }

                this.ItemIsCurrency = true;

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

                    this.GetPriceCurrency = ParseAndGetCurrency(this.Price);

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

                    this.GetPriceCurrency = ParseAndGetCurrency(this.Price);

                    // Set league
                    this.League = match.Groups[5].Value; ;

                    this.AdditionalText = match.Groups[6].Value;
                }

                this.ItemIsCurrency = true;

            }
            else
            {
                throw new Exception("No RegEx match");
            }
        }

        private PriceCurrency ParseAndGetCurrency(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                string strPrice = s.ToLower();

                if (strPrice.Contains("chaos") && !strPrice.Contains("shard"))
                {
                    return PriceCurrency.CHAOS;
                }

                else if (strPrice.Contains("alch") && !strPrice.Contains("shard"))
                {
                    return PriceCurrency.ALCHCHEMY;
                }

                else if (strPrice.Contains("alt"))
                {
                    return PriceCurrency.ALTERATION;
                }

                else if (strPrice.Contains("ancient"))
                {
                    return PriceCurrency.ANCIENT;
                }

                else if (strPrice.Contains("annulment") && !strPrice.Contains("shard"))
                {
                    return PriceCurrency.ANNULMENT;
                }

                else if (strPrice.Contains("apprentice") && strPrice.Contains("sextant"))
                {
                    return PriceCurrency.APPRENTICE_SEXTANT;
                }

                else if (strPrice.Contains("armour") || strPrice.Contains("scrap"))
                {
                    return PriceCurrency.ARMOUR_SCRAP;
                }

                else if (strPrice.Contains("aug"))
                {
                    return PriceCurrency.AUGMENTATION;
                }

                else if (strPrice.Contains("bauble"))
                {
                    return PriceCurrency.BAUBLE;
                }

                else if (strPrice.Contains("bestiary") && strPrice.Contains("orb"))
                {
                    return PriceCurrency.BESTIARY_ORB;
                }

                else if (strPrice.Contains("binding") && strPrice.Contains("orb"))
                {
                    return PriceCurrency.BINDING_ORB;
                }

                else if (strPrice.Contains("whetstone") || strPrice.Contains("blacksmith"))
                {
                    return PriceCurrency.BLACKSMITH_WHETSTONE;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("chayulah"))
                {
                    return PriceCurrency.BLESSING_CHAYULAH;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("esh"))
                {
                    return PriceCurrency.BLESSING_ESH;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("tul"))
                {
                    return PriceCurrency.BLESSING_TUL;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("uul"))
                {
                    return PriceCurrency.BLESSING_UUL;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("xoph"))
                {
                    return PriceCurrency.BLESSING_XOPH;
                }

                else if (strPrice.Contains("blesse"))
                {
                    return PriceCurrency.BLESSE;
                }

                else if (strPrice.Contains("chance"))
                {
                    return PriceCurrency.CHANCE;
                }

                else if (strPrice.Contains("chisel"))
                {
                    return PriceCurrency.CHISEL;
                }

                else if (strPrice.Contains("chrom") || strPrice.Contains("chrome"))
                {
                    return PriceCurrency.CHROM;
                }

                else if (strPrice.Contains("divine") || strPrice.Contains("div"))
                {
                    return PriceCurrency.DIVINE;
                }

                else if (strPrice.Contains("engineer") && strPrice.Contains("orb"))
                {
                    return PriceCurrency.ENGINEER;
                }

                else if (strPrice.Contains("ete"))
                {
                    return PriceCurrency.ETERNAL;
                }

                else if (strPrice.Contains("ex") || strPrice.Contains("exa") || strPrice.Contains("exalted") && !strPrice.Contains("shard"))
                {
                    return PriceCurrency.EXALTED;
                }

                else if (strPrice.Contains("fuse") || strPrice.Contains("fus"))
                {
                    return PriceCurrency.FUSING;
                }

                else if (strPrice.Contains("gcp") || strPrice.Contains("gemc"))
                {
                    return PriceCurrency.GEMCUTTERS;
                }

                else if (strPrice.Contains("harbinger") && strPrice.Contains("orb"))
                {
                    return PriceCurrency.HARBINGER_ORB;
                }

                else if (strPrice.Contains("horizon") && strPrice.Contains("orb"))
                {
                    return PriceCurrency.HORIZON_ORB;
                }

                else if (strPrice.Contains("imprinted") && strPrice.Contains("bestiary"))
                {
                    return PriceCurrency.IMPRINTED_BESTIARY;
                }

                else if (strPrice.Contains("jew"))
                {
                    return PriceCurrency.JEWELLER;
                }

                else if (strPrice.Contains("journeyman") && strPrice.Contains("sextant"))
                {
                    return PriceCurrency.JOURNEYMAN_SEXTANT;
                }

                else if (strPrice.Contains("master") && strPrice.Contains("sextant"))
                {
                    return PriceCurrency.MASTER_SEXTANT;
                }

                else if (strPrice.Contains("mir") || strPrice.Contains("kal"))
                {
                    return PriceCurrency.MIRROR;
                }

                else if (strPrice.Contains("port"))
                {
                    return PriceCurrency.PORTAL;
                }

                else if (strPrice.Contains("rega"))
                {
                    return PriceCurrency.REGAL;
                }

                else if (strPrice.Contains("regr"))
                {
                    return PriceCurrency.REGRET;
                }

                else if (strPrice.Contains("scour"))
                {
                    return PriceCurrency.SCOUR;
                }

                else if (strPrice.Contains("silver"))
                {
                    return PriceCurrency.SILVER;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("chayula"))
                {
                    return PriceCurrency.SPLINTER_CHAYULA;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("esh"))
                {
                    return PriceCurrency.SPLINTER_ESH;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("tul"))
                {
                    return PriceCurrency.SPLINTER_TUL;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("uul"))
                {
                    return PriceCurrency.SPLINTER_UUL;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("xoph"))
                {
                    return PriceCurrency.SPLINTER_XOPH;
                }

                else if (strPrice.Contains("tra"))
                {
                    return PriceCurrency.TRANSMUTE;
                }

                else if (strPrice.Contains("vaal"))
                {
                    return PriceCurrency.VAAL;
                }

                else if (strPrice.Contains("wis"))
                {
                    return PriceCurrency.WISDOM;
                }
            }
            throw new Exception("Currency not found");
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

        public string League { get; set; }

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

        public PriceCurrency GetPriceCurrency{ get; set; }


        public bool ItemIsCurrency { get; set; }

        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string ExtractFloatFromString(string s)
        {
            var match = Regex.Match(s, @"([-+]?[0-9]*\.?[0-9]+)");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }
    }

}
