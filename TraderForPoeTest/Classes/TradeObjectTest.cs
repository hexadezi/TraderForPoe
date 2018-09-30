using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraderForPoe.Classes;
using TraderForPoe;
using System.Windows;

namespace TraderForPoeTest
{
    [TestClass]
    public class TradeObjectTest
    {
        /// <summary>
        /// Is needed for the registration of the needed components for the uri
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var app = new App();
            app.InitializeComponent();
        }

        /// <summary>
        /// Tests for poe.trade
        /// </summary>
        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeTradeAllTest()
        {
            string whisper = "@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed for 1 jewellers in Bestiary (stash tab \"~b/o 0 alt\"; position: left 23, top 8)";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeTradeNoStashTest()
        {
            string whisper = "@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed for 1 jewellers in Bestiary";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeTradeNoPriceTest()
        {
            string whisper = "@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed in Bestiary (stash tab \"~b / o 0 alt\"; position: left 23, top 8)";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeTradeBuyCurrencyTest()
        {
            string whisper = "@To Labooooooo: Hi, I'd like to buy your 260 chaos for my 2 exalted in Bestiary.";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Tests for poe.app
        /// </summary>
        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeAppAllTest()
        {
            string whisper = "@To Labooooooo: wtb Cybil's Paw Thresher Claw listed for 1 Orb of Chance in bestiary (stash \"BIG\"; left 5, top 3)";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeAppNoPriceTest()
        {
            string whisper = "@To Labooooooo: wtb Cybil's Paw Thresher Claw in bestiary (stash \"BIG\"; left 5, top 3)";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckIfWhisperIsTradeWhisperPoeAppBuyCurrencyTest()
        {
            string whisper = "@To Labooooooo: I'd like to buy your 260 Chaos Orb for my 2 Exalted Orb in bestiary.";
            bool actual = TradeObject.IsLogTradeWhisper(whisper);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Check if method parses the whispers correctly
        /// </summary>
        [TestMethod]
        public void ParseWhisperPoeTradeRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed for 1.5 jewellers in Bestiary (stash tab \"~b / o 0 alt\"; position: left 23, top 8) canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(1, to.Item.Amount);
            Assert.AreEqual("Cybil's Paw Thresher Claw", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.UNKNOWN, to.Item.ItemAsType);
            Assert.AreEqual(Convert.ToDecimal(1.5), to.Item.Price.Amount);
            Assert.AreEqual(ItemType.JEWELLER, to.Item.Price.ItemAsType);
            Assert.IsNotNull(to.Item.Price.Image);
            Assert.AreEqual("Bestiary", to.League);
            Assert.AreEqual("~b / o 0 alt", to.Stash);
            Assert.AreEqual(new Point(23, 8), to.Position);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeTradeUnpricedRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed in Bestiary (stash tab \"~b / o 0 alt\"; position: left 23, top 8) canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(1, to.Item.Amount);
            Assert.AreEqual("Cybil's Paw Thresher Claw", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.UNKNOWN, to.Item.ItemAsType);
            Assert.AreEqual(null, to.Item.Price);
            Assert.AreEqual("Bestiary", to.League);
            Assert.AreEqual("~b / o 0 alt", to.Stash);
            Assert.AreEqual(new Point(23, 8), to.Position);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeTradeNoLocationRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed for 1.5 jewellers in Bestiary canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(1, to.Item.Amount);
            Assert.AreEqual("Cybil's Paw Thresher Claw", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.UNKNOWN, to.Item.ItemAsType);
            Assert.AreEqual(Convert.ToDecimal(1.5), to.Item.Price.Amount);
            Assert.AreEqual(ItemType.JEWELLER, to.Item.Price.ItemAsType);
            Assert.IsNotNull(to.Item.Price.Image);
            Assert.AreEqual("Bestiary", to.League);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeTradeCurrencyRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: Hi, I'd like to buy your 260.9 chaos for my 2.5 exalted in Bestiary. canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(Convert.ToDecimal(260.9), to.Item.Amount);
            Assert.AreEqual("chaos", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.CHAOS, to.Item.ItemAsType);
            Assert.AreEqual(Convert.ToDecimal(2.5), to.Item.Price.Amount);
            Assert.AreEqual(ItemType.EXALTED, to.Item.Price.ItemAsType);
            Assert.IsNotNull(to.Item.Price.Image);
            Assert.AreEqual("Bestiary", to.League);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeAppRegExBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: wtb Cybil's Paw Thresher Claw listed for 1.5 Orb of Chance in bestiary (stash \"BIG\"; left 5, top 3) canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(1, to.Item.Amount);
            Assert.AreEqual("Cybil's Paw Thresher Claw", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.UNKNOWN, to.Item.ItemAsType);
            Assert.AreEqual(Convert.ToDecimal(1.5), to.Item.Price.Amount);
            Assert.AreEqual(ItemType.CHANCE, to.Item.Price.ItemAsType);
            Assert.IsNotNull(to.Item.Price.Image);
            Assert.AreEqual("bestiary", to.League);
            Assert.AreEqual("BIG", to.Stash);
            Assert.AreEqual(new Point(5, 3), to.Position);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeAppUnpricedRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: wtb Cybil's Paw Thresher Claw in bestiary (stash \"BIG\"; left 5, top 3) canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(1, to.Item.Amount);
            Assert.AreEqual("Cybil's Paw Thresher Claw", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.UNKNOWN, to.Item.ItemAsType);
            Assert.AreEqual(to.Item.Price, null);
            Assert.AreEqual("bestiary", to.League);
            Assert.AreEqual("BIG", to.Stash);
            Assert.AreEqual(new Point(5, 3), to.Position);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

        [TestMethod]
        public void ParseWhisperPoeAppCurrencyRegexBuy()
        {
            TradeObject to = new TradeObject("@To Labooooooo: I'd like to buy your 260.4 Chaos Orb for my 2 Exalted Orb in bestiary. canyou");
            Assert.AreEqual(TradeTypeEnum.BUY, to.TradeType);
            Assert.AreEqual("Labooooooo", to.Customer);
            Assert.AreEqual(Convert.ToDecimal(260.4), to.Item.Amount);
            Assert.AreEqual("Chaos Orb", to.Item.ItemAsString);
            Assert.AreEqual(ItemType.CHAOS, to.Item.ItemAsType);
            Assert.AreEqual(2, to.Item.Price.Amount);
            Assert.AreEqual(ItemType.EXALTED, to.Item.Price.ItemAsType);
            Assert.IsNotNull(to.Item.Price.Image);
            Assert.AreEqual("bestiary", to.League);
            Assert.AreEqual("canyou", to.AdditionalText);
        }

    }
}