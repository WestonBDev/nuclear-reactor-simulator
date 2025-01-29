using UnityEngine;
using Unity.Entities;
using TMPro;
using System.Threading;

public class NeutronText : MonoBehaviour
{
    public TMP_Text text;

    public float timer = 0f;
    public float cd = .5f;
    public FloatVariable neutrons;

    // Update is called once per frame
    void Update()
    {
        if(timer > cd)
        {

            text.text = neutrons.value.ToString();

            timer = 0f;
        }

        timer += Time.deltaTime;

    }
}
