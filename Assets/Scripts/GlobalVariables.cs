public static class GlobalVariables
{
    public static int PlayerLayerIndex = 6;
    public static int BoxLayerIndex = 7;
    public static int GridLayerIndex = 9;

    public enum UnitType : byte
    {
        Empty,
        Box,
        Coin,
        Plug
    }

}