using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Water.Graphics.Screens;
using Water.Utils;

namespace FrivoloCo.Arcade;

public class ArcadeShim
{
    private int credits = 0;
    private int coinsPerCredit = 1;
    private int coinsInserted = 0;
    
    private static ArcadeShim _instance;
    private static Object _threadLock = new Object();
    
    public static bool hasInstance { get { return _instance is null; } }
    private ArcadeShim()
    {
        
    }

    public static ArcadeShim GetInstance()
    {
        lock (_threadLock)
        {
            if (_instance == null)
            {
                _instance = new ArcadeShim();
            }
        }
        return _instance;
    }

    public int AcceptCoin()
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

    public int Credits { get { return credits; } }

    public int CoinsInserted
    { get { return coinsInserted; } }

    public int CoinsPerCredit
    { get { return coinsPerCredit; } }

    public async Task LoadArcadeAsync()
    {
        ArcadeConfig arcadeConfig = await JsonUtils.ReadAsync<ArcadeConfig>("arcadeSettings.json");

        coinsPerCredit = arcadeConfig.coinOption.coins;
    }

    public async Task WriteArcadeAsync()
    {
        throw new NotImplementedException();
    }

}