using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private float loadingTimer = 0f;
    float loadNewSceneTimer = 1.3f;
    private float loadingTimerMax = 1.5f;
    bool targetSceneToBeLoaded = true;

    void Update()
    {
        if (loadingTimer < loadingTimerMax) {
            loadingTimer+=Time.deltaTime;
            if (loadingTimer >= loadNewSceneTimer && targetSceneToBeLoaded) {
                Loader.LoaderCallback(); targetSceneToBeLoaded = false;
            }
        }

    }
}
