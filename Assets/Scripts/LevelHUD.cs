using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUD : MonoBehaviour
{
    private int currentLevel;
    public TextMeshProUGUI LevelText;
    public int currentXP = 0;
    public int maksLevelXP = 0;
    public int katSayi = 2;
    public Image BackBarXP;
    public Image FrontBarXP;


    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow)) 
        {
            int randomXp = Random.Range(10, 20);
            AddXp(randomXp);
        }
        UpdateLevelUI();
    }

    public void UpLevel(int _level)
    {
        currentLevel += _level;
        ChangeTextUI();
    }


    public void DownLevel(int _level)
    {
        currentLevel -= _level;
        ChangeTextUI();
    }

    public int CalMaksLevel(int _level)
    {
        // int _maksLevelXP = (int)(Mathf.Log(_currentLevel) * katSayi) + 100;
        if(_level <= 0)
        {
            return 0;
        }
        
        int _maksLevelXP = _level * 100;
        return _maksLevelXP;
    }

    public void AddXp(int x)
    {
        currentXP += x;
        
        while (currentXP > CalMaksLevel(currentLevel))
        {
            UpLevel(1);
        }
    }

    public int GetRelativeXP(int _level)
    {
        return currentXP - CalMaksLevel(_level - 1);
    }

    public void UpdateLevelUI()
    {
        float hFractionXP = (float)GetRelativeXP(currentLevel) / ((float)CalMaksLevel(currentLevel) - (float)CalMaksLevel(currentLevel - 1));
        FrontBarXP.fillAmount = hFractionXP;
    }
    void ChangeTextUI()
    {
        LevelText.text = currentLevel.ToString();

    }
}
