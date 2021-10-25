using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console
{
    public class Program
    {
        // TODO: Find a better way to do this
        public IList<Item> Items;
        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
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

            app.UpdateQuality();
            System.Console.ReadKey();
        }

        public void UpdateQuality()
        {
            foreach (var i in Items)
            {
                i.Update();
            }
        }
    }

    public abstract class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }

        public void Update()
        {
            UpdateSellIn();
            UpdateQuality();
        }

        public virtual void UpdateSellIn()
        {
            SellIn--;
        }
        abstract public void UpdateQuality();

        protected void IncreaseQuality(int amount)
        {
            if (Quality < 50) Quality += amount;
            Quality = Math.Min(Quality, 50);
        }

        protected void DecreaseQuality(int amount)
        {
            if (Quality > 0) Quality -= amount;
            Quality = Math.Max(Quality, 0);
        }
    }

        public class Legendary : Item
        {
            public override void UpdateQuality() { }


            public override void UpdateSellIn() { }
        }

        public class Cheese : Item
        {
            public override void UpdateQuality()
            {
                // If SellIn is negative, quality increase twice as fast (for cheese).
                if (SellIn >= 0) IncreaseQuality(1);
                else IncreaseQuality(2);
            }
        }

        public class Common : Item
        {
            public override void UpdateQuality()
            {
                if (SellIn > 0) DecreaseQuality(1);
                else DecreaseQuality(2);
            }
        }

        public class ConcertTicket : Item
        {
            public override void UpdateQuality()
            {
                if (SellIn >= 0)
                {
                    IncreaseQuality(1);
                     if (SellIn < 10) IncreaseQuality(1);
                     if (SellIn < 5) IncreaseQuality(1);
                     if (SellIn <= 0) Quality = 0;
                }
            }
        }

        public class Conjured : Common
        {
            public override void UpdateQuality()
            {
                base.UpdateQuality();
                base.UpdateQuality();
            }
        }
    }