using System;

namespace Livodyo.WebAdmin.Lib
{
    public static class Helpers
    {
        /// <summary>
        /// Gets all 3 environmentvariables by priority (Process > User > Machine), we need this for docker's API endpoint
        /// Stolen from my previous work ☜(ﾟヮﾟ☜)
        /// </summary>
        /// <param name="varName">Single_String_VarName</param>
        /// <returns>Value of Environment variable</returns>
        public static string GetEnvironmentVariable(string varName)
        {
            varName = varName.ToUpper();

            var processVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(processVar)) return processVar;

            var userVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(userVar)) return userVar;

            var machineVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(machineVar)) return machineVar;

            return string.Empty;
        }
    }
}
