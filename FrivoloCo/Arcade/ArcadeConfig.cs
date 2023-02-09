namespace FrivoloCo.Arcade;

public class ArcadeConfig
{
    public MachineData machineData { get; set; }
    public CoinOption coinOption { get; set; }
}

public class MachineData
{
    public string region { get; set; }
    public string model { get; set; }
    public string revision { get; set; }
    public string ver { get; set; }
}

public class CoinOption
{
    public int coins { get; set; }
    public int credits { get; set; }
}