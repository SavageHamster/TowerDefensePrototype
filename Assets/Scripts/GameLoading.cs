using UnityEngine;
using System.Collections;

public sealed class GameLoading : MonoBehaviour
{
    private void Start()
    {
        ScenesController.Instance.LoadScene(ScenesController.Scene.Menu);
    }
}
