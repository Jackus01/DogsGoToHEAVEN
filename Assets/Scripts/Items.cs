using NUnit.Framework.Internal;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Items : MonoBehaviour
{
    [SerializeField] private GameObject itemFactory;
    public GameObject[] itemspawns;
    public List<GameObject> itemPrefabs;

    public void SpawnRandomItem(Vector3 position)
    {
            int randomitem = Random.Range(0, itemPrefabs.Count);
            GameObject item = Instantiate(itemPrefabs[randomitem], position, Quaternion.identity);
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       foreach (GameObject itemspawn in itemspawns)
       {
            SpawnRandomItem(itemspawn.transform.position);

       } 
    }

    // Update is called once per frame
    void Update()
    {
        



    }
}
