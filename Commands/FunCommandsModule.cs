using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharp​Plus;
using System;
using System.Threading.Tasks;
using System.Linq;
using DSharpPlus.Interactivity.Extensions;
using HtmlAgilityPack;
using JackTheStudent.CommandDescriptions; 
using System.Collections.Generic;

namespace JackTheStudent.Commands
{
public class FunCommandsModule : Base​Command​Module
{
    [Command("roll")]
    [Description(FunDescriptions.rollDescription)]
    public async Task Roll(CommandContext ctx)
    {
        Random r = new Random();
        int rnd = r.Next(1,100);
        await ctx.RespondAsync("Your lucky number is: " + rnd);
    }


    [Command("chances")]
    [Description(FunDescriptions.chancesDescription)]    
    public async Task Chances(CommandContext ctx)
    {
        await ctx.TriggerTypingAsync();
        await ctx.RespondAsync("On a scale from 1 to 10, how do you rate your skills?");

        var intr = ctx.Client.GetInteractivity();
        var response = await intr.WaitForMessageAsync(
            c => c.Author.Id == ctx.Message.Author.Id, 
            TimeSpan.FromSeconds(15));

        if(response.Result.Content == null) {
            await ctx.RespondAsync("If you not gonna answer, dont bother me dude...");
            
        } else if (response.Result.Content == "1" || response.Result.Content == "2" || response.Result.Content == "3") {
            await ctx.RespondAsync("That's kinda bad... What's your lucky number?");
            var response1 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id,
                TimeSpan.FromSeconds(15));
            int luck = 0; Int32.TryParse(response1.Result.Content, out luck);

            if(response1.Result == null) {
            await ctx.RespondAsync("You might want to use !roll command to get your lucky number.");
            } else if (luck > 0 && luck <= 25) {
                await ctx.RespondAsync("Low skills and low luck, I'm not sure about this dude... That's the worst you could get.");
            } else if (luck > 25 && luck <= 50) {
                await ctx.RespondAsync("Low skills + under average luck = some trouble.");
            } else if (luck > 50 && luck <= 75) {
                await ctx.RespondAsync("Low skills and above average luck, but do you really want to trust on that?");
            } else if (luck > 75 && luck <= 100) {
                await ctx.RespondAsync("Low skills and high luck, let the gamble begin!");
            } else if (luck <= 0 || luck > 100) {
                await ctx.RespondAsync("Too high, too low luck or you didn't type a number. Yikes. Please use !chances again and type luck from 1 to 100.");
                return;
            }
             
        } else if (response.Result.Content == "4" || response.Result.Content == "5" || response.Result.Content == "6") {
            await ctx.RespondAsync("Not bad. What's your lucky number?");
            var response1 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id,
                TimeSpan.FromSeconds(15));
            int luck = 0; Int32.TryParse(response1.Result.Content, out luck);
            
            if(response1.Result == null) {
            await ctx.RespondAsync("You might want to use !roll command to get your lucky number.");
            } else if (luck > 0 && luck <= 25) {
                await ctx.RespondAsync("Average skills and low luck, this will be tough.");
            } else if (luck > 25 && luck <= 50) {
                await ctx.RespondAsync("Average skills and under average luck, won't be easy.");
            } else if (luck > 50 && luck <= 75) {
                await ctx.RespondAsync("Average skills and above average luck, kinda boring.");
            } else if (luck > 75 && luck <= 100) {
                await ctx.RespondAsync("Average skills and high luck, has some potential to be good.");
            } else if (luck <= 0 || luck > 100) {
                await ctx.RespondAsync("Too high, too low luck or you didn't type a number. Yikes. Please use !chances again and type luck from 1 to 100.");
                return;
            }
             
        } else if (response.Result.Content == "7" || response.Result.Content == "8" || response.Result.Content == "9") {
            await ctx.RespondAsync("Looks promising! What's your lucky number?");
            var response1 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id,
                TimeSpan.FromSeconds(15));
            int luck = 0; Int32.TryParse(response1.Result.Content, out luck);

            if(response1.Result == null) {
            await ctx.RespondAsync("You might want to use !roll command to get your lucky number.");
            } else if (luck > 0 && luck <= 25) {
                await ctx.RespondAsync("Above average skills and low luck means you need to be careful.");
            } else if (luck > 25 && luck <= 50) {
                await ctx.RespondAsync("Above average skills and under average luck. Don't worry, just focus.");
            } else if (luck > 50 && luck <= 75) {
                await ctx.RespondAsync("Above average skills and above average luck. You got this for sure.");
            } else if (luck > 75 && luck <= 100) {
                await ctx.RespondAsync("Above average skills and high luck. Nothing can go wrong.");
            } else if (luck <= 0 || luck > 100) {
                await ctx.RespondAsync("Too high, too low luck or you didn't type a number. Yikes. Please use !chances again and type luck from 1 to 100.");
                return;
            }
            
        } else if (response.Result.Content == "10") {
            await ctx.RespondAsync("Wow, do you really need this? What's your lucky number?");
            var response1 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id,
                TimeSpan.FromSeconds(15));
            int luck = 0; Int32.TryParse(response1.Result.Content, out luck);

            if(response1.Result == null) {
            await ctx.RespondAsync("You might want to use !roll command to get your lucky number.");
            } else if (luck > 0 && luck <= 25) {
                await ctx.RespondAsync("You know everything and have low luck. Kinda like an uphill bike climb.");
            } else if (luck > 25 && luck <= 50) {
                await ctx.RespondAsync("You know everything and have under average luck, I guess you still got it.");
            } else if (luck > 50 && luck <= 75) {
                await ctx.RespondAsync("You know everything and have above average luck. Just go and write it already!");
            } else if (luck > 75 && luck <= 100) {
                await ctx.RespondAsync("You know everything and have super high luck. Daaaaamn. This really can't go wrong.");
            }  else if (luck <= 0 || luck > 100) {
                await ctx.RespondAsync("Too high, too low luck or you didn't type a number. Yikes. Please use !chances again and type luck from 1 to 100.");
                return;
            }

        } else if (response.Result.Content == "0") {
            await ctx.RespondAsync("You can't know nothing, you always know something.");
            return;

        } else if (!(response.Result.Content == "1" || response.Result.Content == "2" || response.Result.Content == "3" || response.Result.Content == "4"
        || response.Result.Content == "5" || response.Result.Content == "6" || response.Result.Content == "7" || response.Result.Content == "8" 
        || response.Result.Content == "9" || response.Result.Content == "10")) {
            await ctx.RespondAsync("Please use !chances again and type a number from 1 to 10.");
            return;
        }

        await ctx.RespondAsync("Despite my answer, I hope you pass it anyway. Good luck!");

        }

    
    [Command("inspire")]
    [Description(FunDescriptions.inspireDescription)]
    public async Task Inspire(CommandContext ctx)    
    {
        var random = new Random();
        int index = random.Next(JackTheStudent.Program.quotes.Count);
        await ctx.RespondAsync(JackTheStudent.Program.quotes[index]);
    }


    [Command("poll")]
    [Description(FunDescriptions.pollDescription)]
    public async Task Poll(CommandContext ctx, string dur = "" , string input = "")
    {
        var intr = ctx.Client.GetInteractivity();
     
        try {
        TimeSpan durationX = TimeSpan.Parse(dur);
            }
        catch (FormatException) {
        await ctx.RespondAsync(dur + " is not a valid format. Use hh:mm:ss format instead. Sorry for that.");
        return;
            }   
        catch (OverflowException) {
        await ctx.RespondAsync("Overflow duuude. Seconds max is 60, same for minutes.");
        return;
            }

        TimeSpan duration = TimeSpan.Parse(dur);

        if(duration.Days >= 1){
            await ctx.RespondAsync("Max poll duration is 23:59:59. Use hh:mm:ss format.");
            return;
        }

        if(input == null) {
            await ctx.RespondAsync("Fuck your poll dude, I'm out.");
            return;
        }

        string[] emojiList = input.Split(new Char [] {','});
        
        var pollEmojiOptions = emojiList.Select(e => e.ToString());

        var pollEmbed = new DiscordEmbedBuilder
        {
            Title = "The poll is about: " + input,
            Description = "React with :thumbsup: or :thumbsdown: to show what you think!\n\nYou have " + duration
        };

        var pollMsg = await ctx.RespondAsync(embed: pollEmbed);

        List<string> list = new List<string>();

        List<string> watermelon = new List<string>();
        watermelon.Add(":thumbsup:");
        watermelon.Add(":thumbsdown:");

         for (int i = 0; i < 2; i++)
        {
            var random = new Random();
            int index = random.Next(watermelon.Count());
            await pollMsg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, watermelon[index]));
            watermelon.RemoveAt(index);
        }

        //Result below, not emoji creation involed!!!

        var result = await intr.CollectReactionsAsync(pollMsg, duration).ConfigureAwait(false);
        var disctinctResult = result.Distinct();
        var results = disctinctResult.Select(x => $"{x.Emoji}: {x.Total}");

        var resultsEmbed = new DiscordEmbedBuilder
        {
            Title = "Results:",
            Description = "Votes for: "+string.Join("\nVotes for: ", results)
        };

        var resultsMsg = await ctx.RespondAsync(embed: resultsEmbed);
    }   


    [Command("weather")]
    [Description(FunDescriptions.weatherDescription)]
    public async Task Weather(CommandContext ctx, string city = "")
        {
            var url = "https://pogoda.wprost.pl/prognoza-pogody/" + city;

            if (!(JackTheStudent.Program.weatherCities.Contains(city))){
                await ctx.RespondAsync("This city is not supported, sorry!");
                return;
            }

            var tableTempTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[1]";
            var tablePressureTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[2]";
            var tableWindTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[3]";
            var tableHumidityTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[4]";
            var tableCloudinessTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[5]";

            var tempTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[1]/span";
            var pressureTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[2]/span";
            var WindTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[3]/span";
            var humidityTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[4]/span";
            var cloudinessTodayXpath = "/html/body/div[2]/div/section[1]/section/div/div[1]/dl/dd[5]/span";

            var htmlDoc = new HtmlWeb().Load(url);

            var tableTempToday = htmlDoc.DocumentNode.SelectNodes(tableTempTodayXpath).First();
            var temp = tableTempToday.SelectNodes(tempTodayXpath).Select(n => n.GetDirectInnerText().Trim()).SingleOrDefault();

            var tablePressureToday = htmlDoc.DocumentNode.SelectNodes(tablePressureTodayXpath).First();
            var pressure = tablePressureToday.SelectNodes(pressureTodayXpath).Select(n => n.GetDirectInnerText().Trim()).SingleOrDefault();

            var tableWindToday = htmlDoc.DocumentNode.SelectNodes(tableWindTodayXpath).First();
            var wind = tableWindToday.SelectNodes(WindTodayXpath).Select(n => n.GetDirectInnerText().Trim()).SingleOrDefault();

            var tableHumidityToday = htmlDoc.DocumentNode.SelectNodes(tableHumidityTodayXpath).First();
            var humidity = tableHumidityToday.SelectNodes(humidityTodayXpath).Select(n => n.GetDirectInnerText().Trim()).SingleOrDefault();

            var tableCloudinessToday = htmlDoc.DocumentNode.SelectNodes(tableCloudinessTodayXpath).First();
            var cloudiness = tableCloudinessToday.SelectNodes(cloudinessTodayXpath).Select(n => n.GetDirectInnerText().Trim()).SingleOrDefault();
            
            var weatherEmbed = new DiscordEmbedBuilder
            {
                Title = ":sun_with_face: Weather for today :cloud_lightning:",
                Description = "Temperature: "+temp+"°C"+"\nPressure: "+pressure+" hPa"+"\nWind: "+wind+" m/s"+"\nHumidity: "+humidity+"%"+"\nCloudiness: "+cloudiness+"%"+"\n\nHave a nice day!"
            };
            
            await ctx.RespondAsync(embed: weatherEmbed);
        }
    }
}