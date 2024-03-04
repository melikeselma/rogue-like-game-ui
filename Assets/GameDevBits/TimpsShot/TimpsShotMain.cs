using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class TimpsShotMain : MonoBehaviour
{

    //reference to the Object holder
    GameObject ObjectHolder;
    //A count of how many children it has. Calculated on start()
    int modelChildCount;
    //Current position in the list of models to cycle through
    int currentChildListPosition = 0;
    //reference to the currently selected model. Used to apply scale and rotation
    GameObject currentChildModel;
    //"Current" state of rotation and scale. Applied to each model viewed in next/prev and when using Save all
    Quaternion currentRotation;
    float currentScale = 1;
    //Reference to the camera
    Camera mainCameraReference;
    //reference to the meshrenderer on the current selected child. Used to find the centre of the object for the initial setup phase
    Renderer m_meshRenderer;
    Vector3 m_Center;
    Vector3 m_worldCenter;
    //The new empty object created for each child in the setup phase
    GameObject newParentEmpty;
    //the 512px item frame and it's centre
    RectTransform itemframeReference;
    Vector2 itemframeCenter;

    //Reference to the master Canvas
    GameObject timpsShotCanvas;

    //UI Components
    //The sliders
    Slider sizeSlider;
    Slider ccSlider;
    Slider panSlider;
    Slider tumbleSlider;
    Slider vOffset;
    Slider hOffset;

    //Light Groups
    GameObject lightGroup1;
    GameObject lightGroup2;
    GameObject lightGroup3;
    GameObject lightGroup4;

    [Header("File Settings")]
    [Tooltip("Folder to save to. This is from the project root. Use \"Assets/\" to save under a subfolder of the Assets folder.")]
    public string saveFolder = "IconOutput";
    [Tooltip("Prefix will be appended to the start of the icon file name.")]
    public string filenamePrefix;

    // Start is called before the first frame update
    void Start()
    {
        //First we find and store references to the things we need
        ObjectHolder = GameObject.Find("ObjectHolder");
        sizeSlider = GameObject.Find("SizeSlider").GetComponent<Slider>();
        ccSlider = GameObject.Find("CCSlider").GetComponent<Slider>();
        tumbleSlider = GameObject.Find("TumbleSlider").GetComponent<Slider>();
        panSlider = GameObject.Find("PanSlider").GetComponent<Slider>();
        vOffset = GameObject.Find("VOffset").GetComponent<Slider>();
        hOffset = GameObject.Find("HOffset").GetComponent<Slider>();
        timpsShotCanvas = GameObject.Find("TimpsShotCanvas");
        

        lightGroup1 = GameObject.Find("LightGroup1");
        lightGroup2 = GameObject.Find("LightGroup2");
        lightGroup3 = GameObject.Find("LightGroup3");
        lightGroup4 = GameObject.Find("LightGroup4");

        //turn on primary lights
        lightGroup1.SetActive(true);
        lightGroup2.SetActive(false);
        lightGroup3.SetActive(false);
        lightGroup4.SetActive(false);




        itemframeReference = GameObject.Find("itemframe").GetComponent<RectTransform>();
        mainCameraReference = GameObject.Find("Main Camera").GetComponent<Camera>();

        //And then we calculate the corner point of the frame and count the children
        itemframeCenter = itemframeReference.anchorMin;
        modelChildCount = ObjectHolder.transform.childCount;
        
        //for each child object we find the centre of the object bounds, create an empty GameObject and attach the object to it.
        //This moves the pivot point of each object dead centre for the clockwise and counterclockwise rotations
        for (int i = 0; i < modelChildCount; i++)
        {
            m_meshRenderer = ObjectHolder.transform.GetChild(i).GetComponent<Renderer>();
            m_worldCenter = m_meshRenderer.bounds.center;
            newParentEmpty = new GameObject();
            newParentEmpty.name = i.ToString();
            newParentEmpty.transform.position = m_worldCenter;
            ObjectHolder.transform.GetChild(i).transform.SetParent(newParentEmpty.transform);
            newParentEmpty.transform.SetParent(ObjectHolder.transform);
            newParentEmpty.transform.SetSiblingIndex(i);
            m_worldCenter = transform.TransformPoint(m_Center);
            newParentEmpty.transform.position = new Vector3(0, 0, 0);                     
            newParentEmpty.SetActive(false);
        }

        ObjectHolder.transform.GetChild(0).gameObject.SetActive(true);
        currentChildModel = ObjectHolder.transform.GetChild(0).gameObject;
        currentRotation = currentChildModel.transform.rotation;
    }


    public void light1()
    {
        lightGroup1.SetActive(true);
        lightGroup2.SetActive(false);
        lightGroup3.SetActive(false);
        lightGroup4.SetActive(false);
    }

    public void light2()
    {
        lightGroup1.SetActive(false);
        lightGroup2.SetActive(true);
        lightGroup3.SetActive(false);
        lightGroup4.SetActive(false);
    }
    public void light3()
    {
        lightGroup1.SetActive(false);
        lightGroup2.SetActive(false);
        lightGroup3.SetActive(true);
        lightGroup4.SetActive(false);
    }
    public void light4()
    {
        lightGroup1.SetActive(false);
        lightGroup2.SetActive(false);
        lightGroup3.SetActive(false);
        lightGroup4.SetActive(true);
    }
    private void OnGUI()
    {
        adjustObject();
    }

    private void adjustObject()
    {
        currentChildModel.transform.localScale = new Vector3(sizeSlider.value * 8, sizeSlider.value * 8, sizeSlider.value * 8);
        currentChildModel.transform.rotation = Quaternion.EulerAngles(tumbleSlider.value, panSlider.value, ccSlider.value);
        currentChildModel.transform.position = new Vector3(hOffset.value * 5, vOffset.value * 5, 0);
    }

    
    //method to disable the current child object and set the next as active
    public void nextModel()
    {
        ObjectHolder.transform.GetChild(currentChildListPosition).gameObject.SetActive(false);
        currentChildListPosition++;
        
        //if function checks if the next object exceeds the child count and restarts at 0
        if (currentChildListPosition > modelChildCount-1)
        {
            currentChildListPosition = 0;
        }
        ObjectHolder.transform.GetChild(currentChildListPosition).gameObject.SetActive(true);
        currentChildModel = ObjectHolder.transform.GetChild(currentChildListPosition).gameObject;
        adjustObject();

    }

    //method to disable the current child object and set the previous as active
    public void prevModel()
    {
        ObjectHolder.transform.GetChild(currentChildListPosition).gameObject.SetActive(false);
        currentChildListPosition--;

        //if function checks if the next object is below zeroand if so sets it to the max child count
        if (currentChildListPosition < 0)
        {
            currentChildListPosition = modelChildCount-1;
        }
        ObjectHolder.transform.GetChild(currentChildListPosition).gameObject.SetActive(true);
        currentChildModel = ObjectHolder.transform.GetChild(currentChildListPosition).gameObject;
        adjustObject();
    }

    //method to set the rotation back to 0 for the current model and the global var
    public void resetRotation()
    {
        currentChildModel.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentRotation = Quaternion.Euler(0, 0, 0);
    }

    //Method to call the single screenshot save routine. We use a coroutine so we can wait til WaitForEndOfFrame() to capture the image
    public void SaveButtonTrigger()
    {
        
        StartCoroutine(takeAndSaveScreenshot());
    }

    //Method to call the Save all screenshot routine. We use a coroutine so we can wait til WaitForEndOfFrame() to capture the image
    public void SaveAllButtonTrigger()
    {
        
     StartCoroutine(SaveAllScreenshots());

    }

    //Coroutine method to save all screenshots. It loops through the total count of children saving a screenshot, then loading the next model
    IEnumerator SaveAllScreenshots()
    {
        currentChildListPosition = 0;

        for (int i = 0; i < modelChildCount; i++)
        {
            StartCoroutine(takeAndSaveScreenshot());
            nextModel();
            yield return new WaitForSeconds(0.01f);
        }
        

    }

    //Coroutine to save the screenshot
     IEnumerator takeAndSaveScreenshot()
    {
        //disable the canvas
        timpsShotCanvas.SetActive(false);
        //creates a new 2d texture of the right size
        var texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        //waits til end of frame. This is the last graphics call made. After GUI and Post Processing have been called
        yield return new WaitForEndOfFrame();
        //We render the main camera
        //mainCameraReference.Render();
        //And then read the pixels from that frame, centred on the frame.
        texture.ReadPixels(new Rect((itemframeCenter.x * Screen.width)-256, (itemframeCenter.y * Screen.height)-256, 512, 512), 0, 0);

        var outputPath = $"{saveFolder}";
        //Check for the output folder and create it if necessary
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        //4 random numbers are joined together to give a unique string for each icon. If a prefix is entered, it goes first
        string spriteName = filenamePrefix+Random.Range(1111, 9999).ToString()+ Random.Range(1111, 9999).ToString() + Random.Range(1111, 9999).ToString() + Random.Range(1111, 9999).ToString();
        //we create the PNG data
        var bytes = texture.EncodeToPNG();
        //We create the full path of folder and file name
        var iconPath = $"{outputPath}/{spriteName}.png";
        //write the actual file
        File.WriteAllBytes(iconPath, bytes);
        //Debug the details to the console to confirm to the user
        Debug.Log($"Icon saved to path '{iconPath}'");
        yield return null;
        //restore the canvas
        timpsShotCanvas.SetActive(true);
    }        
}
