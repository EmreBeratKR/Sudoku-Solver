using UnityEngine;

namespace UI
{
    public class SourceCode : MonoBehaviour
    {
        public void Open()
        {
            WebRequest.OpenGithubRepository();
        }
    }
}
