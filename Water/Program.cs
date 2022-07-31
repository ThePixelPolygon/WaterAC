using System;

namespace Water
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new WaterGame())
                game.Run();
        }
    }
}
