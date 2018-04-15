using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TraderForPoe
{
    public class TradeItem
    {
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

        public Currency PriceCurrency { get; set; }

        public BitmapImage PriceCurrencyBitmap { get; set; }


        public bool ItemIsCurrency { get; set; }

        public enum TradeTypes { BUY, SELL };

        public enum Currency { CHAOS, ALCHCHEMY, ALTERATION, ANCIENT, ANNULMENT, APPRENTICE_SEXTANT, ARMOUR_SCRAP, AUGMENTATION, BAUBLE, BESTIARY_ORB, BINDING_ORB, BLACKSMITH_WHETSTONE, BLESSING_CHAYULAH, BLESSING_ESH, BLESSING_TUL, BLESSING_UUL, BLESSING_XOPH, BLESSE, CHANCE, CHISEL, CHROM, DIVINE, ENGINEER, ETERNAL, EXALTED, FUSING, GEMCUTTERS, HARBINGER_ORB, HORIZON_ORB, IMPRINTED_BESTIARY, JEWELLER, JOURNEYMAN_SEXTANT, MASTER_SEXTANT, MIRROR, PORTAL, REGAL, REGRET, SCOUR, SILVER, SPLINTER_CHAYULA, SPLINTER_ESH, SPLINTER_TUL, SPLINTER_UUL, SPLINTER_XOPH, TRANSMUTE, VAAL, WISDOM };

        Regex poeTradeRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) listed for (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeUnpricedRegex = new Regex("@(.*) (.*): Hi, I would like to buy your (.*) in (.*) [(]stash tab \"(.*)[\"]; position: left (.*), top (.*)[)](.*)");
        Regex poeTradeCurrencyRegex = new Regex("@(.*) (.*): Hi, I'd like to buy your (.*) for my (.*) in (.*).(.*)");

        Regex poeAppRegEx = new Regex("@(.*) (.*): wtb (.*) listed for (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppUnpricedRegex = new Regex("@(.*) (.*): wtb (.*) in (.*) [(]stash \"(.*)[\"]; left (.*), top (.*)[)](.*)");
        Regex poeAppCurrencyRegex = new Regex("@(.*) (.*): I'd like to buy your (.*) for my (.*) in (.*).(.*)");



        // Constructor
        public TradeItem(string whisper)
        {
            WhisperMessage = whisper;

            SetTradeType(WhisperMessage);

            ParseWhisper(WhisperMessage);
        }



        // Set property TradeType
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

                    this.PriceCurrency = ParseCurrency(this.Price);

                    this.PriceCurrencyBitmap = SetCurrencyBitmap(this.PriceCurrency);

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

                    this.PriceCurrency = ParseCurrency(this.Price);

                    this.PriceCurrencyBitmap = SetCurrencyBitmap(this.PriceCurrency);

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

                    this.PriceCurrency = ParseCurrency(this.Price);

                    this.PriceCurrencyBitmap = SetCurrencyBitmap(this.PriceCurrency);

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

                    this.PriceCurrency = ParseCurrency(this.Price);

                    this.PriceCurrencyBitmap = SetCurrencyBitmap(this.PriceCurrency);

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

        private Currency ParseCurrency(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                string strPrice = s.ToLower();

                if (strPrice.Contains("chaos") && !strPrice.Contains("shard"))
                {
                    return Currency.CHAOS;
                }

                else if (strPrice.Contains("alch") && !strPrice.Contains("shard"))
                {
                    return Currency.ALCHCHEMY;
                }

                else if (strPrice.Contains("alt"))
                {
                    return Currency.ALTERATION;
                }

                else if (strPrice.Contains("ancient"))
                {
                    return Currency.ANCIENT;
                }

                else if (strPrice.Contains("annulment") && !strPrice.Contains("shard"))
                {
                    return Currency.ANNULMENT;
                }

                else if (strPrice.Contains("apprentice") && strPrice.Contains("sextant"))
                {
                    return Currency.APPRENTICE_SEXTANT;
                }

                else if (strPrice.Contains("armour") || strPrice.Contains("scrap"))
                {
                    return Currency.ARMOUR_SCRAP;
                }

                else if (strPrice.Contains("aug"))
                {
                    return Currency.AUGMENTATION;
                }

                else if (strPrice.Contains("bauble"))
                {
                    return Currency.BAUBLE;
                }

                else if (strPrice.Contains("bestiary") && strPrice.Contains("orb"))
                {
                    return Currency.BESTIARY_ORB;
                }

                else if (strPrice.Contains("binding") && strPrice.Contains("orb"))
                {
                    return Currency.BINDING_ORB;
                }

                else if (strPrice.Contains("whetstone") || strPrice.Contains("blacksmith"))
                {
                    return Currency.BLACKSMITH_WHETSTONE;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("chayulah"))
                {
                    return Currency.BLESSING_CHAYULAH;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("esh"))
                {
                    return Currency.BLESSING_ESH;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("tul"))
                {
                    return Currency.BLESSING_TUL;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("uul"))
                {
                    return Currency.BLESSING_UUL;
                }

                else if (strPrice.Contains("blessing") && strPrice.Contains("xoph"))
                {
                    return Currency.BLESSING_XOPH;
                }

                else if (strPrice.Contains("blesse"))
                {
                    return Currency.BLESSE;
                }

                else if (strPrice.Contains("chance"))
                {
                    return Currency.CHANCE;
                }

                else if (strPrice.Contains("chisel"))
                {
                    return Currency.CHISEL;
                }

                else if (strPrice.Contains("chrom") || strPrice.Contains("chrome"))
                {
                    return Currency.CHROM;
                }

                else if (strPrice.Contains("divine") || strPrice.Contains("div"))
                {
                    return Currency.DIVINE;
                }

                else if (strPrice.Contains("engineer") && strPrice.Contains("orb"))
                {
                    return Currency.ENGINEER;
                }

                else if (strPrice.Contains("ete"))
                {
                    return Currency.ETERNAL;
                }

                else if (strPrice.Contains("ex") || strPrice.Contains("exa") || strPrice.Contains("exalted") && !strPrice.Contains("shard"))
                {
                    return Currency.EXALTED;
                }

                else if (strPrice.Contains("fuse") || strPrice.Contains("fus"))
                {
                    return Currency.FUSING;
                }

                else if (strPrice.Contains("gcp") || strPrice.Contains("gemc"))
                {
                    return Currency.GEMCUTTERS;
                }

                else if (strPrice.Contains("harbinger") && strPrice.Contains("orb"))
                {
                    return Currency.HARBINGER_ORB;
                }

                else if (strPrice.Contains("horizon") && strPrice.Contains("orb"))
                {
                    return Currency.HORIZON_ORB;
                }

                else if (strPrice.Contains("imprinted") && strPrice.Contains("bestiary"))
                {
                    return Currency.IMPRINTED_BESTIARY;
                }

                else if (strPrice.Contains("jew"))
                {
                    return Currency.JEWELLER;
                }

                else if (strPrice.Contains("journeyman") && strPrice.Contains("sextant"))
                {
                    return Currency.JOURNEYMAN_SEXTANT;
                }

                else if (strPrice.Contains("master") && strPrice.Contains("sextant"))
                {
                    return Currency.MASTER_SEXTANT;
                }

                else if (strPrice.Contains("mir") || strPrice.Contains("kal"))
                {
                    return Currency.MIRROR;
                }

                else if (strPrice.Contains("port"))
                {
                    return Currency.PORTAL;
                }

                else if (strPrice.Contains("rega"))
                {
                    return Currency.REGAL;
                }

                else if (strPrice.Contains("regr"))
                {
                    return Currency.REGRET;
                }

                else if (strPrice.Contains("scour"))
                {
                    return Currency.SCOUR;
                }

                else if (strPrice.Contains("silver"))
                {
                    return Currency.SILVER;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("chayula"))
                {
                    return Currency.SPLINTER_CHAYULA;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("esh"))
                {
                    return Currency.SPLINTER_ESH;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("tul"))
                {
                    return Currency.SPLINTER_TUL;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("uul"))
                {
                    return Currency.SPLINTER_UUL;
                }

                else if (strPrice.Contains("splinter") && strPrice.Contains("xoph"))
                {
                    return Currency.SPLINTER_XOPH;
                }

                else if (strPrice.Contains("tra"))
                {
                    return Currency.TRANSMUTE;
                }

                else if (strPrice.Contains("vaal"))
                {
                    return Currency.VAAL;
                }

                else if (strPrice.Contains("wis"))
                {
                    return Currency.WISDOM;
                }
            }
            throw new Exception("Currency not found");
        }

        private BitmapImage SetCurrencyBitmap(Currency c)
        {
            switch (PriceCurrency)
            {
                case Currency.CHAOS:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chaos.png"));
                case Currency.ALCHCHEMY:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_alch.png"));
                case Currency.ALTERATION:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_alt.png"));
                case Currency.ANCIENT:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_ancient.png"));
                case Currency.ANNULMENT:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_annul.png"));
                case Currency.APPRENTICE_SEXTANT:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_appr_carto_sextant.png"));
                case Currency.ARMOUR_SCRAP:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_armour_scrap.png"));
                case Currency.AUGMENTATION:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_aug.png"));
                case Currency.BAUBLE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bauble.png"));
                case Currency.BESTIARY_ORB:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bestiary_orb.png"));
                case Currency.BINDING_ORB:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_binding.png"));
                case Currency.BLACKSMITH_WHETSTONE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_black_whetstone.png"));
                case Currency.BLESSING_CHAYULAH:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_chayula.png"));
                case Currency.BLESSING_ESH:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_chayula.png"));
                case Currency.BLESSING_TUL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_tul.png"));
                case Currency.BLESSING_UUL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_uul.png"));
                case Currency.BLESSING_XOPH:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_xoph.png"));
                case Currency.BLESSE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_blessed.png"));
                case Currency.CHANCE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chance.png"));
                case Currency.CHISEL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chisel.png"));
                case Currency.CHROM:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chrom.png"));
                case Currency.DIVINE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_divine.png"));
                case Currency.ENGINEER:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_engineer.png"));
                case Currency.ETERNAL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_eternal.png"));
                case Currency.EXALTED:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_ex.png"));
                case Currency.FUSING:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_fuse.png"));
                case Currency.GEMCUTTERS:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_gcp.png"));
                case Currency.HARBINGER_ORB:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_harbinger.png"));
                case Currency.HORIZON_ORB:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_horizon.png"));
                case Currency.IMPRINTED_BESTIARY:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_impr_bestiary.png"));
                case Currency.JEWELLER:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_jew.png"));
                case Currency.JOURNEYMAN_SEXTANT:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_journ_carto_sextant.png"));
                case Currency.MASTER_SEXTANT:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_master_carto_sextant.png"));
                case Currency.MIRROR:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_mirror.png"));
                case Currency.PORTAL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_port.png"));
                case Currency.REGAL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_regal.png"));
                case Currency.REGRET:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_regret.png"));
                case Currency.SCOUR:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_scour.png"));
                case Currency.SILVER:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_silver.png"));
                case Currency.SPLINTER_CHAYULA:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_chayula.png"));
                case Currency.SPLINTER_ESH:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_esh.png"));
                case Currency.SPLINTER_TUL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_tul.png"));
                case Currency.SPLINTER_UUL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_uul.png"));
                case Currency.SPLINTER_XOPH:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_xoph.png"));
                case Currency.TRANSMUTE:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_tra.png"));
                case Currency.VAAL:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_vaal.png"));
                case Currency.WISDOM:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_wis.png"));
                default:
                    throw new ArgumentException();
            }
        }

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
