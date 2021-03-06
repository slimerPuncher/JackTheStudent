using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Threading.Tasks;
using JackTheStudent.Models;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

/* Create our class and extend from IModule */
namespace JackTheStudent.Commands
{
public class StudentCommandsModule : Base​Command​Module
{
    [Command("group")]
    [Description("Command retrieving group IDs")]
    public async Task GroupLogs(CommandContext ctx)
    {
        try {
            using (var db = new JackTheStudentContext()){
                var groups = db.Group.ToList();
                    if (groups.Count == 0) {
                            await ctx.RespondAsync("No groups logged!");
                    } else {
                        string result = String.Empty;
                        foreach (Group group in groups) {
                            result = result + "\n" + group.GroupId;
                        }
                        await ctx.RespondAsync(result);
                    }
            }
        } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
        }
        return; 
    }

    [Command("class")]
    [Description("Command retrieving classes")]
    public async Task ClassesLogs(CommandContext ctx)
    {
        try {
            using (var db = new JackTheStudentContext()){
                var classes = db.Class.ToList();
                    if (classes.Count == 0) {
                            await ctx.RespondAsync("No classes logged!");
                    } else {
                        string result = String.Empty;
                        foreach (Class uniClass in classes) {
                             result = result + "\n" + "Class - " + uniClass.Name + ", short version - " + uniClass.ShortName;
                        }
                        await ctx.RespondAsync(result);
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
}