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

        public void createAssessmentFolder()
        {
            //Create a subfolder called Assessments (may exist already)
            var folderPath = getDirectory() + "Assessments"; //Replace 'DividniApi' with 'Assessments'
            System.IO.Directory.CreateDirectory(folderPath);
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
        public byte[] getAssessmentFile(string name)
        {
            byte[] data = System.IO.File.ReadAllBytes(getDirectory() + "\\Assessments\\" + name + ".zip");
            deleteAssessmentFolder();
            return data;
        }

        //Delete the Assessments folder and any subdirectories
        public void deleteAssessmentFolder()
        {
            System.IO.Directory.Delete(getDirectory() + "Assessments", true);
        }

        public string CompileQuestion()
        {
            /*
            var result = executeCommand("/c cd .. & cd Assessments\\" + assessment.Name + " & csc -t:library -lib:\"C:\\Program Files\\Dividni.com\\Dividni\" -r:Utilities.Courses.dll -out:QHelper.dll " + questionIds);
                    foreach (var line in result) {
                        Console.WriteLine(line);
                    }
            */
            return "";
        }

        public byte[] generateStandard(string assessmentName, string questionIds, int versions, byte[] data)
        {
            createAssessmentFolder();
            //Create zip file from data
            System.IO.File.WriteAllBytes(getDirectory() + "\\Assessments\\" + assessmentName + "Data.zip", data); 
            //Extract zip file contents to Assessments
            executeCommand("/c cd .. & cd Assessments & tar xf " + assessmentName + "Data.zip " + assessmentName);
            //Compile all of the questions
            executeCommand("/c cd .. & cd Assessments\\" + assessmentName + " & csc -t:library -lib:\"C:\\Program Files\\Dividni.com\\Dividni\" -r:Utilities.Courses.dll -out:QHelper.dll " + questionIds);
            //Generate assessment
            executeCommand("/c cd .. & cd Assessments\\" + assessmentName + " & TestGen -lib QHelper.dll -htmlFolder papers -answerFolder answers -paperCount " + versions + " Assessment.Template.html");
            //Compress the folder contents into a .zip archive
            executeCommand("/c cd .. & cd Assessments & tar cf " + assessmentName + ".zip " + assessmentName);
            //Create a byte[] to hold the .zip archive contents
            var result = getAssessmentFile(assessmentName);
            //Return the byte[]
            return result;
        }

        public byte[] generateLMS(string assessmentName, string questionIds, int versions, string type, byte[] data)
        {
            createAssessmentFolder();
            //Create zip file from data
            System.IO.File.WriteAllBytes(getDirectory() + "\\Assessments\\" + assessmentName + "Data.zip", data); 
            //Extract zip file contents to Assessments
            executeCommand("/c cd .. & cd Assessments & tar xf " + assessmentName + "Data.zip " + assessmentName);
            //Generate the assessment, based on type
            if (type == "moodle")
            {
                executeCommand("/c cd .. & cd Assessments\\" + assessmentName + " & MoodleGen -variants " + versions + " -xmlFolder questions -bank " + assessmentName + " " + questionIds);
            }
            else
            {
                //Determine qtiVers
                var qtiVers = "";
                if (type == "canvas")
                {
                    qtiVers = "1.2";
                }
                else
                {
                    qtiVers = "2.1";
                }
                //Create LMS compatible zip 
                executeCommand("/c cd .. & cd Assessments\\" + assessmentName + " & QtiGen -qtiVersion " + qtiVers + " -variants " + versions + " -id " + assessmentName + " " + questionIds);
            }
            //Compress the folder contents into a .zip archive
            executeCommand("/c cd .. & cd Assessments & tar cf " + assessmentName + ".zip " + assessmentName);
            //Create a byte[] to hold the .zip archive contents
            var result = getAssessmentFile(assessmentName);
            //Return the byte[]
            return result;
        }
    }
}