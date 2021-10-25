using Xunit;
using GildedRose.Console;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        Program app;

        public TestAssemblyTests()
        {
            var _app = new Program
            {
                Items = new List<Item>
                {
                    new Common {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                    new Cheese {Name = "Aged Brie", SellIn = 2, Quality = 0},
                    new Common {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                    new Legendary {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                    new ConcertTicket {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
                    new Conjured {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                }
            };
            this.app = _app;
        }

        [Fact]
        public void TestTheTruth()
        {
            Assert.True(true);
        }
        [Fact]
        public void QualityDecreasesWhenAbove0_AndIsNot_Brie_Backstage_Sulfuras()
        {
            app.UpdateQuality();
            var expected = 19;
            var actual = app.Items.Where(i => i.Name == "+5 Dexterity Vest").FirstOrDefault().Quality;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void QualityStaysAt_0_AndIsNot_Brie_Backstage_Sulfuras()
        {
            for (int i = 0; i < 7; i++)
            {
                app.UpdateQuality();
            }

            var expected = 0;
            var actual = app.Items.Where(i => i.Name == "Conjured Mana Cake").FirstOrDefault().Quality;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AgedBrie_QualityIncreases_Below50()
        {
            app.UpdateQuality();
            var expected = 1;
            var actual = app.Items.Where(i => i.Name == "Aged Brie").FirstOrDefault().Quality;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AgedBrie_Quality_Does_Not_Increase_above_50()
        {
            for (int i = 0; i < 51; i++)
            {
                app.UpdateQuality();
            }

            app.UpdateQuality();
            var expected = 50;
            var actual = app.Items.Where(i => i.Name == "Aged Brie").FirstOrDefault().Quality;
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SellIn_Decreases_Except_Sulfuras()
        {
            app.UpdateQuality();

            Assert.Equal(9, app.Items.ElementAt(0).SellIn);
            Assert.Equal(1, app.Items.ElementAt(1).SellIn);
            Assert.Equal(4, app.Items.ElementAt(2).SellIn);
            Assert.Equal(14, app.Items.ElementAt(4).SellIn);
            Assert.Equal(2, app.Items.ElementAt(5).SellIn);
        }
        [Fact]
        public void Backstagepass_Increases_Drastically()
        {
            var item = app.Items.Where(i => i.Name == "Backstage passes to a TAFKAL80ETC concert").FirstOrDefault();

            //Quality = 21
            //SellIn = 15
            app.UpdateQuality();

            //Quality = 22
            //SellIn = 14
            app.UpdateQuality();

            //Quality = 23
            //SellIn = 13
            app.UpdateQuality();

            //Quality = 24
            //SellIn = 12
            app.UpdateQuality();

            //Quality = 25
            //SellIn = 11
            app.UpdateQuality();

            Assert.Equal(25, item.Quality);


            app.UpdateQuality();
            //Quality = 27
            //SellIn = 10

            app.UpdateQuality();
            //Quality = 29
            //SellIn = 9

            app.UpdateQuality();
            //Quality = 31
            //SellIn = 8

            app.UpdateQuality();
            //Quality = 33
            //SellIn = 7

            app.UpdateQuality();
            //Quality = 35
            //SellIn = 6

            Assert.Equal(35, item.Quality);

            app.UpdateQuality();
            //Quality = 38
            //SellIn = 5

            Assert.Equal(38, item.Quality);
        }
        [Fact]
        public void QualityDecreasesTwiceBelowZero_Except_Brie_Sulfur_BackStage()
        {
            app.UpdateQuality();
            //Quality = 5

            app.UpdateQuality();
            //Quality = 4

            app.UpdateQuality();
            //Quality = 3

            // Mana Bun At Zero
            app.UpdateQuality();
            //Quality = 2

            // Mana Bun Negative
            app.UpdateQuality();
            //Quality = 0

            Assert.Equal(0, app.Items.Where(i => i.Name == "Conjured Mana Cake").FirstOrDefault().Quality);
        }
        [Fact]
        public void QualityCannotBeNegative()
        {
            app.UpdateQuality();
            //Quality = 5

            app.UpdateQuality();
            //Quality = 4

            app.UpdateQuality();
            //Quality = 3

            // Mana Bun At Zero
            app.UpdateQuality();
            //Quality = 2

            // Mana Bun Negative
            app.UpdateQuality();
            //Quality = 0

            app.UpdateQuality();
            //Quality = 0
            Assert.Equal(0, app.Items.Where(i => i.Name == "Conjured Mana Cake").FirstOrDefault().Quality);
        }
        [Fact]
        public void BackStageQuality_IsZero_When_SellInNegative()
        {
            for (int i = 0; i < 16; i++)
            {
                app.UpdateQuality();
            }
            Assert.Equal(0, app.Items.Where(c => c.Name == "Backstage passes to a TAFKAL80ETC concert").FirstOrDefault().Quality);
        }
        [Fact]
        public void AgedBrie_QualityDoubleIncrease()
        {
            var cheese = app.Items.Where(c => c.Name == "Aged Brie").FirstOrDefault();

            app.UpdateQuality();
            app.UpdateQuality();
            Assert.Equal(2, cheese.Quality);
            app.UpdateQuality();
            Assert.Equal(4, cheese.Quality);
        }

        [Fact]
        public void ConjuredItem_Decrease_Twice_As_Fast_As_Common()
        {
            var conjured = app.Items.Where(c => c.Name == "Conjured Mana Cake").FirstOrDefault();

            app.UpdateQuality();

            Assert.Equal(4, conjured.Quality);
        }
    }
}