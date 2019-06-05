using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public GameManager m_gameManager;
    public Text m_healthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_healthText.text = "Lives : " + m_gameManager.m_currLives;    
    }
}
