using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPooling : MonoBehaviour
{
    private static ArrowPooling instance;
    public static ArrowPooling Instance { get { return instance; } }

    [SerializeField] GameObject arrowPrefab = null;
    int maxArrows = 10;

    List<GameObject> arrowsList;

    private void Awake()
    {
        instance = this;

        arrowsList = new List<GameObject>(maxArrows);

        for (int i = 0; i < maxArrows; i++)
        {
            GameObject arrowInstance = Instantiate(arrowPrefab);
            arrowInstance.transform.SetParent(transform);
            arrowInstance.SetActive(false);

            arrowsList.Add(arrowInstance); 
        }
    }

    public GameObject GetArrow()
    {
        foreach(GameObject arrow in arrowsList)
        {
            if(!arrow.activeInHierarchy)
            {
                arrow.SetActive(true);
                return arrow;
            }
        }

        GameObject arrowInstance = Instantiate(arrowPrefab);
        arrowInstance.transform.SetParent(transform);
        arrowsList.Add(arrowInstance);
        arrowInstance.SetActive(true);
        return arrowInstance;
    }

}
