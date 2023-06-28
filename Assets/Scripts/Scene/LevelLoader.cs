using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Scenes
{
	public enum Transition
    {
		Fade,
		None
    }


	/// <summary>
	/// Utiliser pour se d�placer de sc�ne en sc�ne.
	/// Possede une option de transition.
	/// </summary>
    public class LevelLoader: MonoBehaviour
    {
		[Header("Type de transition")]
		public Transition tr;
		[Range(0, 5)]
		public float timeTransition;
		public Animator transition;

		[Header("Event")]
		public UnityEvent onTransition;

		[Header("Reference scene pour game maps")]
		public SceneReference sceneref; 
		public void QuitApplication()
        {
			Application.Quit();
        }

        public void Awake()
        {
			transition.SetBool("SceneChangeEnd", true);
            switch (tr)
            {
                case Transition.Fade:
					transition.SetBool("Fade", true);
					break;
                case Transition.None:
					transition.SetBool("Fade", false);
					break;
                default:
                    break;
            }
        }

        public void Update()
        {
            if(transition == null)
            {
				transition = FindObjectOfType<Canvas>().GetComponent<Animator>(); 
            }
        }

		public void reloadScene()
        {
			Scene s = SceneManager.GetActiveScene();
			LoadScene(s.name);
		}

		public void loadNextMap(SceneReference s)
        {
			if(s != null)
            {
				StartCoroutine(LevelLoading(s.sceneSuivante));

			} else
            {
				Debug.LogError("Scene non presente");
				StartCoroutine(LevelLoading("MainMenu"));
            }
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
		public void SimpleLoadScene(Scene s)
		{
			if (s == null)
			{
				s = SceneManager.GetActiveScene();
			}
			// to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create(0, () => {
				StartCoroutine(LevelLoading(s.name));
			});
		}

		public void SimpleLoadSceneString(string s)
		{
			if (s == null)
			{
				s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			}
			// to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create(0, () => {
				StartCoroutine(LevelLoading(s));
				});
		}
		public static void UnloadScene(string s)
		{
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
		}

		public IEnumerator LevelLoading(string scenename)
		{
			onTransition.Invoke();
			transition.SetBool("SceneChangeEnd", false);
			transition.SetBool("SceneChangeBegin", true);

			yield return new WaitForSeconds(timeTransition);

			SceneManager.LoadScene(scenename);

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

