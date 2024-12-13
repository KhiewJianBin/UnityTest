using UnityEditor;
using UnityEngine;  

public class ScreenShotWindow
{
    //TODO
#if UNITY_STANDALONE || UNITY_WEBPLAYER
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
#endif

    [MenuItem("Utilities/Take Screen Shot")]
    public static void TakeScreenShot()
    {
        //The name of your Screenshot
        string screenshotName = "Screenshot.png";
        
        //Generate a Unique Asset Name each time so we can create many photos. Eg. Screenshot1.png,Screenshot2.png
        screenshotName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + screenshotName).Remove(0, 6);//Remove the "Assets" word

        //Using Absolute Path because of Windows security not able to use reletive path
        string absolutePath = Application.dataPath;
        ScreenCapture.CaptureScreenshot(absolutePath + screenshotName);

        Debug.Log(string.Format("Screenshot Saved : {0}", absolutePath + screenshotName));
    }
}