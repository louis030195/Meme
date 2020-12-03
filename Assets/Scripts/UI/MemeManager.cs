using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MemeManager : MonoBehaviour
    {
        public Meme[] memes;
        public GameObject memePrefab;
        public Transform memeContainer;
        private void Start()
        {
            foreach (var m in memes)
            {
                var go = Instantiate(memePrefab, memeContainer);
                go.GetComponentInChildren<Text>().text = m.memeName;
                go.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(m.sceneName));
                // TODO: maybe have to reset camera position to 0 0 0
            }
        }
    }
}
