using ColorSnipersU.Utilities.Enumerations;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorSnipersU.Utilities
{
    public static class HelperFunctions
    {
        public static void LoadSceneAsync(GameScenes gameSceneID, MonoBehaviour sceneMonoBehavior, object yieldObject)
        {
            sceneMonoBehavior.StartCoroutine(LoadSceneAsyncCoroutine(gameSceneID, yieldObject));
        }
        private static IEnumerator LoadSceneAsyncCoroutine(GameScenes gameSceneID, object yieldObject)
        {
            yield return yieldObject;
            SceneManager.LoadSceneAsync((int)gameSceneID);
        }

        public static void LoadScene(GameScenes gameSceneID)
        {
            SceneManager.LoadScene((int)gameSceneID);
        }

        public static void MoveObejectTowardsPosition(MonoBehaviour MB, GameObject targetGO, Vector3 targetPosition, float speed = 1.0f)
        {
            MB.StartCoroutine(MoveObejectTowardsPositionCoroutine(targetGO, targetPosition, speed));
        }
        private static IEnumerator MoveObejectTowardsPositionCoroutine(GameObject targetGO, Vector3 targetPosition, float speed = 1.0f)
        {
            while (targetGO != null && targetGO.transform.position != targetPosition)
            {
                // The step size is equal to speed times frame time.
                float step = speed * Time.deltaTime;

                // Move our position a step closer to the target.
                targetGO.transform.position = Vector3.MoveTowards(targetGO.transform.position, targetPosition, step);

                yield return null;
            }
        }

        public static void ShareData(string destination)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your new Score");

            currActivity.Call("startActivity", chooser);
        }

        public static void ShareScreenshot()
        {
            ScreenCapture.CaptureScreenshot("screenshot.png", 2);
            string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");
            ShareData(destination);
        }
    }
}