using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kino;


public class TacticalCamera : MonoBehaviour
{
    public Vector3 pointWarp_1, pointWarp_2;
    private bool isPoint_1, isPoint_2;
    public bool isWarp = false;
    private float speedWarp;
    public float speedWarpSeparator;
    public float speedWarpWalls;
    private float distanceAccur;
    public float distanceAccurSeparator;
    public float distanceAccurWalls;
    public int number = 0;
    public GameObject arrow_1;
    public GameObject arrow_2;
    public Image arrow_1Button;
    public Image arrow_2Button;
    public Text arrow_1Text;
    public Text arrow_2Text;
    public AnalogGlitch cameraGluk;
    public AudioListener audioListenerTM;
    private float offsetListiner = 8f;
    private bool isPlanetMap = false;

    //Planet level stuff
    private Vector3 endPos;
    private float speed = 5;
    private bool isCollide = false;


    private void Awake()
    {
        if(!transform.root.GetComponent<PhotonView>().photonView.isMine)
        {
            this.enabled = false;
        }

        if (GarbageInstance.Instanse.curSceneType == CurSceneType.SpaceBattle)
        {
            float cameraOffset = 50;
            pointWarp_1.y = cameraOffset;
            pointWarp_2.y = cameraOffset;
        }
        if (GarbageInstance.Instanse.curSceneType == CurSceneType.PlanetBattle)
        {
            float cameraOffset = 50;
            pointWarp_1.y = cameraOffset;
            pointWarp_2.y = cameraOffset;

            isPlanetMap = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isWarp)
        {
            if (other.tag == "LevelWall") //on planet
            {
                endPos = transform.position + other.transform.forward * 10;
                isCollide = true;
            }
            else //in space
            {
                if (number != 0) //first activation collision
                {
                    if (other.tag == "TMcameraTrigger_1")
                    {
                        WarpToPos_1();
                    }
                    if (other.tag == "TMcameraTrigger_2")
                    {
                        WarpToPos_2();
                    }
                }
                else
                {
                    if (MVC.Instanse.MVCi.Controller.ShipSpawnControll.indexPlayer == 1) //First player (left ring - my, right - enemy)
                    {
                        arrow_2.SetActive(true);
                        number += 1;
                    }
                    if (MVC.Instanse.MVCi.Controller.ShipSpawnControll.indexPlayer == 2) //First player (left ring - my, right - enemy)
                    {
                        arrow_1.SetActive(true);
                        number += 1;
                    }
                }

                if (other.tag == "TMcameraWall_1")
                {
                    WarpToPos_1();
                }
                if (other.tag == "TMcameraWall_2")
                {
                    WarpToPos_2();
                }
            } 
        }
    }

    public void WarpToPos_1()
    {
        speedWarp = speedWarpSeparator;
        distanceAccur = distanceAccurSeparator;
        isPoint_1 = true;
        isWarp = true;
        arrow_1.SetActive(false);
        arrow_2.SetActive(true);
        GameEvents.Instanse.warpPos_1.Invoke(true);
    }

    public void WarpToPos_2()
    {
        speedWarp = speedWarpSeparator;
        distanceAccur = distanceAccurSeparator;
        isPoint_2 = true;
        isWarp = true;
        arrow_1.SetActive(true);
        arrow_2.SetActive(false);
        GameEvents.Instanse.warpPos_2.Invoke(true);
    }

    private void FixedUpdate()
    {
        bool cameraIsFly;
        cameraIsFly = MVC.Instanse.MVCi.Controller.tacticalModeController.isCameraFly;

        if(isPlanetMap)
        {
            //calculate listiner pos
            int layer = LayerMask.GetMask("Terrain");
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                offsetListiner =  hit.point.y + 8f;
            }

            if(isCollide)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, endPos) <= 5f)
                {
                    isCollide = false;
                }
            }
        }
        audioListenerTM.transform.position = new Vector3(transform.position.x, offsetListiner, transform.position.z);

        if (cameraIsFly) return;

        if(isWarp)
        {
            arrow_1.SetActive(false);
            arrow_2.SetActive(false);
            if (isPoint_1)
            {
                transform.position = Vector3.Lerp(transform.position, pointWarp_1, Time.deltaTime * speedWarp);
                float distanceToPoint = Vector3.Distance(transform.position, pointWarp_1);
                if (distanceToPoint <= distanceAccur)
                {
                    isPoint_1 = false;
                    isPoint_2 = false;
                    isWarp = false;
                    arrow_2.SetActive(true);
                }
            }
            if (isPoint_2)
            {
                transform.position = Vector3.Lerp(transform.position, pointWarp_2, Time.deltaTime * speedWarp);
                float distanceToPoint = Vector3.Distance(transform.position, pointWarp_2);
                if (distanceToPoint <= distanceAccur)
                {
                    isPoint_2 = false;
                    isPoint_1 = false;
                    isWarp = false;
                    arrow_1.SetActive(true);
                }
            }
        }
      
    }

    public void CameraGluke()
    {
        if (transform.root.GetComponent<PhotonView>().photonView.isMine)
        {
            StartCoroutine(C_CameraGluke());
        }
    }

    public IEnumerator C_CameraGluke()
    {
        cameraGluk.enabled = true;
        yield return new WaitForSeconds(0.6f);
        cameraGluk.enabled = false;
    }


}
