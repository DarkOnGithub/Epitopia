namespace Events.EventHandler
{
    public enum EventPriority
    {
        High = 1,
        Normal = 2,
        Low = 3
    }

    public static class EventPriorityHelper
    {
        public static EventPriority[] GetPriorities { get; } =
            (EventPriority[])System.Enum.GetValues(typeof(EventPriority));
    }
}