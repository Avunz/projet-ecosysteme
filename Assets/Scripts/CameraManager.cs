using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{

    [SerializeField]
    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift

    private Enums.CameraMode cameraMode = Enums.CameraMode.Preset;

    public Camera[] cameraList = new Camera[5];


    private int currentPresetIndex = 0;

    public Transform target;

    public Vector3 translationOffset;

    private float angleX = -30;


    



    float camSens = 0.25f; //sensibilité
    private Vector3 lastMouse = new Vector3(255, 255, 255); //au milieu de l'écran
    private float totalRun= 1.0f;

    private GameObject trackingTarget;

    // Start is called before the first frame update
    void Start()
    {
        activateCamera(0);
    }

    // Update is called once per frame
    void Update()
    {
        inputDispatch();
        freeMode(); // doit etre call pas juste si "1" est hold
        keepTracking();

    }


    private void activateCamera(int x){
        for (int i = 0; i < cameraList.Length; i++){
           cameraList[i].gameObject.SetActive(false);

       }

       cameraList[x].gameObject.SetActive(true);
    }

    private void inputDispatch(){


        if (Input.GetKeyDown(KeyCode.Alpha1) & !Input.GetKeyDown(KeyCode.Alpha2) & !Input.GetKeyDown(KeyCode.Alpha3)){

            activateCamera(0);

            cameraMode = Enums.CameraMode.Free;
            trackingTarget = null;

        } else if (!Input.GetKeyDown(KeyCode.Alpha1) & Input.GetKeyDown(KeyCode.Alpha2) & !Input.GetKeyDown(KeyCode.Alpha3)){

            changePreset();


        } else if (!Input.GetKeyDown(KeyCode.Alpha1) & !Input.GetKeyDown(KeyCode.Alpha2) & Input.GetKeyDown(KeyCode.Alpha3)){

            activateCamera(4);


            cameraMode = Enums.CameraMode.Tracking;
            changeTracking();

        }


    }


    private void keepTracking(){
        if (cameraMode != Enums.CameraMode.Tracking)
            return;

        target.SetPositionAndRotation(trackingTarget.transform.position, trackingTarget.transform.rotation);
        cameraList[4].transform.position = target.position + translationOffset;
        cameraList[4].transform.rotation = target.rotation;
        
        cameraList[4].transform.rotation = Quaternion.Euler(30f, 0f, 0f);

        
        
        

    }

    private void changeTracking(){
        trackingTarget = ScanWithTag("Chicken");
        
    }




    private GameObject ScanWithTag(string tag) {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 500);

        while (true){
            int i = Random.Range(0, hitColliders.Length);
            if (hitColliders[i].gameObject.tag.Equals("Chicken")
                | hitColliders[i].gameObject.tag.Equals("Fox")) {
                return hitColliders[i].gameObject;
            }
        }        
    }

    private void changePreset(){
            currentPresetIndex =( (currentPresetIndex + 1) % 3 )+ 1;

            activateCamera(currentPresetIndex);

            
            trackingTarget = null;
    }

    private Vector3 createUnitaryVector() { // fait un vecteur unitaire pour trouver la direction des déplacements
        Vector3 vector = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            vector += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            vector += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A)){
            vector += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            vector += new Vector3(1, 0, 0);
        }
        return vector;
    }



    private void freeMode(){
        if (cameraMode != Enums.CameraMode.Free){
            return;
        }

        lastMouse = Input.mousePosition - lastMouse ;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
        cameraList[0].transform.eulerAngles = lastMouse;





        Vector3 vector = createUnitaryVector();


        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        vector = vector * mainSpeed;
        vector = vector * Time.deltaTime;

        Vector3 newPosition = transform.position;

        cameraList[0].transform.Translate(vector);

    }

}
