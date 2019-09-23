using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    VideoPlayer vp = null;

    // Start is called before the first frame update
    void Awake()
    {
        vp = gameObject.GetComponent<VideoPlayer>();
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "glitch.mp4");
    }

    
}
