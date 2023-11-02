using Managers;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SceneHandler : MonoBehaviour
    {       
        [SerializeField] private GameObject _pressStartText;
        [SerializeField] private Slider _loadingBarFill;
        [SerializeField] private TextMeshProUGUI _progressText;
             
        public void LoadMyScene(int sceneID)
        {   
            gameObject.SetActive(true);
            StartCoroutine(LoadYourAsyncScene(sceneID));
        }

        IEnumerator LoadYourAsyncScene(int sceneID)
        {
            UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
            asyncLoad.allowSceneActivation = false;
            
            while (!asyncLoad.isDone)
            {
                float progressValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);

                _loadingBarFill.value = progressValue;
                _progressText.text = progressValue * 100f + "%";

                if (asyncLoad.progress >= 0.9f)
                {
                    _pressStartText.SetActive(true);
                    if (Input.anyKey)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }
    }
}




