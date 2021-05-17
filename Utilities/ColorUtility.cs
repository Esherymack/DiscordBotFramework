using System;

namespace DiscordBotFramework.Utilities
{
    static class ColorUtil
    {
        public static uint RandomColorGenerate()
        {
            Random random = new Random();
            return (uint)random.Next(0x1000000);
        }
    }
}
