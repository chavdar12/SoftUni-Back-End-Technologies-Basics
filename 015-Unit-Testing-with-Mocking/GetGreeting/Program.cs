using GetGreeting;

Console.WriteLine(new GreetingProvider(new SystemTimeProvider()).GetGreeting());
