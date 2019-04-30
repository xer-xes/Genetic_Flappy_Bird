using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject Bird_Prefab;
    public GameObject Starting_Position;
    public int Population_Size = 50;
    List<GameObject> Population = new List<GameObject>();
    public static float Elapsed = 0;
    public float Timer = 5;
    int Generation = 1;

    GUIStyle GUIStyle = new GUIStyle();
    private void OnGUI()
    {
        GUIStyle.fontSize = 25;
        GUIStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", GUIStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Generation" + Generation, GUIStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}",Elapsed), GUIStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population" + Population.Count, GUIStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        for (int i = 0; i < Population_Size; i++)
        {
            GameObject Bird = Instantiate(Bird_Prefab, Starting_Position.transform.position, transform.rotation);
            Bird.GetComponent<Brain>().Init();
            Population.Add(Bird);
        }
        Time.timeScale = 5;
    }

    GameObject Breed(GameObject Parent1,GameObject Parent2)
    {
        GameObject offSpring = Instantiate(Bird_Prefab, Starting_Position.transform.position, transform.rotation);
        Brain brain = offSpring.GetComponent<Brain>();
        if(Random.Range(0,100) == 1)//Mutate 1 in 100
        {
            brain.Init();
            brain.DNA.Mutate();
        }
        else
        {
            brain.Init();
            brain.DNA.Combine(Parent1.GetComponent<Brain>().DNA, Parent2.GetComponent<Brain>().DNA);
        }
        return offSpring;
    }

    void BreedPopulation()
    {
        List<GameObject> sortedList = Population.OrderBy(o => (o.GetComponent<Brain>().Distance_Travelled)).ToList();
        Population.Clear();
        for (int i = (3 * sortedList.Count/4) - 1; i < sortedList.Count-1; i++)
        {
            Population.Add(Breed(sortedList[i], sortedList[i + 1]));
            Population.Add(Breed(sortedList[i + 1], sortedList[i]));
            Population.Add(Breed(sortedList[i], sortedList[i + 1]));
            Population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        Generation++;
    }

    private void Update()
    {
        Elapsed += Time.deltaTime;
        if(Elapsed >= Timer)
        {
            BreedPopulation();
            Elapsed = 0;
        }
    }
}
