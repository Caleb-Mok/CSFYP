using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;

public class RunPythonScript : MonoBehaviour
{
     
    private string pythonExecutable = "python";
    private string pythonScriptRelative = "Python Script/pdf_generator.py";
    private string runPath;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetRunPath(string path)
    {
        runPath = path;
    }

    public void RunPython()
    {

        if (string.IsNullOrEmpty(runPath))
        {
            UnityEngine.Debug.LogWarning("RunPythonScript: No runPath set. Skipping Python script.");
            return;
        }

        string pythonScript = Path.Combine(Application.dataPath, pythonScriptRelative);
        string args = $"\"{pythonScript}\" \"{runPath}\"";
        UnityEngine.Debug.Log("Running Python with args: " + args);


        ProcessStartInfo startInfo  = new ProcessStartInfo
        {
            FileName = pythonExecutable,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true, // True redirects output to unity
            RedirectStandardError = true // True Redirects errors
        };

        using (Process proc = Process.Start(startInfo))
        {
            string stdout = proc.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            UnityEngine.Debug.Log("STDOUT: " + stdout);
            if (!string.IsNullOrEmpty(stderr)){
                UnityEngine.Debug.LogWarning("STDERR: " + stderr);
            }
                
        }
    }
}

