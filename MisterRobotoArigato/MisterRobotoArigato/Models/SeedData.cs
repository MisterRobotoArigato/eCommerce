using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MisterRobotoArigato.Data;
using System;
using System.Linq;

namespace MisterRobotoArigato.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RobotoDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<RobotoDbContext>>()))
            {
                if (context.Products.Any()) return;

                context.Products.AddRange(
                new Product
                {
                    Name = "BB8",
                    SKU = "11111",
                    Price = .25M,
                    Description = "Gumbo beet greens corn soko endive gumbo gourd.",
                    ImgUrl = "/Assets/bb8.png",
                },

                new Product
                {
                    Name = "Bending Unit",
                    SKU = "22222",
                    Price = .50M,
                    Description = "Parsley shallot courgette tatsoi pea sprouts fava bean collard greens dandelion okra wakame tomato.",
                    ImgUrl = "/Assets/bendingunit.jpg",
                },

                new Product
                {
                    Name = "Gundam",
                    SKU = "33333",
                    Price = 4.20M,
                    Description = "Bunya nuts black-eyed pea prairie turnip leek lentil turnip greens parsnip.",
                    ImgUrl = "/Assets/gundam.jpg",
                },

                new Product
                {
                    Name = "Android",
                    SKU = "44444",
                    Price = 2.50M,
                    Description = "Dandelion cucumber earthnut pea peanut soko zucchini.",
                    ImgUrl = "/Assets/humanandroid.jpg",
                },

                new Product
                {
                    Name = "Jukebox",
                    SKU = "55555",
                    Price = .75M,
                    Description = "Dandelion cucumber earthnut pea peanut soko zucchini.",
                    ImgUrl = "/Assets/jukebox.jpg",
                },

                new Product
                {
                    Name = "Killer Robot",
                    SKU = "66666",
                    Price = 3.25M,
                    Description = "Nori grape silver beet broccoli kombu beet greens fava bean potato quandong celery.",
                    ImgUrl = "/Assets/killerrobot.jpg",
                },

                new Product
                {
                    Name = "Mars Rover",
                    SKU = "77777",
                    Price = 5.00M,
                    Description = "Celery quandong swiss chard chicory earthnut pea potato.",
                    ImgUrl = "/Assets/marsrover.jpg",
                },

                new Product
                {
                    Name = "Iron Giant",
                    SKU = "88888",
                    Price = 2.00M,
                    Description = "Nori grape silver beet broccoli kombu beet greens fava bean potato quandong celery. ",
                    ImgUrl = "/Assets/miyazakirobot.jpg",
                },

                new Product
                {
                    Name = "Ramen Vending Robot",
                    SKU = "99999",
                    Price = 1.00M,
                    Description = "Beetroot water spinach okra water chestnut ricebean pea catsear courgette summer purslane.",
                    ImgUrl = "/Assets/ramenvending.jpg",
                },

                new Product
                {
                    Name = "Mechanical Bee",
                    SKU = "1010101010",
                    Price = .10M,
                    Description = "Grape wattle seed kombu beetroot horseradish carrot squash brussels sprout chard.",
                    ImgUrl = "/Assets/robotbee.jpg",
                });

                context.SaveChanges();
            }
        }
    }
}