using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{

    [SerializeField] Transform Enemy;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider BackHpSlider;

    [SerializeField] float maxHp = 1000f;
    public float currentHp = 1000f;
    [SerializeField] bool BackHpHit = false;



    void Update()
    {
        transform.position = Enemy.position;
        hpSlider.value = Mathf.Lerp( hpSlider.value, currentHp / maxHp, Time.deltaTime * 5f);

        if (BackHpHit)
        {
            HpEffect();
        }
    }

    public void Dmg()
    {
        currentHp -= 300f;
        StartCoroutine(BackHpFun());
    }

    public IEnumerator BackHpFun()
    {
        yield return new WaitForSeconds(0.5f);
        BackHpHit = true;
    }

    public void HpEffect()
    {
        BackHpSlider.value = Mathf.Lerp(BackHpSlider.value, hpSlider.value, Time.deltaTime * 10f);
        if (hpSlider.value >= BackHpSlider.value - 0.01f)
        {
            BackHpHit = false;
            BackHpSlider.value = hpSlider.value;
        }
    }
}
