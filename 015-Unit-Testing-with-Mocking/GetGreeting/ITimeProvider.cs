namespace GetGreeting;

public interface ITimeProvider
{
    DateTime GetCurrentTime();
}

public class SystemTimeProvider : ITimeProvider
{
    public DateTime GetCurrentTime() => DateTime.Now;
}
