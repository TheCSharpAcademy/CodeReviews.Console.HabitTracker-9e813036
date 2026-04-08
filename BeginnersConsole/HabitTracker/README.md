## Challenges in Resource Management

Actually, during the integration of the Azure Speech SDK, one of my biggest takeaways was how to handle unmanaged resources. At the start, my app was prone to unexpected errors or it just "froze" after longer usage, because I didn't close the speech recognizer objects properly. As a mid-level C# developer, I honestly assumed that the built-in Garbage Collector (GC) would just take care of everything in the background and free up every bit of memory automatically, even because this study app is 'not difficult' :D ...

But I soon realized that the GC has its limits: while it’s great with managed memory, it cannot immediately and effectively close so-called unmanaged resources, like network connections or hardware (the microphone, for example). The solution finally came with the conscious implementation of the `IDisposable` interface in the `SpeechService` class. This allowed me to manually and safely release the `SpeechRecognizer` resources via the `Dispose` method as soon as they weren't needed anymore. This way, I could prevent memory leaks and ensure the long-term stability of the application.

## Challanges in Define layers

Even with 2.5 years of enterprise experience, I realized that I’ve mostly just inherited well-structured codebases where my only task was to follow existing patterns and well-separated layers. This gave me the false impression that I truly mastered Separation of Concerns.

Starting even a tiny practice project like this made me realize that finding the "edges" of different layers requires much more deliberate thinking when you're the one building the architecture from scratch. Stepping out of my daily routine—and not just feeding a massive context into an LLM—made me realize how much I still have to learn about where one layer ends and another begins.
