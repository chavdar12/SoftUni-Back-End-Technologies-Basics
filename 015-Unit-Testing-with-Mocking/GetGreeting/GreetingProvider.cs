namespace GetGreeting;

public class GreetingProvider(ITimeProvider timeProvider)
{
    public string GetGreeting()
    {
        var hour = timeProvider.GetCurrentTime().Hour;
        return hour switch
        {
            >= 5 and < 12 => "Good morning!",
            >= 12 and < 18 => "Good afternoon!",
            >= 18 and < 22 => "Good evening!",
            _ => "Good night!",
        };
    }
}
