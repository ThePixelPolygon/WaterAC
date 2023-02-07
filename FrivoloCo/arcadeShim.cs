using System;
using Microsoft.Xna.Framework;
using Water.Graphics.Screens;

namespace FrivoloCo;

public class arcadeShim
{
    private int credits = 0;
    private int coinsPerCredit = 1;
    private int coinsInserted = 0;
    
    private static arcadeShim instance;
    private static Object threadLock = new Object();

    private arcadeShim() { }

    public static arcadeShim getInstance()
    {
        lock (threadLock)
        {
            if (instance == null)
            {
                instance = new arcadeShim();
            }
        }
        return instance;
    }

    public int acceptCoin()
    {
        coinsInserted++;
        if (coinsInserted == coinsPerCredit)
        {
            credits++;
            coinsInserted = 0;
            return 0;
        }
        
        return 1;
    }

    public int Credits
    {
        get { return credits; }
    }

    public int CoinsInserted
    {
        get { return coinsInserted;  }
    }

    public int CoinsPerCredit
    {
        get { return coinsPerCredit; }
    }

}