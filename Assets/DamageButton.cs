using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    [SerializeField]
    HealthBar playerHealthBar;
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TakeDamage);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TakeDamage()
    {
        int damage = Random.Range(10, 20);
        playerHealthBar.TakeDamage(damage);
    }
}
