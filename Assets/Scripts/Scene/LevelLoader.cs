using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class LevelLoader: MonoBehaviour
    {
		public void QuitApplication()
        {
			Application.Quit();
        }
		public static void LoadScene(string s, bool additive = false, bool setActive = false)
		{
			if (s == null)
			{
				s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			}
			UnityEngine.SceneManagement.SceneManager.LoadScene(
				s, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

			if (setActive)
			{
				// to mark it active we have to wait a frame for it to load.
				CallAfterDelay.Create(0, () => {
					UnityEngine.SceneManagement.SceneManager.SetActiveScene(
						UnityEngine.SceneManagement.SceneManager.GetSceneByName(s));
				});
			}
		}
		public static void SimpleLoadScene(Scene s)
		{
			if (s == null)
			{
				s = SceneManager.GetActiveScene();
			}
			// to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create(0, () => {
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(
					UnityEngine.SceneManagement.SceneManager.GetSceneByName(s.name));
			});
		}

		public static void SimpleLoadSceneString(string s)
		{
			if (s == null)
			{
				s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			}
				// to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create(0, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene(s);
				});
		}
		public static void UnloadScene(string s)
		{
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
		}
	}

	public class CallAfterDelay : MonoBehaviour
	{
		float delay;
		System.Action action;

		public static CallAfterDelay Create(float delay, System.Action action)
		{
			CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
			cad.delay = delay;
			cad.action = action;
			return cad;
		}

		float age;

		void Update()
		{
			if (age > delay)
			{
				action();
				Destroy(gameObject);
			}
		}
		void LateUpdate()
		{
			age += Time.deltaTime;
		}
	}
}

