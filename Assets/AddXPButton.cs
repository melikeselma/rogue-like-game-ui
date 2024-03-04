using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddXPButton : MonoBehaviour
{
    [SerializeField]
    LevelHUD levelHUD;
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(AddXP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddXP()
    {
        int xp = Random.Range(10, 20);
        levelHUD.AddXp(xp);
    }
}
