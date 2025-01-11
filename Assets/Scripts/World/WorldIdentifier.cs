namespace World
{
    public enum WorldIdentifier
    {
        Overworld = 0
    }

    public static class WorldIdentifierUtils
    {
        public static string GetWorldName(this WorldIdentifier identifier)
        {
            return identifier switch
                   {
                       WorldIdentifier.Overworld => "Overworld",
                       _ => "Unknown"
                   };
        }
    }
}