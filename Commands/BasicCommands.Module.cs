using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Threading.Tasks;
using System.Linq;
using JackTheStudent.Commands;

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

    [Command("interact")]
    [Description("Simple command to test interaction!")]
    public async Task Interact(CommandContext ctx)
    {
    /* Trigger the Typing... in discord */
        await ctx.TriggerTypingAsync();

    /* Send the message "I'm Alive!" to the channel the message was recieved from */
        await ctx.RespondAsync("How are you today?");

        var intr = ctx.Client.GetInteractivityModule(); // Grab the interactivity module
        var response = await intr.WaitForMessageAsync(
            c => c.Author.Id == ctx.Message.Author.Id, // Make sure the response is from the same person who sent the command
            TimeSpan.FromSeconds(5) // Wait 60 seconds for a response instead of the default 30 we set earlier!
        );
    // You can also check for a specific message by doing something like
        
    // Null if the user didn't respond before the timeout
        if(response == null) {
            await ctx.RespondAsync("Sorry, I didn't get a response!");
            return;
        } else if (response.Message.Content.ToLower() == "bad") {
            await ctx.RespondAsync("Loser!");
        } else if (response.Message.Content.ToLower() == "good") {
            await ctx.RespondAsync("I'm glad!");
        } 
        
        await ctx.RespondAsync("Thank you for telling me how you are!");
    }
    
}
}