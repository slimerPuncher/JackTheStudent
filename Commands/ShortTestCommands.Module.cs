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
public class ShortTestCommandsModule : Base​Command​Module
{
    
    [Command("shorttest")]
    [Description("Command logging a short test, last two arguments are optional." +
        "\nTo pass without addInfo but with materials use \".\" where addInfo should be.\n" +
        "Words seperated with spaces must be wrapped with \"\"\n" +
        "\n!shorttest <groupId> <classShortName> <shortTestDate> <shortTestTime> <additionalInfo> <materials>\n" + 
        "\nExamples:\n" +
        "\n!shorttest 3 mat 05-05-2021 13:30" + 
        "\n!shorttest 1 ele 05-05-2021 12:30 \"Calculator required\"" +
        "\n!shorttest 3 mat 05-05-2021 13:30 \"Calculator required\"")]
    public async Task ShortTestLog(CommandContext ctx,
        [Description ("\nTakes group IDs, type !group to retrieve all groups.\n")] string groupId = "", 
        [Description ("\nTakes class' short names, type !class to retrive all classes.\n")] string classType = "", 
        [Description ("\nTakes dates in dd/mm/yyyy format, accepts different separators.\n")] string eventDate = "", 
        [Description ("\nTakes time in hh:mm format.\n")] string eventTime = "", 
        [Description ("\nTakes additional information, multiple words must be wrapped with \"\".\n")] string additionalInfo = "")
    {
        DateTime parsedEventDate = new DateTime();
        DateTime parsedEventTime = new DateTime();

        if(groupId == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !shorttest <group> <class> <shortTestDate> <shortTestTime> Try again!");
            return;
        } else if (!JackTheStudent.Program.groupList.Contains(groupId)){
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (classType == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !shorttest <group> <class> <shortTestDate> <shortTestTime> Try again!");
            return;      
        } else if (!JackTheStudent.Program.classList.Any(c => c.ShortName == classType)) {
            await ctx.RespondAsync("There's no such class, you high bruh?");
            return;
        } else if (eventDate == ""){
            await ctx.RespondAsync("There's date missing, fix it!");
            return;
        } else if (!DateTime.TryParse(eventDate, out parsedEventDate)) {
            await ctx.RespondAsync("That's not a valid date you retard, learn to type!");
            return;
        } else if (eventTime == ""){
            await ctx.RespondAsync("There's time missing, fix it!");
            return;
        } else if (!DateTime.TryParse(eventTime, out parsedEventTime)) {
            await ctx.RespondAsync("That's not a valid time you retard, learn to type!");
            return;
        } else {
            try {
                using (var db = new JackTheStudentContext()){
                var shortTest = new ShortTest {
                    ClassShortName = classType,
                    Class = JackTheStudent.Program.classList.Where(c => c.ShortName == classType).Select(c => c.Name).FirstOrDefault(),
                    Date = parsedEventDate.Date.Add(parsedEventTime.TimeOfDay),
                    GroupId = groupId,
                    LogById = ctx.Message.Author.Id.ToString(),
                    LogByUsername = ctx.Message.Author.Username + "#" + ctx.Message.Author.Discriminator,
                    AdditionalInfo = additionalInfo
                };
                JackTheStudent.Program.shortTestList.Add(shortTest);
                db.ShortTest.Add(shortTest);
                await db.SaveChangesAsync();
                }
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Log failed");
                return;
            }
        await ctx.RespondAsync("Short test logged successfully");     
        return;
        }   
    }

    [Command("shorttests")]
    [Description("Command retrieving logged short test based on passed arguments, ALL arguments are optional and the command has default settings.\n" +
        "\n!shorttests <groupId> <classShortName> <alreadyTookPlace?>\n" + 
        "\nType !classes to retrieve short names and !groups to retrieve group IDs" +
        "\nUse \".\" to retrieve ALL possible entries for each argument, <alreadyTookPlace?> takes \"planned\" or \".\"\n" +
        "\nExamples:\n" +
        "\n!shorttests - will retrieve all PLANNED short tests for all the groups and all the classes" + 
        "\n!shorttests 1 - will retrieve all PLANNED short tests for group 1 for all the classes" +
        "\n!shorttests 1 mat - will retrieve all PLANNED short tests for group 1 for Maths class" +
        "\n!shorttests 1 mat planned - will retrieve all PLANNED short tests for group 1 for Maths class" +
        "\n!shorttests 1 mat . - will retrieve all LOGGED short tests for group 1 for Maths class" +
        "\n!shorttests 1 . . - will retrieve all LOGGED short tests for group 1 for ALL classes" + 
        "\n!shorttests . . . - will retrieve all LOGGED short tests for ALL groups for ALL classes" +
        "\n!shorttests . mat . - will retrieve all LOGGED short tests for ALL groups for MAths class" +
        "\n!shorttests . . planned - will retrieve all PLANNED short tests for ALL groups for ALL classes")]
    public async Task ShortTestLogs(CommandContext ctx, 
        [Description("\nTakes group IDs or \".\", type !group to retrieve all groups, usage of \".\" will tell Jack to retrieve short test for ALL groups.\n")] string group = ".",
        [Description("\nTakes class' short names or \".\", type !class to retrieve all classes, usage of \".\" will tell Jack to retrieve short test for ALL classes.\n")] string classType = ".",
        [Description("\nTakes \".\" or \"planned\", usage of \".\" will tell Jack to retrieve all LOGGED short test, \"planned\" retrieves only future events.\n")] string span = "planned")
    {       
        if (!JackTheStudent.Program.groupList.Contains(group) && group != ".") {
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (!JackTheStudent.Program.classList.Any(c => c.ShortName == classType) && classType != ".") {
            await ctx.RespondAsync("There's no such class, you high bruh?");
            return;
        } else if (span != "." && span != "planned") {
            await ctx.RespondAsync("Span only accepts . and planned values");
            return;
        }

        var shortTests = JackTheStudent.Program.shortTestList;
        string result = String.Empty;

        if (group == "." && classType == "." && span == "planned") {
            try {         
                shortTests = shortTests.Where(s => s.Date > DateTime.Now).ToList();
                if (shortTests.Count == 0) {
                        await ctx.RespondAsync("Wait what!? There are literally no short tests planned at all!");
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                }
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if(classType == "." && span == "." && group != "." ) {
            try {
                shortTests = shortTests.Where(s => s.GroupId == group).ToList();
                if (shortTests.Count == 0) {
                        await ctx.RespondAsync($"There are no short tests logged for group {group}!");
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen/happened on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                }
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if (classType == "." && span == "planned" && group != ".") {
            try {
                shortTests = shortTests.Where(s => s.Date > DateTime.Now && s.GroupId == group).ToList();
                if (shortTests.Count == 0) {
                        await ctx.RespondAsync($"Wait what!? There are no short tests planned for any class for group {group}!");
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                }
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if (classType != "." && span == "planned" && group != ".") {
            try {
                shortTests = shortTests.Where(s => s.Date > DateTime.Now && s.Class == classType && s.GroupId == group).ToList();                     
                if (shortTests.Count == 0) {
                    await ctx.RespondAsync($"There are no {shortTests.Select(s => s.Class).FirstOrDefault()} short tests planned for group {group}!");
                    return;
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                           
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }                   
        } else if (classType != "." && span == "." && group != ".") {
            try {
                shortTests = shortTests.Where(s => s.Class == classType && s.GroupId == group).ToList();                     
                if (shortTests.Count == 0) {
                    await ctx.RespondAsync($"There are no short tests logged for {shortTests.Select( c => c.Class).FirstOrDefault()} class for group {group}!");
                    return;
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen/happened on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                           
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }                 
        } else {
            try {
                shortTests = shortTests.ToList();                     
                if (shortTests.Count == 0) {
                    await ctx.RespondAsync("There are no short tests logged!");
                    return;
                } else {
                    foreach (ShortTest shortTest in shortTests) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(shortTest.Class)} short test for group {shortTest.GroupId}, will happen/happened on {shortTest.Date}.{(shortTest.AdditionalInfo.Equals("") ? "" : $"Additional info: {shortTest.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                          
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        }    
    }
}
}