using UnityEngine;
using System.Collections.Generic;


//Second version of background change
public class Hashmap : MonoBehaviour
{
    //Breakdown of how the syntax works:
    //Dictionary -> this is the data structure that stores data in pairs. In our case the Key and value
    //<string, GameObject> -> This is the Key and value. The key will be storing a data type of string. While the value will store whatever the the key gives (in our case a prefab).
    //Example: key -> 'rock' and value -> The prefab that belongs to 'rock'
    //BG -> This is the name of the dictionary. This name will be used to add data later to the dictionary
    //new  Dictionary<string, Sprite>() -> this will make a new empty dictionary that will store a string and the object(value) that belongs to the key
    //() -> actually runs the building process 
    Dictionary<string, GameObject> BG = new  Dictionary<string, GameObject>();
    private GameObject background; //this will be used to call the current background being displayed in gamescene 
    public GameObject defaultBackground; //this is used in Start()

    //loads all the prefabs and the default background at the start of the game
    void Awake()
    {
        //this will load all the backgrounds into our Dictionary(BG)
        prefabBG();
        //This is storing the default background. So it will go into Resources and look for the prefab that name starts with default.
        //then it will save it in defaultBackground to use later in the code.
        defaultBackground = Resources.Load<GameObject>("Prefabs/default");
    }


    //function purpose: populates the dictionary with the key and value of each prefab located in 'Assets/Resources/Prefabs'
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


      //function purpose: This will select and apply the background based on the genre given by the API, but if the API doesn't return aything default background will display
    public void ChangeBackground(string genre)
    {
        //adding because I had a problem where the BG was begin checked ay to early when it was empty. This lets me know when exactly the BG starts loading the keys and value
        if (BG.Count == 0)
        {
            Debug.LogWarning("BG is empty, but prefabs are being loaded right now.");
            prefabBG();
        }
        //checking if the genre is return empty or unknown by the API
        if (string.IsNullOrEmpty(genre))
        {
            //if the genre is empty or unknown set the game scene as the defaultBackground (forest world)
            SetBackground(defaultBackground);
            Debug.Log("Genre was not received and is unknown or empty"); //just a debuger to let us know why the background didn't change
            return;
        }

        //converting the letters in genre as lowercase to match the 'key' names
        //example: if the genre gets return as 'Rock' it will convert to 'rock'
        genre = genre.ToLower();

        //looping through all the prefabs in our dictionary(BG)
        //what is 'prefab' -> all the existing prefabs that now have a key and a value in our dictionary
        //remember Key is the string -> in our case the genre name of the prefab (example: 'rock')
        //value is the prefab itself that belongs to the key
        foreach (var prefab in BG)
        {
            //if the genre given by the api matchs one of the 'prefab' key 
            if (genre.Contains(prefab.Key))
            {
                //then set then key's own value to the game scene.
                SetBackground(prefab.Value);
                Debug.Log("The genre matches the key name called: " + prefab.Key + "and set the background to its value"); //just a debuger to see the change worked in console
                return;

            }
    
        }
        //if the genre could not match any key at all, it will display the defaultBackground.
        SetBackground(defaultBackground);
        Debug.Log("The genre given by the API, did not match any of the keys in BG at all.");

    }

    //function purpose: its job it to destroy(remove) the current background and replace it with the next prefab's backgrounds 
    void SetBackground(GameObject prefab)
    {
        //checking if there already a existing background in the game scene
        if (background != null)
        {
            //we are destroying the old background at runtime (aka removing not actually destorying the original prefab)
            //just the copy the prefab creates in the gamescene
            Destroy(background);
        }
        //Then creating a new instant of the new prefab's background in the gamescene that replaces the old prefab's background
        background = Instantiate(prefab);
    }


}

