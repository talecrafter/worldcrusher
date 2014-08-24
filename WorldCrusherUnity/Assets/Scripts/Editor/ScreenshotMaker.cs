using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;

/// <summary>
/// Unity Editor Script for making screenshots in the editor
/// use shortcut Ctrl+Alt+Shift+S
/// use Before and AfterScreenshotHandler to disable and reenable debug GUI
/// </summary>
[InitializeOnLoad]
public class ScreenshotMaker {

    public enum ScreenshotTakingPhase
    {
        None,
        Started,
        AfterFrame,
        StartedWithoutInterface,
        AfterFrameWithoutInterface
    }

    public static Action BeforeScreenshotHandler;
    public static Action AfterScreenshotHandler;

    public static Action BeforeScreenshotWithoutInterfaceHandler;
    public static Action AfterScreenshotWithoutInterfaceHandler;

    private static ScreenshotTakingPhase screenshotMakingPhase = ScreenshotTakingPhase.None;

    /// <summary>
    /// static constructor - attach to the unity editor frame handler
    /// </summary>
    static ScreenshotMaker()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        // have to make sure AfterScreenshot gets called the frame after the next one;
        // otherwise screenshot taking could not have happened

        // post processing for making screenshots
        if (screenshotMakingPhase == ScreenshotTakingPhase.AfterFrame)
        {
            if (AfterScreenshotHandler != null)
            {
                AfterScreenshotHandler();
            }
            screenshotMakingPhase = ScreenshotTakingPhase.None;
        }
        if (screenshotMakingPhase == ScreenshotTakingPhase.AfterFrameWithoutInterface)
        {
            if (AfterScreenshotWithoutInterfaceHandler != null)
            {
                AfterScreenshotWithoutInterfaceHandler();
            }
            screenshotMakingPhase = ScreenshotTakingPhase.None;
        }

        // wait until next frame
        if (screenshotMakingPhase == ScreenshotTakingPhase.Started)
        {
            screenshotMakingPhase = ScreenshotTakingPhase.AfterFrame;
        }
        if (screenshotMakingPhase == ScreenshotTakingPhase.StartedWithoutInterface)
        {
            screenshotMakingPhase = ScreenshotTakingPhase.AfterFrameWithoutInterface;
        }
    }

    [MenuItem("Framework/Screenshot #%&s")]
    public static void TakeScreenshotMenuEntry()
    {
        StartMakingScreenshot();
    }

    [MenuItem("Framework/Screenshot #%&s", true)]
    public static bool TakeScreenshotMenuEntryValidator()
    {
        return Application.isPlaying;
    }

    [MenuItem("Framework/Screenshot (No Interface) #%&d")]
    public static void TakeScreenshotWithoutInterfaceMenuEntry()
    {
        StartMakingScreenshotWithoutInterface();
    }

    [MenuItem("Framework/Screenshot (No Interface) #%&d", true)]
    public static bool TakeScreenshotWithoutInterfaceMenuEntryValidator()
    {
        return Application.isPlaying;
    }

    private static void StartMakingScreenshot()
    {
        // pre processing for making screenshots
        if (BeforeScreenshotHandler != null)
            BeforeScreenshotHandler();

        MakeScreenshotWithTimeString();

        screenshotMakingPhase = ScreenshotTakingPhase.Started;
    }

    private static void StartMakingScreenshotWithoutInterface()
    {
        // pre processing for making screenshots
        if (BeforeScreenshotWithoutInterfaceHandler != null)
            BeforeScreenshotWithoutInterfaceHandler();

        MakeScreenshotWithTimeString();

        screenshotMakingPhase = ScreenshotTakingPhase.StartedWithoutInterface;
    }

    private static void MakeScreenshotWithTimeString()
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string screenshotName = "../Promotion/Screenshots/screenshot_" + Screen.width + "x" + Screen.height + "_" + timeStamp + ".png";
        string path = Application.dataPath + "../../../Promotion/Screenshots";
        Directory.CreateDirectory(path);
            
        Application.CaptureScreenshot(screenshotName);
    }

}