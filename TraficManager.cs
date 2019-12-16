using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraficManager : MonoBehaviour
{
    public List<CarTraffic> cars;
    public Transform[] waypoints;
    [HideInInspector]
    public List<CarTraffic> awailableCars;
    public int minDuration;
    public int maxDuration;
    public float LineSpeed;

    private bool isLock = false;

    private void Start()
    {
        awailableCars = new List<CarTraffic>();
        awailableCars = cars;

        if(waypoints.Length== 0)
        {
            Debug.Log("Trafic manager not have wayypoints");
            return;
        }

        //Init cars parameters
        foreach (CarTraffic item in cars)
        {
            item.InitCar(LineSpeed, this, waypoints);
        }
    }

    private void Update()
    {
        if(!isLock)
        {
            isLock = true;
            StartCoroutine(Wait());
        }
    }
    
    IEnumerator Wait()
    {
        int timeDuration = Random.Range(minDuration, maxDuration);
        yield return new WaitForSeconds(timeDuration);
        ActivateCar();
        isLock = false;
    }

    void ActivateCar()
    {
        if (awailableCars.Count == 0)
            return;

        int randomIndex = Random.Range(0, awailableCars.Count);
        CarTraffic curCar = awailableCars[randomIndex];
        curCar.gameObject.SetActive(true);
        curCar.StartCar();
        
    }
}
