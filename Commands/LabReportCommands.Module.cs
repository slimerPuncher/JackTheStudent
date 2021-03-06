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
public class LabReportCommandsModule : Base​Command​Module
{
    
    [Command("labreport")]
    [Description("Command logging a lab report, last two arguments are optional." +
        "\nTo pass without addInfo but with materials use \".\" where addInfo should be.\n" +
        "Words seperated with spaces must be wrapped with \"\"\n" +
        "\n!labreport <groupId> <classShortName> <labReportDate> <labReportTime> <additionalInfo> <materials>\n" + 
        "\nExamples:\n" +
        "\n!labreport 3 mat 05-05-2021 13:30" + 
        "\n!labreport 1 ele 05-05-2021 12:30 \"Calculator required\"" +
        "\n!labreport 3 mat 05-05-2021 13:30 \"Calculator required\" \"https://yourmaterials.com\"" +
        "\n!labreport 1 eng 05-05-2021 13:30 . \"https://yourmaterials.com\"")]
    public async Task LabReportLog(CommandContext ctx,
        [Description ("\nTakes group IDs, type !group to retrieve all groups.\n")] string groupId = "", 
        [Description ("\nTakes class' short names, type !class to retrive all classes.\n")] string classType = "", 
        [Description ("\nTakes dates in dd/mm/yyyy format, accepts different separators.\n")] string eventDate = "", 
        [Description ("\nTakes time in hh:mm format.\n")] string eventTime = "", 
        [Description ("\nTakes additional information, multiple words must be wrapped with \"\".\n")] string additionalInfo = "", 
        [Description ("\nTakes material links, multiple links must be wrapped with \"\".\n")] string materials = "")
    {
        DateTime parsedEventDate = new DateTime();
        DateTime parsedEventTime = new DateTime();

        if(groupId == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !labreport <group> <class> <labReportDate> <labReportTime> Try again!");
            return;
        } else if (!JackTheStudent.Program.groupList.Contains(groupId)){
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (classType == "") {
            await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !labreport <group> <class> <labReportDate> <labReportTime> Try again!");
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
                var labReport = new LabReport {Class = classType,
                                                Date = parsedEventDate.Date.Add(parsedEventTime.TimeOfDay),
                                                GroupId = groupId,
                                                LogById = ctx.Message.Author.Id.ToString(),
                                                LogByUsername = ctx.Message.Author.Username + "#" + ctx.Message.Author.Discriminator,
                                                AdditionalInfo = additionalInfo,
                                                Materials = materials};
                JackTheStudent.Program.labReportList.Add(labReport);
                db.LabReport.Add(labReport);
                await db.SaveChangesAsync();
                }
            } catch(Exception ex) {
                Console.Error.WriteLine("[Jack] " + ex.ToString());
                await ctx.RespondAsync("Log failed");
                return;
            }
        await ctx.RespondAsync("Lab report logged successfully");     
        return;
        }   
    }

    [Command("labreports")]
    [Description("Command retrieving logged lab report based on passed arguments, ALL arguments are optional and the command has default settings.\n" +
        "\n!labreports <groupId> <classShortName> <alreadyTookPlace?>\n" + 
        "\nType !classes to retrieve short names and !groups to retrieve group IDs" +
        "\nUse \".\" to retrieve ALL possible entries for each argument, <alreadyTookPlace?> takes \"planned\" or \".\"\n" +
        "\nExamples:\n" +
        "\n!labreports - will retrieve all PLANNED lab reports for all the groups and all the classes" + 
        "\n!labreports 1 - will retrieve all PLANNED lab reports for group 1 for all the classes" +
        "\n!labreports 1 mat - will retrieve all PLANNED lab reports for group 1 for Maths class" +
        "\n!labreports 1 mat planned - will retrieve all PLANNED lab reports for group 1 for Maths class" +
        "\n!labreports 1 mat . - will retrieve all LOGGED lab reports for group 1 for Maths class" +
        "\n!labreports 1 . . - will retrieve all LOGGED lab reports for group 1 for ALL classes" + 
        "\n!labreports . . . - will retrieve all LOGGED lab reports for ALL groups for ALL classes" +
        "\n!labreports . mat . - will retrieve all LOGGED lab reports for ALL groups for MAths class" +
        "\n!labreports . . planned - will retrieve all PLANNED lab reports for ALL groups for ALL classes")]
    public async Task LabReportLogs(CommandContext ctx, 
        [Description("\nTakes group IDs or \".\", type !group to retrieve all groups, usage of \".\" will tell Jack to retrieve lab report for ALL groups.\n")] string group = ".",
        [Description("\nTakes class' short names or \".\", type !class to retrieve all classes, usage of \".\" will tell Jack to retrieve lab report for ALL classes.\n")] string classType = ".",
        [Description("\nTakes \".\" or \"planned\", usage of \".\" will tell Jack to retrieve all LOGGED lab report, \"planned\" retrieves only future events.\n")] string span = "planned")
    {       
        if (!JackTheStudent.Program.groupList.Contains(group) && group != ".") {
            await ctx.RespondAsync("There's no such group dumbass. Try again!");
            return;
        } else if (!JackTheStudent.Program.classList.Any(c => c.ShortName == classType) && classType != ".") {
            await ctx.RespondAsync("There's no such class, you high bruh?");
            return;
        }
        if (group == "." && classType == "." && span == "planned") {
            try {
                using (var db = new JackTheStudentContext()){
                var labReports = db.LabReport
                            .Where( x => x.Date > DateTime.Now)
                            .ToList();
                    if (labReports.Count == 0) {
                            await ctx.RespondAsync("Wait what!? There are literally no lab reports planned at all!");
                    } else {
                        string result = String.Empty;
                        foreach (LabReport labReport in labReports) {
                            result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(JackTheStudent.Program.classList
                                                        .Where( c => c.ShortName == labReport.Class)
                                                        .Select( c => c.Name)
                                                        .FirstOrDefault()) + " lab report for group " + labReport.GroupId + ", deadline is " + labReport.Date;
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
        } else if(classType == "." && span == "." && group != "." ) {
            try {
                using (var db = new JackTheStudentContext()){
                var labReports = db.LabReport
                    .Where( x => x.GroupId == group)
                    .ToList();
                    if (labReports.Count == 0) {
                            await ctx.RespondAsync("There are no lab reports logged for group " + group + "!");
                    } else {
                        string result = String.Empty;
                        foreach (LabReport labReport in labReports) {
                            result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(JackTheStudent.Program.classList
                                                        .Where( c => c.ShortName == labReport.Class)
                                                        .Select( c => c.Name)
                                                        .FirstOrDefault()) + " " + labReport.Date;
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
        } else if (classType == "." && span == "planned") {
            try {
                using (var db = new JackTheStudentContext()){
                var labReports = db.LabReport
                    .Where(x => x.Date > DateTime.Now && x.GroupId == group)
                    .ToList();
                    if (labReports.Count == 0) {
                            await ctx.RespondAsync("Wait what!? There are no lab reports planned for any class for group " + group + "!");
                    } else {
                        string result = String.Empty;
                        foreach (LabReport labReport in labReports) {
                            result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(JackTheStudent.Program.classList
                                                        .Where( c => c.ShortName == labReport.Class)
                                                        .Select( c => c.Name)
                                                        .FirstOrDefault()) + " lab report for group " + labReport.GroupId + ", deadline is " + labReport.Date;
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
        } else if (classType != "." && span == "planned" && group != ".") {

            if(JackTheStudent.Program.classList.Any(c => c.ShortName == classType)) {
                try {
                    using (var db = new JackTheStudentContext()){
                        var labReports = db.LabReport
                            .Where(x => x.Date > DateTime.Now && x.Class == classType && x.GroupId == group)
                            .ToList();                     

                        if (labReports.Count == 0) {
                            string response = "There is no " + JackTheStudent.Program.classList
                                                                .Where( c => c.ShortName == classType)
                                                                .Select( c => c.Name)
                                                                .FirstOrDefault() + " lab report planned for group " + group + "!";
                            await ctx.RespondAsync(response);
                            return;
                        } else {
                            string result = String.Empty;
                            foreach (LabReport labReport in labReports) {
                                result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                            .ToTitleCase(JackTheStudent.Program.classList
                                                            .Where( c => c.ShortName == labReport.Class)
                                                            .Select( c => c.Name)
                                                            .FirstOrDefault()) + " labReport for group " + labReport.GroupId + ", deadline is " + labReport.Date;
                            }
                            await ctx.RespondAsync(result);
                            return;
                        }                           
                    }
                } catch(Exception ex) {
                    Console.Error.WriteLine("[Jack] " + ex.ToString());
                    await ctx.RespondAsync("Show logs failed");
                    return;
                }
            } else {
                await ctx.RespondAsync("Learn to read you dumbass. The command looks like: !labReports <group> <group> <labReportDate> <labReportTime> Try again!");
                return;
            }                    
        } else if (classType != "." && span == "." && group != ".") {
            if(JackTheStudent.Program.classList.Any(c => c.ShortName == classType)) {
                try {
                    using (var db = new JackTheStudentContext()){
                        var labReports = db.LabReport
                            .Where(x => x.Class == classType && x.GroupId == group)
                            .ToList();                     

                        if (labReports.Count == 0) {
                            string response = "There is no lab report logged for " + JackTheStudent.Program.classList
                                                                                    .Where( c => c.ShortName == classType)
                                                                                    .Select( c => c.Name)
                                                                                    .FirstOrDefault() + " class " + "for group " + group + "!";
                            await ctx.RespondAsync(response);
                            return;
                        } else {
                            string result = String.Empty;
                            foreach (LabReport labReport in labReports) {
                            result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(JackTheStudent.Program.classList
                                                        .Where( c => c.ShortName == labReport.Class)
                                                        .Select( c => c.Name)
                                                        .FirstOrDefault()) + " lab report for group " + labReport.GroupId + ", deadline is/was " + labReport.Date;
                            }
                            await ctx.RespondAsync(result);
                            return;
                        }                           
                    }
                } catch(Exception ex) {
                    Console.Error.WriteLine("[Jack] " + ex.ToString());
                    await ctx.RespondAsync("Show logs failed");
                    return;
                }
            } else {
                await ctx.RespondAsync("Ya know there's only either all possible events or the ones that didn't happen right? Get yo facts straight negro!");
                return;
            }                   
        } else {
            try {
                using (var db = new JackTheStudentContext()){
                    var labReports = db.LabReport.ToList();                     

                    if (labReports.Count == 0) {
                        string response = "There aren no lab reports logged!";
                        await ctx.RespondAsync(response);
                        return;
                    } else {
                        string result = String.Empty;
                        foreach (LabReport labReport in labReports) {
                            result = result + "\n" + CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(JackTheStudent.Program.classList
                                                        .Where( c => c.ShortName == labReport.Class)
                                                        .Select( c => c.Name)
                                                        .FirstOrDefault()) + " lab report for group " + labReport.GroupId + ", deadline is/was " + labReport.Date;
                        }
                        await ctx.RespondAsync(result);
                        return;
                    }
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