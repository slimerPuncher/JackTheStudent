using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Threading.Tasks;
using System.Linq;
using JackTheStudent.Commands;
using JackTheStudent.Models;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace JackTheStudent.Commands
{
/* Create our class and extend from IModule */
public class BasicCommandsModule : IModule
{
    /* Commands in DSharpPlus.CommandsNext are identified by supplying a Command attribute to a method in any class you've loaded into it. */
    /* The description is just a string supplied when you use the help command included in CommandsNext. */
    [Command("alive")]
    [Description("Simple command to test if the bot is running!")]
    public async Task Alive(CommandContext ctx)
    {
        /* Trigger the Typing... in discord */
        await ctx.TriggerTypingAsync();

        /* Send the message "I'm Alive!" to the channel the message was recieved from */
        await ctx.RespondAsync("I'm alive!");
    }

    [Command("dead")]
    [Description("Simple command to test if the bot is dead!")]
    public async Task Dead(CommandContext ctx)
    {
        /* Trigger the Typing... in discord */
        await ctx.TriggerTypingAsync();

        /* Send the message "I'm Alive!" to the channel the message was recieved from */
        await ctx.RespondAsync("I'm dead!");
    }

    [Command("truncate")]
    [Description("Simple command to test if the bot is dead!")]
    public async Task Truncate(CommandContext ctx)
    {
        
        using (var db = new JackTheStudentContext()){
            
        }                    

        /* Send the message "I'm Alive!" to the channel the message was recieved from */
        await ctx.RespondAsync("All tables have been cleared!");
    }

    [Command("logdickappointment")]
    [Description("Simple command to test if the bot is dead!")]
    public async Task DickAppointment(CommandContext ctx, string name, string length, string circumference, string width, DateTime date)
    {

        try {
            using (var db = new JackTheStudentContext()) {
                var dickAppointment = new DickAppointment {
                    Name = name,
                    Date = date,
                    Width = width,
                    Circumference = circumference,
                    Length = length
                };
                db.DickAppointment.Add(dickAppointment);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex) {
            Console.Error.WriteLine("[Jack] " + ex.ToString());
            await ctx.RespondAsync("Dick appointment log failed");
            return;
        }

        await ctx.RespondAsync("Dick appointment logged successfully");
        return;
    }

    [Command("logsdickappointment")]
    [Description("Simple command to test if the bot is dead!")]
    public async Task DickAppointmentLogs(CommandContext ctx, string span = "planned")
    {
        if (span == "planned") {
            try {
            using (var db = new JackTheStudentContext()){
                var dickAppointments = db.DickAppointment
                    .Where(c => c.Date > DateTime.Now)
                    .ToList();
                    foreach (DickAppointment dickAppointment in dickAppointments) {
                        await ctx.RespondAsync(dickAppointment.Name + " is equipped with a " + dickAppointment.Length + " cm length dingus, " + "that is " +
                         dickAppointment.Circumference + " in circumference (" + dickAppointment.Width + " width). Meeting scheduled for " + dickAppointment.Date + ".");
                    }
                    }
                } catch(Exception ex) {
                    Console.Error.WriteLine("[Jack] " + ex.ToString());
                    await ctx.RespondAsync("Show logs failed");
                    return;
                }
            return;
        } else if (span == ".") {
            try {
            using (var db = new JackTheStudentContext()){
                var dickAppointments = db.DickAppointment
                    .ToList();
                    foreach (DickAppointment dickAppointment in dickAppointments) {
                        await ctx.RespondAsync(dickAppointment.Name + " is equipped with a " + dickAppointment.Length + " cm length dingus, " + "that is " +
                         dickAppointment.Circumference + " in circumference (" + dickAppointment.Width + " width). Meeting scheduled for " + dickAppointment.Date + ".");
                    }
                    }
                } catch(Exception ex) {
                    Console.Error.WriteLine("[Jack] " + ex.ToString());
                    await ctx.RespondAsync("Show logs failed");
                    return;
                }
            return;
        }
        
    }


    [Command("interact")]
    [Description("Simple command to test interaction!")]
    public async Task Interact(CommandContext ctx)
    {
    /* Trigger the Typing... in discord */
        await ctx.TriggerTypingAsync();

    /* Send the message "I'm Alive!" to the channel the message was recieved from */
        await ctx.RespondAsync("How I am today?");

        var intr = ctx.Client.GetInteractivityModule(); // Grab the interactivity module
        var response = await intr.WaitForMessageAsync(
            c => c.Author.Id == ctx.Message.Author.Id, // Make sure the response is from the same person who sent the command
            TimeSpan.FromSeconds(15) // Wait 60 seconds for a response instead of the default 30 we set earlier!
        );
    // You can also check for a specific message by doing something like
        
    // Null if the user didn't respond before the timeout
        if(response == null) {
            await ctx.RespondAsync("Sorry, I didn't get a response!");
            return;
        } else if (response.Message.Content.ToLower() == "bad") {
            await ctx.RespondAsync("Okay, how are you then?");

            var response1 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id, // Make sure the response is from the same person who sent the command
                TimeSpan.FromSeconds(15)
            );
            if(response1 == null) {
            await ctx.RespondAsync("Sorry, I didn't get a response!");
            return;
            } else if (response1.Message.Content.ToLower() == "bad") {
                await ctx.RespondAsync("Loser!");
            } else if (response1.Message.Content.ToLower() == "good") {
                await ctx.RespondAsync("I'm glad!");
            } // Wait 60 seconds for a response instead of the default 30 we set earlier!
        } else if (response.Message.Content.ToLower() == "good") {
            await ctx.RespondAsync("Okay, how are you then?");
            var response2 = await intr.WaitForMessageAsync(
                c => c.Author.Id == ctx.Message.Author.Id, // Make sure the response is from the same person who sent the command
                TimeSpan.FromSeconds(15)
            );
            if(response2 == null) {
            await ctx.RespondAsync("Sorry, I didn't get a response!");
            return;
            } else if (response2.Message.Content.ToLower() == "bad") {
                await ctx.RespondAsync("Aww, that's too bad!");
            } else if (response2.Message.Content.ToLower() == "good") {
                await ctx.RespondAsync("I'm glad!");
            }
        } 
        
        await ctx.RespondAsync("Thank you for telling me how you are!");
    }
    
}
}