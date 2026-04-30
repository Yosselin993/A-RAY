using UnityEngine;
using System.Collections.Generic;


//Second version of background change
public class Hashmap : MonoBehaviour
{
    //Breakdown of how the syntax works:
    //Dictionary -> this is the data structure that stores data in pairs. In our case the Key and value
    //<string, GameObject> -> This is the Key and value. The key will be storing a data type of string. And value is what you get back from that string.
    //Example: key -> 'rock' and value -> The prefab that belongs to 'rock'
    //BG -> This is the name of the dictionary. This name will be used to add data later to the dictionary
    //new  Dictionary<string, Sprite>() -> this will make a new empty dictionary that will store a string and the object(value) that belongs to the key
    //() -> actually runs the building process 
    Dictionary<string, GameObject> BG = new  Dictionary<string, GameObject>();
    private GameObject background; //this will be used to call the current background being displayed in gamescene 


    void start()
    {
        prefabBG();
    }

    void prefabBG()
    {
        //This holds all the prefabs located in 'Assets/Resources/Prefabs'
        //All the prefabs will be stored as a GameObject inside the array "allprefabs"
        //What is "allprefabs" -> all the different prefabs we created.
        //The folder Resources had to be created becuase it acts as a special folder to Unity.
        GameObject [] allprefabs = Resources.LoadAll<GameObject>("Prefabs");

        //loop through all the prefabs in the array, one by one.
        foreach (GameObject Oneprefab in allprefabs)
        {
            //get the prefab name and convert all the letters to lowercase
            //I also did this to 'genre' so everything is the same and easier to compare
            //We'll need to change all prefab names to match their genre name's 
            //(or I can remove the ToLower and we manually just type the name as lowercase)
            string key = Oneprefab.name.ToLower();
            
            //if the prefab(oneprefab) name is not stored in the dictionary yet then store it
            //So it goes through all the prefabs in allprefabs and creates a reference(key) to the value.
            //key(reference) -> is the prefab's name (example: 'rock')
            //value -> is the Oneprefab (the prefab itself ) (AKA the GameObject)
            if (!BG.ContainsKey(key))
            {
                // this is not creating a new prefab. We are making a reference name to all the existing prefabs.
                //Example: if the existing prefab being iterated(Oneprefab) is called 'rock' and its not stored in Dictionary. It will take that name as the key. Then That key name becomes the reference to the value(AKA the prefab itself)
               BG.Add(key, Oneprefab); 
               Debug.Log("Adding prefab key name:" + key); //Just a debuger
            }


        }
    }


        //still building... 
    public void ChangeBackground(string genre)
    {
        if (string.IsNullOrEmpty(genre))
        {
            Debug.Log("Genre was not received and is unknown");
            return;
        }


        
    }

   


}

