using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Framework
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

            var lastScene = SceneManager.GetSceneByPath(_ScenePaths.Last());
            SceneManager.SetActiveScene(lastScene);
        }

        [ContextMenu(nameof(OpenAllScenes))]
        private void OpenAllScenes()
        {
            foreach (var scenePath in _ScenePaths)
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        [ContextMenu(nameof(RemoveAllOpenedScenes))]
        private void RemoveAllOpenedScenes()
        {
            foreach (var scenePath in _ScenePaths)
            {
                var scene = SceneManager.GetSceneByPath(scenePath);
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }
}