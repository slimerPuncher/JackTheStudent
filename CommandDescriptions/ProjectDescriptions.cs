using System;

namespace JackTheStudent.CommandDescriptions
{
    public class ProjectDescriptions
    {
    public const string projectLogDescription = "Command logging a project, last two arguments are optional." +
        "\nTo log without addInfo but with materials use \".\" where addInfo should be.\n" +
        "Words seperated with spaces must be wrapped with \"\"\n" +
        "\n!project <groupId> <classShortName> <projectDate> <projectTime> <additionalInfo> <materials>\n" + 
        "\nExamples:\n" +
        "\n!project 3 mat 05-05-2021 13:30" + 
        "\n!project 1 ele 05-05-2021 12:30 \"Calculator required\"" +
        "\n!project 3 mat 05-05-2021 13:30 \"Calculator required\" \"https://yourmaterials.com\"" +
        "\n!project 1 eng 05-05-2021 13:30 . \"https://yourmaterials.com\"";
    

    public const string projectLogsDescription = "Command retrieving logged project based on passed arguments, ALL arguments are optional and the command has default settings.\n" +
        "\n!projects <groupId> <classShortName> <alreadyTookPlace?>\n" + 
        "\nType !classes to retrieve short names and !groups to retrieve group IDs" +
        "\nUse \".\" to retrieve ALL possible entries for each argument, <alreadyTookPlace?> takes \"planned\" or \".\"\n" +
        "\nExamples:\n" +
        "\n!projects - will retrieve all PLANNED projects for all the groups and all the classes" + 
        "\n!projects 1 - will retrieve all PLANNED projects for group 1 for all the classes" +
        "\n!projects 1 mat - will retrieve all PLANNED projects for group 1 for Maths class" +
        "\n!projects 1 mat planned - will retrieve all PLANNED projects for group 1 for Maths class" +
        "\n!projects 1 mat . - will retrieve all LOGGED projects for group 1 for Maths class" +
        "\n!projects 1 . . - will retrieve all LOGGED projects for group 1 for ALL classes" + 
        "\n!projects . . . - will retrieve all LOGGED projects for ALL groups for ALL classes" +
        "\n!projects . mat . - will retrieve all LOGGED projects for ALL groups for MAths class" +
        "\n!projects . . planned - will retrieve all PLANNED projects for ALL groups for ALL classes";
    }

}