using System;
using System.Collections.Generic;
using System.IO;

namespace DividniApi.Services
{
    public class DividniService
    {
        public string getDirectory()
        {
            //Return the current directory, but remove DividniApi (the code folder) from the path
            return Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 10);
        }

        public List<string> executeCommand(string command)
        {
            var output = new List<string>();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.OutputDataReceived += (sender, args) => output.Add(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return output;
        }

        public string compileQuestion(string question)
        {
            //Create a subfolder called Testing (may exist already)
            var folderPath = getDirectory() + "Testing"; //Replace 'DividniApi' with 'Testing'
            System.IO.Directory.CreateDirectory(folderPath);
            //Create a file from the code

            //System.IO.File.WriteAllBytes(getDirectory() + "\\Assessments\\" + apiObject.assessmentName + "Data.zip", apiObject.data);
            
            /*
            var result = executeCommand("/c cd .. & cd Assessments\\" + assessment.Name + " & csc -t:library -lib:\"C:\\Program Files\\Dividni.com\\Dividni\" -r:Utilities.Courses.dll -out:QHelper.dll " + questionIds);
                    foreach (var line in result) {
                        Console.WriteLine(line);
                    }
            */
            
            //Delete the Testing folder and any subdirectories
            System.IO.Directory.Delete(getDirectory() + "Testing", true);
            //Return the result
            return "Success";
        }
    }
}