using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitchManager : MonoBehaviour
{
    [Header("Number of times each bitch has won")]
    public int BWins;
    public int TWins;
    public int CWins;
    public int HWins;

    public int[] bitchArray;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        BWins = 0;
        TWins = 0;
        CWins = 0;
        HWins = 0;

        bitchArray = new int[] { BWins, TWins, CWins, HWins };
    }

    public int[] increaseBitchWin(ref int bitchWhichWon)
    {
        bitchWhichWon++;
        bitchArray = new int[] { BWins, TWins, CWins, HWins };

        return bitchArray;
    }
}
