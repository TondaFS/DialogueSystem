using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Load Scene", menuName = "Dialogue System/Dialogue Actions/Load Scene", order = 1)]
    public class DialogueAction_LoadScene : DialogueAction
    {
        [SerializeField] private string _sceneName;

        public override void DoAction()
        {
            FadeToBlack.Instance.FadeInAndLoadNewScene(_sceneName);
        }
    }
}
