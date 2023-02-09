using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Water.Graphics.Screens;
using Water.Utils;

namespace FrivoloCo.Arcade;

public class ArcadeShim
{
    public ArcadeConfig ArcadeConfig;
    private int credits = 0;
    private int coinsInserted = 0;
    
    private static ArcadeShim _instance;
    private static Object _threadLock = new Object();
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
        if (coinsInserted == ArcadeConfig.coinOption.coins)
        {
            credits += ArcadeConfig.coinOption.credits;
            coinsInserted = 0;
            return 0;
        }
        
        return 1;
    }

    public void TakeCredit()
    {
        credits -= credits > 0 ? 1 : 0;
    }

    public int Credits { get { return credits; } }

    public int CoinsInserted
    { get { return coinsInserted; } }

    public async Task LoadArcadeAsync()
    {
        ArcadeConfig = await JsonUtils.ReadAsync<ArcadeConfig>("arcadeSettings.json");
    }

    public async Task WriteArcadeAsync()
    {
        throw new NotImplementedException();
    }

}