using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndScreenScript : MonoBehaviour
{
    public GameObject playerEndPrefab;
    private List<Manette> manettes=new List<Manette>();
    
    // Start is called before the first frame update
    void Start()
    {
        manettes = new List<Manette>();

        foreach (var manette in PlayerInputs.pControllers)
        {
            if (manette==null||manette.character==null||manette.character=="")
            {
               
            }
            else
            {
                manettes.Add(manette);
            }
        }
        
        Debug.Log("Manettes"+ manettes.Count+ " sd " + manettes);
        
        //DEBUG
        if (false)
        {
            manettes = new List<Manette>();
            float baseX = GetComponent<Canvas>().renderingDisplaySize.x;
        
            //Debug
            manettes.Add(new Manette(Gamepad.current));
            manettes.Add(new Manette(Gamepad.current));
            manettes.Add(new Manette(Gamepad.current));
            manettes.Add(new Manette(Gamepad.current));
            manettes[0].Winner = true;
            foreach (var manette in manettes)
            {
                manette.character = GameManager.characters[Random.Range(0,2)].name;
            }
            manettes[0].character = GameManager.characters[0].name;
        }
        
        float yHeight=0;
        float xInterval = 300;
        float xStart = -(xInterval * (manettes.Count - 1))/2;

        for (int i = 0; i < manettes.Count; i++)
        {
            GameObject newPrefab = Instantiate(playerEndPrefab, transform);
            newPrefab.transform.localPosition = new Vector3(xStart + (i * xInterval), yHeight, transform.position.z);

            Sprite s = null;
            foreach (var character in GameManager.characters)
            {
                if (character.name == manettes[i].character)
                {
                    s = character.spr;
                    break;
                }
            }
            newPrefab.transform.Find("CharImg").GetComponent<Image>().sprite = s;
            TextMeshProUGUI text = newPrefab.transform.Find("TopText").GetComponent<TextMeshProUGUI>();
            if (manettes[i].Winner)
            {
                text.text = "Winner";
                text.color = Color.green;
            }
            else
            {
                text.text = "Loser";
                text.color = Color.red;
            }
        }
    }
    public void PlayAgain()
    {
        PlayerInputs.ResetManettes();
        SceneChanger.ChangeScene(SceneTypes.MainMenu);
    }
    
}
