using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using JackTheStudent.Models;
using System.Linq;
using System.Globalization;
using JackTheStudent.CommandDescriptions;
using Serilog;

namespace JackTheStudent.Commands
{
public class HomeworkCommandsModule : Base​Command​Module
{
    
    [Command("homework")]
    [Description(HomeworkDescriptions.homeworkLogDescription)]
    public async Task HomeworkLog(CommandContext ctx,
        [Description ("\nTakes group IDs, type !group to retrieve all groups.\n")] string groupId = "", 
        [Description ("\nTakes class' short names, type !class to retrive all classes.\n")] string classType = "", 
        [Description ("\nTakes dates in dd/mm/yyyy format, accepts different separators.\n")] string eventDate = "", 
        [Description ("\nTakes time in hh:mm format.\n")] string eventTime = "", 
        [Description ("\nTakes additional information, multiple words must be wrapped with \"\".\n")] string additionalInfo = "")
    {
        DateTime parsedEventDate = new DateTime();
        DateTime parsedEventTime = new DateTime();

        if(groupId == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !homework <group> <class> <deadlineDate> <deadlineTime> Try again!");
            return;
        } else if (!JackTheStudent.Program.groupList.Any(g => g.GroupId == groupId)){
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (classType == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !homework <group> <class> <deadlineDate> <deadlineTime> Try again!");
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
                var homeWork = new Homework {
                    ClassShortName = classType,
                    Class = JackTheStudent.Program.classList.Where(c => c.ShortName == classType).Select(c => c.Name).FirstOrDefault(),
                    Date = parsedEventDate.Date.Add(parsedEventTime.TimeOfDay),
                    GroupId = groupId,
                    LogById = ctx.Message.Author.Id.ToString(),
                    LogByUsername = ctx.Message.Author.Username + "#" + ctx.Message.Author.Discriminator,
                    AdditionalInfo = additionalInfo
                };
                JackTheStudent.Program.homeworkList.Add(homeWork);
                db.Homework.Add(homeWork);
                await db.SaveChangesAsync();
                Log.Logger.Information($"[Jack] {ctx.Message.Author.Id} logged new homework with ID: {homeWork.Id}");
                }
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] New homework log, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Log failed");
                return;
            }
        await ctx.RespondAsync("Homework logged successfully");     
        return;
        }   
    }

    [Command("homeworks")]
    [Description(HomeworkDescriptions.homeworkLogsDescription)]
    public async Task HomeworkLogs(CommandContext ctx, 
        [Description("\nTakes group IDs or \".\", type !group to retrieve all groups, usage of \".\" will tell Jack to retrieve homework for ALL groups.\n")] string group = ".",
        [Description("\nTakes class' short names or \".\", type !class to retrieve all classes, usage of \".\" will tell Jack to retrieve homework for ALL classes.\n")] string classType = ".",
        [Description("\nTakes \".\" or \"planned\", usage of \".\" will tell Jack to retrieve all LOGGED homework, \"planned\" retrieves only future events.\n")] string span = "planned")
    {       
        if (!JackTheStudent.Program.groupList.Any(g => g.GroupId == group) && group != ".") {
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (!JackTheStudent.Program.classList.Any(c => c.ShortName == classType) && classType != ".") {
            await ctx.RespondAsync("There's no such class, you high bruh?");
            return;
        } else if (span != "." && span != "planned") {
            await ctx.RespondAsync("Span only accepts . and planned values");
            return;
        }

        var homeworks = JackTheStudent.Program.homeworkList;
        string result = String.Empty;   

        if (group == "." && classType == "." && span == "planned") {
            try {
                homeworks = homeworks.Where(h => h.Date > DateTime.Now).ToList();
                    if (homeworks.Count == 0) {
                            await ctx.RespondAsync("Wait what!? There is literally no homework planned at all!");
                    } else {
                        foreach (Homework homework in homeworks) {
                            result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId} deadline is {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                        }
                        await ctx.RespondAsync(result);
                    }
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if(classType == "." && span == "." && group != "." ) {
            try {
                homeworks = homeworks.Where(h => h.GroupId == group).ToList();
                    if (homeworks.Count == 0) {
                            await ctx.RespondAsync($"Wait what!? There is no homework logged for group {group}!");
                    } else {
                        foreach (Homework homework in homeworks) {
                            result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId} deadline is/was {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                        }
                        await ctx.RespondAsync(result);
                    }
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if (classType == "." && span == "planned" && group != ".") {
            try {
                homeworks = homeworks.Where(h => h.Date > DateTime.Now && h.GroupId == group).ToList();
                    if (homeworks.Count == 0) {
                            await ctx.RespondAsync($"Wait what!? There is no planned homework for group {group}, hmm... league?");
                    } else {
                        foreach (Homework homework in homeworks) {
                            result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId} deadline is {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                        }
                        await ctx.RespondAsync(result);
                    }
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        return;
        } else if (classType != "." && span == "planned" && group !=".") {
            try {
                homeworks = homeworks.Where(h => h.Date > DateTime.Now && h.Class == classType && h.GroupId == group).ToList();                  
                if (homeworks.Count == 0) {
                    await ctx.RespondAsync($"There is no {homeworks.Select(h => h.Class).FirstOrDefault()} homework planned for group {group} at all!");
                    return;
                } else {
                    foreach (Homework homework in homeworks) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId}, deadline is {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                           
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }                
        } else if (classType != "." && span == "." && group !=".") {
            try {
                homeworks = homeworks.Where(h => h.Class == classType && h.GroupId == group).ToList();                     
                if (homeworks.Count == 0) {
                    await ctx.RespondAsync($"There is no homework logged for {homeworks.Select(h => h.Class).FirstOrDefault()} class for group {group}!");
                    return;
                } else {
                    foreach (Homework homework in homeworks) {
                    result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId}, deadline is/was {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                           
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }               
        } else {
            try {
                homeworks = homeworks.ToList();                     
                if (homeworks.Count == 0) {
                    await ctx.RespondAsync("There is no logged homework!");
                    return;
                } else {
                    foreach (Homework homework in homeworks) {
                        result = $"{result} \n{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(homework.Class)} homework for group {homework.GroupId}, deadline is/was {homework.Date}.{(homework.AdditionalInfo.Equals("") ? "" : $"Additional info: {homework.AdditionalInfo}")}";
                    }
                    await ctx.RespondAsync(result);
                    return;
                }                       
            } catch(Exception ex) {
                Log.Logger.Error($"[Jack] Homework logs, caller - {ctx.Message.Author.Id}, error: " + ex.ToString());
                await ctx.RespondAsync("Show logs failed");
                return;
            }
        }    
    }
}
}
