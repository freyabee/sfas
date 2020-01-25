using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // --------------------------------------------------------------

    [SerializeField]
    Text m_BulletText;

    [SerializeField]
    Text m_GrenadeText;

    // --------------------------------------------------------------

    public void SetAmmoText(int bulletCount, int grenadeCount)
    {
        if(m_BulletText)
        {
            m_BulletText.text = "Test 1";
        }
        
        if(m_GrenadeText)
        {
            m_GrenadeText.text = "Test 2";
        }
    }
}
