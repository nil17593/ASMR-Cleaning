using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Fluxy;
//using Obi;

public class GameManager : MonoBehaviour
{
    [Header("STARTING TEXTURES")]
    public Texture2D texture1;
    public Texture2D texture2;
    public Texture2D texture3;

    [Header("RING MATERIALS")]
    public Material material1;
    public Material material2;
    public Material material3;

    [Header("AFTER CLEANING TEXTURES")]
    public Texture2D AfterCleaningtex1;
    public Texture2D AfterCleaningtex2;
    public Texture2D AfterCleaningtex3;
    

    [Header("TRANSFORMS")]
    public Transform RingHolder_Dipping;
    public Transform RingHolder_Steaming;
    public Transform water_Emmiting;
    [Tooltip("transform to emmit dust effect")]
    public Transform targetTransform1;//1st fluxy target 
    [Tooltip("transform to emmit dust effect")]
    public Transform targetTransform2;//2nd fluxy target

    [Header("FILLBAR SETTINGS")]
    public Image ProgressFillingBar;
    public float TimeforProgress;

   
    public enum GameState { None, Polishing, Pouring, Dipping, Steaming,End };//34.1 for steaming
    [Header("ENUM")]
    public GameState presentGameState;

   
    [Header("BOOLS")]
    public bool canProgress;
    public bool onlyOnce;
    public bool canHandleTouch;

    public int multiplyVal;

    [Header("GAMEOBJECTS")]
    public GameObject firstStepRing;
    public GameObject mug;
    public GameObject waterEmitter;
    public GameObject ringAtTub;
    public FluxyTarget target_DustEffect1;
    public FluxyTarget target_DustEffect2;
    //public GameObject target_DustEffect;
    public GameObject container;
    public GameObject steamPartical;
    public ParticleSystem sparxPartical;
    public GameObject finalStepRing;
    public GameObject steamer;

    [Header("ANIMATORS")]
    public Animator RingHolder_DAnimator;
    public Animator RingHolder_SAnimator;

    public FluxyTarget[] targets;

    public static GameManager Instance;


    void Start()
    {
        Instance = this;
        canHandleTouch = true;
        waterEmitter.gameObject.SetActive(false);
        target_DustEffect1.enabled = false;
        target_DustEffect2.enabled = false;
        steamPartical.SetActive(false);

        material1.DisableKeyword("_EMISSION");
        material2.DisableKeyword("_EMISSION");
        material3.DisableKeyword("_EMISSION");

        material1.mainTexture = texture1;
        material2.mainTexture = texture2;
        material3.mainTexture = texture3;
    }

    // Update is called once per frame
    void Update()
    {
        if (canProgress)
        {
            if (presentGameState == GameState.Polishing || presentGameState == GameState.Steaming)
                multiplyVal = 7;
            if (presentGameState == GameState.Pouring)
                multiplyVal = 12;
            if (presentGameState == GameState.Dipping)
                multiplyVal = 14;

            if (TimeforProgress > 0)
            {
                TimeforProgress -= Time.deltaTime;
                ProgressFillingBar.fillAmount += 0.4f / multiplyVal * Time.deltaTime;
            }
            else
            {
                if (!onlyOnce)
                {
                    onlyOnce = true;
                    if (presentGameState == GameState.Polishing)
                    {
                        firstStepRing.GetComponent<Rotate>().canRotate = false;
                        sparxPartical.Play();
                        StartCoroutine(NextStep("Pouring"));
                    }
                    else if (presentGameState == GameState.Pouring)
                    {
                        sparxPartical.Pause();
                        StartCoroutine(NextStep("Dipping"));

                    }
                    else if (presentGameState == GameState.Dipping)
                    {
                        StartCoroutine(NextStep("Steaming"));

                    }
                    else if (presentGameState == GameState.Steaming)
                    {
                        StartCoroutine(NextStep("End"));
                    }
                    else
                    {

                    }
                }
                canProgress = false;
            }
        }

        if (canHandleTouch)
        {
            if (Input.GetMouseButton(0))
            {
                canProgress = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                canProgress = false;
            }
        }

    }


    public IEnumerator NextStep(string name)
    {
        canHandleTouch = false;
        yield return new WaitForSeconds(1);
        CameraPositin(name);
    }

        //step 2=----17.5f,6.9f,-6.9f

    public void CameraPositin(string step)
    {
        switch(step)
        {
            case "Polishing":
                break;
            case "Pouring":
                presentGameState = GameState.Pouring;
                onlyOnce = false;
                TimeforProgress = 6;
                Camera.main.transform.DOLocalMove(new Vector3(17.5f, 6.9f, -6.9f), 2).OnComplete(WaterPouring);
                StartCoroutine(Enable_Touch(3));
                
              //  Camera.m
                break;
            case "Dipping":
                presentGameState = GameState.Dipping;
                onlyOnce = false;
                TimeforProgress = 6;
                Camera.main.transform.DOLocalMove(new Vector3(20, 5f, -1f), 2).OnComplete(RingHolder_D_Pos);
                Camera.main.transform.DOLocalRotate(new Vector3(60.5f, -66, 0), 2);
                StartCoroutine(Enable_Touch(2));

                //target_DustEffect.SetActive(true);
                Camera.main.fieldOfView = 55;            
                Debug.Log("---Dipping-----");
                break;
            case "Steaming":
                presentGameState = GameState.Steaming;
                onlyOnce = false;
                TimeforProgress = 5;
                StartCoroutine(Enable_Touch(2));
                Camera.main.transform.DOLocalMove(new Vector3(34f, 3.9f, -4.5f), 2).OnComplete(RingHolder_S_Pos);
                Camera.main.transform.DOLocalRotate(new Vector3(38.229f, 0, 0), 2);

                Camera.main.fieldOfView = 60;
           
                Debug.Log("---Steaming----");
                break;
            case "End":
                presentGameState = GameState.End;
                finalStepRing.transform.SetParent(null);
                RingHolder_SAnimator.enabled = false;
                RingHolder_Steaming.transform.DOLocalMoveX(31.6f, 1f);
                steamer.transform.DOLocalMoveX(37.84f, 1f);
                finalStepRing.transform.DOLocalMove(new Vector3(33.98f, 3.08f, -3.62f), 2f).OnComplete(EndState);
                
                Debug.Log("----Dead End----");
                break;

        }
    }


    //End State
    public void EndState()
    {
        finalStepRing.GetComponent<Rotate>().enabled = true;
        canHandleTouch = false;
        finalStepRing.GetComponent<Rotate>().CanControl = false;
    }

    public void RingHolder_D_Pos()
    {
        mug.SetActive(false);
        RingHolder_Dipping.gameObject.SetActive(true);
        RingHolder_Dipping.DOLocalMove(new Vector3(18.4f, 2.1f, 0.9f), 0.5f);

        //StartCoroutine(WashRing());

        RingHolder_DAnimator.Play("RingCleaningInWater");

        //+ new Vector3(-0.08f, -0.8f, -0.1f);
        container.gameObject.transform.DOLocalMoveY(1.23f, 4f);
        StartCoroutine(Dusting());       
        //container.SetActive(true);
    }

    public void RingHolder_S_Pos()
    {
        RingHolder_Steaming.gameObject.SetActive(true);
        //target_DustEffect.SetActive(false);

        waterEmitter.SetActive(false);
        RingHolder_Steaming.DOLocalMove(new Vector3(33.5f, 2.07f, -2.58f), 0.5f);
        steamPartical.SetActive(true);
        RingHolder_SAnimator.Play("RingHolderAtSteamer");
    }

   
    public void WaterPouring()
    {
        mug.transform.eulerAngles = new Vector3(mug.transform.rotation.x + 95f, mug.transform.rotation.y - 105f, mug.transform.rotation.z + 0f);
        waterEmitter.transform.position = water_Emmiting.position;
        waterEmitter.gameObject.SetActive(true);
    }


    //Coroutine for Dusting Effect
    IEnumerator Dusting()
    {
        target_DustEffect1.transform.SetParent(targetTransform1.transform);
        target_DustEffect2.transform.SetParent(targetTransform2.transform);
        //container.gameObject.transform.DOLocalMoveY(1.23f, 4f).OnComplete(() => target_DustEffect.SetActive(true));
        target_DustEffect1.transform.position = targetTransform1.transform.position;
        target_DustEffect2.transform.position = targetTransform2.transform.position;
        Debug.Log(Time.time);
        //yield return new WaitForSeconds(1f);
        Debug.Log("1"+Time.time);
        target_DustEffect1.enabled = true;
        target_DustEffect2.enabled = true;

        target_DustEffect1.scale = new Vector2(0.02f, 0.02f);
        target_DustEffect1.rateOverDistance = 0;
        target_DustEffect1.rateOverTime = 0.1f;
        target_DustEffect1.rateOverSteps = 0;

        target_DustEffect2.scale = new Vector2(0.02f, 0.02f);
        target_DustEffect2.rateOverDistance = 0;
        target_DustEffect2.rateOverTime = 0.1f;
        target_DustEffect2.rateOverSteps = 0;

        yield return new WaitForSeconds(2f);
        Debug.Log("2" + Time.time);
        target_DustEffect1.scale = new Vector2(0.03f, 0.03f);
        target_DustEffect1.rateOverDistance = 0;
        target_DustEffect1.rateOverTime = 0.3f;
        target_DustEffect1.rateOverSteps = 0;
       

        target_DustEffect2.scale = new Vector2(0.03f, 0.03f);
        target_DustEffect2.rateOverDistance = 0;
        target_DustEffect2.rateOverTime = 0.3f;
        target_DustEffect2.rateOverSteps = 0;

        yield return new WaitForSeconds(2f);
        Debug.Log("3" + Time.time);
        target_DustEffect1.scale = new Vector2(0.04f, 0.04f);
        target_DustEffect1.positionRandomness = 0.03f;
        //target_DustEffect.rotationRandomness = 0.1f;
        target_DustEffect1.rateOverDistance = 0.03f;
        target_DustEffect1.rateOverTime = 0.7f;
        target_DustEffect1.rateOverSteps = 1;
        target_DustEffect1.GetComponent<SetRandomForce>().enabled = true;

        target_DustEffect2.scale = new Vector2(0.04f, 0.04f);
        target_DustEffect2.positionRandomness = 0.03f;
        //target_DustEffect.rotationRandomness = 0.1f;
        target_DustEffect2.rateOverDistance = 0.01f;
        target_DustEffect2.rateOverTime = 0.7f;
        target_DustEffect2.rateOverSteps = 1;
        target_DustEffect2.GetComponent<SetRandomForce>().enabled = true;
       
       
        


        yield return new WaitForSeconds(3f);
        Debug.Log("4" + Time.time);
        target_DustEffect1.scale = new Vector2(0.1f, 0.1f);
        //target_DustEffect.positionRandomness = 0.02f;
        //target_DustEffect.rotationRandomness = 0.2f;
        target_DustEffect1.rateOverDistance = 0.5f;
        target_DustEffect1.rateOverTime = 2f;
        target_DustEffect1.rateOverSteps = 2;

        target_DustEffect2.scale = new Vector2(0.1f, 0.1f);
        //target_DustEffect.positionRandomness = 0.02f;
        //target_DustEffect.rotationRandomness = 0.2f;
        target_DustEffect2.rateOverDistance = 0.5f;
        target_DustEffect2.rateOverTime = 2f;
        target_DustEffect2.rateOverSteps = 2;
        ApplyAfterDustingTextures();
        //target_DustEffect.transform.SetParent(null);

        //adding extra delay because dusting stops before Dipping animation ends
        yield return new WaitForSecondsRealtime(20);
        target_DustEffect1.enabled = false;
        target_DustEffect2.enabled = false;
        
    }

    //Apply Ring Cleaned textures
    void ApplyAfterDustingTextures()
    {
        material1.mainTexture = AfterCleaningtex1;
        material2.mainTexture = AfterCleaningtex2;
        material3.mainTexture = AfterCleaningtex3;

        if (!material1.IsKeywordEnabled("_EMISSION"))
        {
            Debug.Log("P");
            material1.EnableKeyword("_EMISSION");
            Debug.Log("Material Emission after ON " + material1.IsKeywordEnabled("_EMISSION"));
        }

        if (!material2.IsKeywordEnabled("_EMISSION"))
        {
            material2.EnableKeyword("_EMISSION");
        }


        if (!material3.IsKeywordEnabled("_EMISSION"))
        {
            material3.EnableKeyword("_EMISSION");
        }
    }

    public IEnumerator Enable_Touch(float sec)
    {
        yield return new WaitForSeconds(sec);
        canHandleTouch = true;
    }
}
