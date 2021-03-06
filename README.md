## Repository for threading talk I gave as part of a lunch-and-learn at IntelliTect.

## Outline
- Introduction
- Thread
  - What is a thread? - An abstraction over CPU execution
  - What is the difference between a thread and a process?
  - Threads exist within a process and share address space (AppDomain), processes do not. This allows threads to act on the same memory (objects). 
    - Threads are much more lightweight than processes.
    - Threads exist only within a process.
  - Do threads make your program faster? - It depends. On a single core machine no, on multi-core machines it can.
  - How many threads should you use in your application? In you want the maximum performance, one thread per CPU.
- How do you put work on a different thread?
  - new Thread().Start()
  - System.ComponentModel.BackgroundWorker - Runs code on a background thread, and raises events on the UI thread. Primarily used for updating progress on the UI thread.
  - System.Threading.ThreadPool.QueueUserWorkItem() 
  - Task.Factory.StartNew/Task.Run - These are the prefered method. We won't be covering TPL directly. There is lots of good information on TPL, async/await, Dispatchers, etc available (see chapter 18 of Mark’s book). 
  - How expensive is it to create a thread? - About 1 MB
- When would you want to create your own thread?
  - Setting a thread’s priority or any of the thread specific properties like identity.
  - You need a foreground thread, or setting single-threaded apartment (STA). All thread pool threads are multithreaded apartment (MTA). 
  - Your app causes threads to block for long periods of time. You don’t want to be blocking thread pool threads. Consider starting a task with TaskCreationOptions.LongRunning.
- What does it mean for something to be “thread safe”? - Code that behaves in a consistent manner when accessed from multiple threads. The key is knowing what is the “expected” behavior.
- What things are thread safe? 
  - Code that is protected by some synchronization (lock or similar)
  - Immutable data structures. They can’t be mutated (changed) so by definition they are thread safe.
  - Local variables. Each thread has its own stack, so these variable only exist on the current thread.
  - Static constructors. They are guaranteed to only be executed once, hence only a single thread will ever invoke it.
  - Readonly fields (mostly). These are effectively immutable; however it is possible to get weird states if a constructor passes ‘this’ off to another method before initializing the field.
  - Just because an object is “thread safe” does not mean that your access to it is thread safe. It means that it will behave in a consistent manner if multiple threads act on it. 
- Threading and architecture. In most cases, you can achieve best results by architecting around immutable data structures which are thread safe by definition. For the remainder of the talk we are going to assume that synchronisation is required. 
- Mutex. What is a mutex? - From the term mutually exclusive. It is a synchronization mechanism that ensure that only a single thread can acquire it. Think of it like a talking stick. Only the person holding the stick is allowed to talk.
  - In C# this is the System.Thread.Mutex class.
  - Primary use case of interprocess synchronisation - so not horribly useful in many cases.
- What is a lock? It is a language keyword around the System.Threading.Monitor class. In most cases it is more efficient than a mutex.
  - Behaves very similar to a mutex, in that it is mutually exclusive.
  - It is associated with an object reference, this means that it is limited to the memory space of the object (its AppDomain).
  - It is lightweight, and fast (reletively).
  - This is by far the easiest way to synchronize access.
  - Should only ever lock on reference types.
- Semaphore - what is a semaphore? It is a counter that can be used to control access to a resource. 
  - Like mutexes you can have named semaphore for doing IPC. 
  - SemephoreSlim
  - Can be used for an async lock. Watch out for disposing of a Task.
- WaitHandles - These comes in two flavors ManualResetEvent and AutoResetEvent. Continuing with the people getting food example, these act like doors that people have to walk through. When they are in an non-signaled state the door is closed. When they are in a signaled state the door is open.
  - APIs. Set, WaitOne, WaitHandle.WaitAll/Any
  - AutoResetEvent 
    - When in a signaled state lets a single thread through then returns to a non-signaled state. 
    - WARNING: Each call to Set is not guaranteed to release a thread. If two calls to Set occur rapidly, only a single thread will be released. 
  - ManualResetEvent
    - Very similar to AutoResetEvent except you must manually call Reset to move it from a signaled to a non-signaled state.
  - Common mistake is forgetting to dispose of wait handles when they are no longer needed.
- Thread safe collections
  - System.Collections.Concurrent nuget package
  - Concurrent* (Dictionary, Queue, Bag, Stack). 
  - These collections behave in a consistent manner, but that does not mean that your usage of them is thread safe.
  - Why is there no ConcurrentList? - It is not reasonable to implement a lock-free IList implementation. Things like Insert, and RemoveAt make it very difficult. If you do not need ordering of items use ConcurrentBag, or simply use a monitor to lock access. 
  - BlockingCollection - producer consumer pattern.
  - Perf measurements. http://download.microsoft.com/download/B/C/F/BCFD4868-1354-45E3-B71B-B851CD78733D/PerformanceCharacteristicsOfThreadSafeCollection.pdf
- Atomic operations - operation that cannot be interrupted
  - Reads and writes or 32 bit variables - bool, char, byte, short, int, float (sbyte, ushort, and uint), and all reference types since a pointer is 32 bits.
  - Language spec 5.5
  - Just using atomic operations does not guarantee your code is thread safe (consider the case where you have multiple operations that rely on each other).
  - Interlocked operations.
  - Optimistic concurrency pattern. The idea that most of the time multiple threads will not collide. This is an ideal situation for Interlocked.
- Volatile
  - Your code can be reordered by the compiler, run-time, or CPU. 
  - A volatile read has “acquire semantics”; that is, it is guaranteed to occur prior to any references to memory that occur after it in the instruction sequence.
  - A volatile write has “release semantics”; that is, it is guaranteed to happen after any memory references prior to the write instruction in the instruction sequence.
  - Volatile fields must be 32 bit types.
  - System.Threading.Volatile.Read/Write. Allows for reading and writing to values in a very similar manner

