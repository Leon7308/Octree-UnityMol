using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{
    private GameObject temp; // Temporary GameObject reference
    private GameObject atoms; // Reference to child GameObject that holds the actual objects we want in the octree
    public GameObject[] worldObjects; // Array of objects to be managed by the octree
    public List<GameObject> worldObjectsTemp = new List<GameObject>(); // Temporary list used for constructing the above array
    public int nodeMinSize = 5; // Minimum size for the octree nodes
    Octree otree; // Instance of our Octree
    bool flag = false; // Flag used to ensure the octree creation logic runs only once

    // Start is called before the first frame update
    void Start()
    {
        // Nothing is executed here in the provided code
    }

    void Update(){
        if(transform.childCount > 0 && !flag){
            temp = transform.GetChild(0).gameObject; // Get the first child of this GameObject
            atoms = temp.transform.GetChild(0).gameObject; // Then get the first child of the temp object (we assume this contains the actual objects)

            // Loop over all children of the atoms GameObject
            for(int i = 0; i < atoms.transform.childCount; i++){
                
                // If the current child object isn't in our temp list yet, add it
                if (!(worldObjectsTemp.Contains(atoms.transform.GetChild(i).gameObject)))
                {
                    worldObjectsTemp.Add(atoms.transform.GetChild(i).gameObject);
                }   
            }
            worldObjects = worldObjectsTemp.ToArray();  // Convert the list of GameObjects to an array
            otree = new Octree(worldObjects, nodeMinSize);  // Initialize the octree with our objects and minimum node size
            flag = true;  // Set the flag to true so the logic doesn't run again
        }
    }

    // This method is used for drawing the octree nodes in the Unity Editor for debugging purposes
    public void onDrawGizmos()
    {
        if(Application.isPlaying)  // If the game is currently running in the editor...
        {
            otree.rootNode.Draw();  // Draw the octree's nodes using Gizmos
        }   
    }
}