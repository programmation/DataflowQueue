# DataflowQueue
Implementing TPL Dataflow on iOS / Android with Xamarin

Code here is borrowed from TPL examples in the Microsoft TPL Dataflow documentation, and adapted for Mono/Xamarin and iOS/Android.

The major concession I've had to make is to implement the TransformBlock that uses Parallel.ForEach in the iOS platform-specific code since it's not available in a PCL for Mono. (Android implementation to follow).

In addition, you may need to track down and manually install / reference the System.Diagnostics.Tracing DLL that's internally referenced by TPL Dataflow. Unfortunately this DLL isn't part of the standard set installed by Xamarin.

Enjoy!
