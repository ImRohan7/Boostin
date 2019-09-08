using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InvincibilityManager : MonoBehaviour
{
    public static InvincibilityManager Iinstance = null;

    [Header("Bitch Invincibility Count")]
    public int BCount;
    public int TCount;
    public int CCount;
    public int HCount;

    public int[] bitchArray;

    // Start is called before the first frame update
    void Start()
    {
        if (Iinstance == null)
        {
            Iinstance = this;
        }
        else
        {
            // Destroy(gameObject);
        }

        BCount = 0;
        TCount = 0;
        CCount = 0;
        HCount = 0;
        bitchArray = new int[] { BCount, TCount, CCount, HCount };
    }

    public int[] increaseBitchCount(ref int bTobeincreased)
    {
        bTobeincreased++;
        bitchArray = new int[] { BCount, TCount, CCount, HCount };

        return bitchArray;
    }

    public void PlayerInvincibility()
    {
        ResetInvincibility();
        int maxVal = InvincibilityManager.Iinstance.bitchArray.Max();
        int maxIndex = InvincibilityManager.Iinstance.bitchArray.ToList().IndexOf(maxVal);
        if(maxVal > 0)
        {
            GameObject.FindGameObjectWithTag(maxIndex.ToString()).GetComponent<PlayerManager>().isInvincible = true;
        }
    }

    private void ResetInvincibility()
    {
        //TO DO: Refactor eventually

        for (int i = 0; i < 4; i++)
        {
            GameObject.FindGameObjectWithTag(i.ToString()).GetComponent<PlayerManager>().isInvincible = false;
        }
    }

    private void Update()
    {
        //Only check when round is active
        PlayerInvincibility();
    }
}
