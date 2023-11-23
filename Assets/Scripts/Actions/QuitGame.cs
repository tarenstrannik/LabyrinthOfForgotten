using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame: MonoBehaviour
{
    public void QuitGameMethod()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();

#else
#if UNITY_WEBGL
        SceneManager.LoadScene(0);
#else
        Application.Quit();
#endif
#endif
    }
}
