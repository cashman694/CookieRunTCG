using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class DebugSceneLoader : MonoBehaviour
    {
        [SerializeField] private string[] _ScenePaths;

        private async UniTaskVoid Start()
        {
            foreach (var scenePath in _ScenePaths)
            {
                await SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            }
        }
    }
}