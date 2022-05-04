using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField]
    private Material[] boxes = new Material[7];
    private Skybox skybox;
    private float timeElapsed;
    private int index = 0;

    float duration = 2.0f;
    
    Renderer rend;

    // Start is called before the first frame update
    void Start()// vaguement inspiré de https://docs.unity3d.com/ScriptReference/Material.Lerp.html
    {
    
        skybox = GetComponent<Skybox>();
        RenderSettings.skybox = boxes[index];
        //skybox.material();

    }

    // Update is called once per frame
    void Update()
    {             
            checkSkybox();
    }

    private void checkSkybox(){
        
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1){// si x secondes sont écoulés -> on update le skybox
            updateSkybox();
            timeElapsed = 0;
        }
    }

    private void updateSkybox(){           
        index = (index + 1) % 9;    
        RenderSettings.skybox = boxes[index];        
    }
}
