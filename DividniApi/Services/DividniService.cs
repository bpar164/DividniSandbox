using System;
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

        public void createAssessmentFolder(string assessmentName) {
            //Create a subfolder called Assessments (may exist already)
            var folderPath = getDirectory() + "Assessments"; //Replace 'DividniApi' with 'Assessments'
            System.IO.Directory.CreateDirectory(folderPath);
            var assessmentPath = folderPath + "\\" + assessmentName;
            //Create a folder specifically for this assessment
            System.IO.Directory.CreateDirectory(assessmentPath);
        }
    }
}