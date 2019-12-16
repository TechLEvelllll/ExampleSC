using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Controller for simple weapon (firing bullet from pistol, rifle, mashingun)
public class WeaponController : Weapon
{
    [Header("Simple bullet weapon")]
    TracerSimple tracerSimple;
    public float resetLifetime = 1f; //How fast dissapear tracer
    private int uvAnimationTileX = 4;
    private int uvAnimationTileY = 16;
    private float framesPerSecond = 30.0f;
    private int _lastIndex = -1;
    LineRenderer tracerLine; //Our tracer line


    protected override void Awake()
    {
        base.Awake();
        // pvpFlag = false;

        photonView.RPC("SetTracer", PhotonTargets.AllBuffered);
    }

    protected override void Start()
    {
        base.Start();
        currAmmo = maxAmmo; //Loading in weapon full ammo on start
    }

    void OnDisable()
    {
        if (tracerLine != null)
        {
            tracerLine.enabled = false; //When weapon disabled we need disable tracer line
        }
    }

    void Update()
    {
        if (photonView.isMine && isControlPlayer == true && isLock == false) //Player fire
        {
            if (MVC.Instanse != null && !MVC.Instanse.MVCi.Controller.tacticalModeController.tmActived && !GarbageInstance.Instanse.InventoryActive)
            {
                SceneSwitchManager ssm = GarbageInstance.Instanse.ssm;
                if (ssm != null && ssm.curScene.isNeutral == true)
                {
                    GameEvents.Instanse.lockWeapon.Invoke(true);
                }

                //Shoot 
                if (Input.GetMouseButton(0) && Time.time > nextFire && currAmmo > 0)
                {
                    PlayerFire();
                }

                //Reloading
                if (currAmmo == 0 || Input.GetKeyDown("r"))
                {
                    nextFire = Time.time + reloadTime;  //set reload time
                    currAmmo = maxAmmo;
                    photonView.RPC("SoundReload", PhotonTargets.All);
                    GameEvents.Instanse.reloadWeapon.Invoke(true); //For animated aim
                    GameEvents.Instanse.reloadTime.Invoke(reloadTime); //For animated aim
                }
            }

        }

        if (photonView.isMine && isControlPlayer == false) //Bot controlled weapon
        {
            //Reloading
            if (currAmmo == 0)
            {
                nextFire = Time.time + reloadTime;  //set reload time
                currAmmo = maxAmmo;
                photonView.RPC("SoundReload", PhotonTargets.All);
            }
        }

    }

    #region Tracer

    [PunRPC]
    void SetTracer()
    {
        tracerLine = GetComponent<LineRenderer>(); //Take lineRenderer from weapon
        tracerSimple = GetComponent<TracerSimple>(); //Take tracer Controller
        tracerSimple.tracerLineRenderer = tracerLine; //Give to tracer controller our lineRenderer
        tracerSimple.tracerLineRenderer.SetPosition(0, new Vector3(0, 0, 0)); //Set on zero lineRenderer
        tracerSimple.tracerLineRenderer.SetPosition(1, new Vector3(0, 0, 0));
    }

    [PunRPC]
    public void TracerStart(Vector3 hitInfo) //Take info about hit from weaponController and send she to TracerGOGO
    {
        try
        {
            if (fireSpawnpoint.activeSelf == true)
            {
                StartCoroutine("C_TracerStart", hitInfo);
                tracerSimple.tracerLineRenderer.SetPosition(0, fireSpawnpoint.transform.position);
                tracerSimple.tracerLineRenderer.SetPosition(1, hitInfo);
                tracerSimple.tracerLineRenderer.enabled = true; //Activated tracerLine
            }
        }
        catch
        {

        }
    }

    public IEnumerator C_TracerStart(Vector3 hitInfo)
    {
        yield return new WaitForSeconds(resetLifetime); //For tracer is not dissapear much fast
        tracerSimple.tracerLineRenderer.SetPosition(0, fireSpawnpoint.transform.position);
        tracerSimple.tracerLineRenderer.enabled = false; //Deactivated tracerLine
    }

    #endregion
}