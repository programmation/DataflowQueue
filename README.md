# DataflowQueue
Implementing TPL Dataflow on iOS / Android with Xamarin

Code here is borrowed from TPL examples in the Microsoft TPL Dataflow documentation, and adapted for Mono/Xamarin and iOS/Android.

The major concession I've had to make is to implement the TransformBlock that uses Parallel.ForEach in the iOS platform-specific code since it's not available in a PCL for Mono. (Android implementation to follow).

In addition, you may need to track down and manually install / reference the System.Diagnostics.Tracing DLL that's internally referenced by TPL Dataflow. Unfortunately this DLL isn't part of the standard set installed by Xamarin.

Here's a sample output run (cleaned up a bit) from the iOS simulator on my Mac:

	Thread started:  #2
	Thread started:  #3
	Thread started: <Thread Pool> #4
	Thread started: <Thread Pool> #5
	2015-05-08 13:53:38.779 DataflowQueue.iOS[2552:44968] Downloading http://www.gutenberg.org/files/6130/6130-0.txt
	Thread started: <Thread Pool> #6
	Thread started: <Thread Pool> #7
	Thread started: <Thread Pool> #8
	Thread started: <Thread Pool> #9
	2015-05-08 13:53:43.610 DataflowQueue.iOS[2552:44982] Downloading http://www.gutenberg.org/cache/epub/1727/pg1727.txt
	2015-05-08 13:53:43.612 DataflowQueue.iOS[2552:44982] Creating word list...
	2015-05-08 13:53:43.718 DataflowQueue.iOS[2552:44968] Filtering word list...
	2015-05-08 13:53:48.579 DataflowQueue.iOS[2552:45036] Downloading http://www.gutenberg.org/cache/epub/1635/pg1635.txt
	2015-05-08 13:53:48.580 DataflowQueue.iOS[2552:44982] Creating word list...
	2015-05-08 13:53:49.117 DataflowQueue.iOS[2552:44968] Found 14493 words
	2015-05-08 13:53:49.118 DataflowQueue.iOS[2552:44968] Filtering word list...
	2015-05-08 13:53:49.120 DataflowQueue.iOS[2552:45036] Checking for reversible words...
	Thread started: <Thread Pool> #10
	2015-05-08 13:53:49.455 DataflowQueue.iOS[2552:45063] Found reversed word dog / god
	2015-05-08 13:53:49.456 DataflowQueue.iOS[2552:45063] Found reversed word aera / area
	2015-05-08 13:53:49.456 DataflowQueue.iOS[2552:45063] Found reversed word doom / mood
	2015-05-08 13:53:49.457 DataflowQueue.iOS[2552:45063] Found reversed word led / del
	2015-05-08 13:53:49.457 DataflowQueue.iOS[2552:45063] Found reversed word draw / ward
	2015-05-08 13:53:49.458 DataflowQueue.iOS[2552:45063] Found reversed word area / aera
	2015-05-08 13:53:49.458 DataflowQueue.iOS[2552:45063] Found reversed word evil / live
	2015-05-08 13:53:49.459 DataflowQueue.iOS[2552:45063] Found reversed word mid / dim
	2015-05-08 13:53:49.460 DataflowQueue.iOS[2552:45063] Found reversed word mood / doom
	2015-05-08 13:53:49.460 DataflowQueue.iOS[2552:45063] Found reversed word moor / room
	2015-05-08 13:53:49.461 DataflowQueue.iOS[2552:45063] Found reversed word net / ten
	2015-05-08 13:53:49.461 DataflowQueue.iOS[2552:45063] Found reversed word not / ton
	2015-05-08 13:53:49.462 DataflowQueue.iOS[2552:45063] Found reversed word now / won
	2015-05-08 13:53:49.462 DataflowQueue.iOS[2552:45063] Found reversed word god / dog
	2015-05-08 13:53:49.463 DataflowQueue.iOS[2552:45063] Found reversed word port / trop
	2015-05-08 13:53:49.463 DataflowQueue.iOS[2552:45063] Found reversed word reed / deer
	2015-05-08 13:53:49.464 DataflowQueue.iOS[2552:45063] Found reversed word deeps / speed
	2015-05-08 13:53:49.464 DataflowQueue.iOS[2552:45063] Found reversed word deer / reed
	2015-05-08 13:53:49.465 DataflowQueue.iOS[2552:45063] Found reversed word del / led
	2015-05-08 13:53:49.466 DataflowQueue.iOS[2552:45063] Found reversed word dew / wed
	2015-05-08 13:53:49.466 DataflowQueue.iOS[2552:45063] Found reversed word dim / mid
	2015-05-08 13:53:49.467 DataflowQueue.iOS[2552:45063] Found reversed word keels / sleek
	2015-05-08 13:53:49.467 DataflowQueue.iOS[2552:45063] Found reversed word room / moor
	2015-05-08 13:53:49.468 DataflowQueue.iOS[2552:45063] Found reversed word saw / was
	2015-05-08 13:53:49.468 DataflowQueue.iOS[2552:45063] Found reversed word seat / taes
	2015-05-08 13:53:49.469 DataflowQueue.iOS[2552:45063] Found reversed word sleek / keels
	2015-05-08 13:53:49.469 DataflowQueue.iOS[2552:45063] Found reversed word speed / deeps
	2015-05-08 13:53:49.470 DataflowQueue.iOS[2552:45063] Found reversed word spot / tops
	2015-05-08 13:53:49.470 DataflowQueue.iOS[2552:45063] Found reversed word spots / stops
	2015-05-08 13:53:49.471 DataflowQueue.iOS[2552:45063] Found reversed word stops / spots
	2015-05-08 13:53:49.471 DataflowQueue.iOS[2552:45063] Found reversed word taes / seat
	2015-05-08 13:53:49.472 DataflowQueue.iOS[2552:45063] Found reversed word ten / net
	2015-05-08 13:53:49.472 DataflowQueue.iOS[2552:45063] Found reversed word tis / sit
	2015-05-08 13:53:49.473 DataflowQueue.iOS[2552:45063] Found reversed word ton / not
	2015-05-08 13:53:49.473 DataflowQueue.iOS[2552:45063] Found reversed word tops / spot
	2015-05-08 13:53:49.474 DataflowQueue.iOS[2552:45063] Found reversed word trop / port
	2015-05-08 13:53:49.474 DataflowQueue.iOS[2552:45063] Found reversed word ward / draw
	2015-05-08 13:53:49.475 DataflowQueue.iOS[2552:45063] Found reversed word was / saw
	2015-05-08 13:53:49.475 DataflowQueue.iOS[2552:45063] Found reversed word wed / dew
	2015-05-08 13:53:49.476 DataflowQueue.iOS[2552:45063] Found reversed word wolf / flow
	2015-05-08 13:53:49.477 DataflowQueue.iOS[2552:45063] Found reversed word won / now
	2015-05-08 13:53:49.938 DataflowQueue.iOS[2552:45063] Creating word list...
	2015-05-08 13:53:52.415 DataflowQueue.iOS[2552:44968] Found 8084 words
	2015-05-08 13:53:52.415 DataflowQueue.iOS[2552:44968] Filtering word list...
	2015-05-08 13:53:52.416 DataflowQueue.iOS[2552:45063] Checking for reversible words...
	2015-05-08 13:53:52.564 DataflowQueue.iOS[2552:45036] Found reversed word draw / ward
	2015-05-08 13:53:52.565 DataflowQueue.iOS[2552:45036] Found reversed word live / evil
	2015-05-08 13:53:52.566 DataflowQueue.iOS[2552:45036] Found reversed word lived / devil
	2015-05-08 13:53:52.566 DataflowQueue.iOS[2552:45036] Found reversed word edit / tide
	2015-05-08 13:53:52.567 DataflowQueue.iOS[2552:45036] Found reversed word mad / dam
	2015-05-08 13:53:52.567 DataflowQueue.iOS[2552:45036] Found reversed word evil / live
	2015-05-08 13:53:52.568 DataflowQueue.iOS[2552:45036] Found reversed word mid / dim
	2015-05-08 13:53:52.568 DataflowQueue.iOS[2552:45036] Found reversed word net / ten
	2015-05-08 13:53:52.569 DataflowQueue.iOS[2552:45036] Found reversed word nod / don
	2015-05-08 13:53:52.569 DataflowQueue.iOS[2552:45036] Found reversed word now / won
	2015-05-08 13:53:52.570 DataflowQueue.iOS[2552:45036] Found reversed word god / dog
	2015-05-08 13:53:52.570 DataflowQueue.iOS[2552:45036] Found reversed word part / trap
	2015-05-08 13:53:52.571 DataflowQueue.iOS[2552:45036] Found reversed word parts / strap
	2015-05-08 13:53:52.571 DataflowQueue.iOS[2552:45036] Found reversed word gut / tug
	2015-05-08 13:53:52.572 DataflowQueue.iOS[2552:45036] Found reversed word pot / top
	2015-05-08 13:53:52.572 DataflowQueue.iOS[2552:45036] Found reversed word dam / mad
	2015-05-08 13:53:52.573 DataflowQueue.iOS[2552:45036] Found reversed word del / led
	2015-05-08 13:53:52.574 DataflowQueue.iOS[2552:45036] Found reversed word deliver / reviled
	2015-05-08 13:53:52.574 DataflowQueue.iOS[2552:45036] Found reversed word devil / lived
	2015-05-08 13:53:52.575 DataflowQueue.iOS[2552:45036] Found reversed word dim / mid
	2015-05-08 13:53:52.576 DataflowQueue.iOS[2552:45036] Found reversed word raw / war
	2015-05-08 13:53:52.576 DataflowQueue.iOS[2552:45036] Found reversed word dog / god
	2015-05-08 13:53:52.577 DataflowQueue.iOS[2552:45036] Found reversed word don / nod
	2015-05-08 13:53:52.577 DataflowQueue.iOS[2552:45036] Found reversed word led / del
	2015-05-08 13:53:52.578 DataflowQueue.iOS[2552:45036] Found reversed word reviled / deliver
	2015-05-08 13:53:52.578 DataflowQueue.iOS[2552:45036] Found reversed word saw / was
	2015-05-08 13:53:52.579 DataflowQueue.iOS[2552:45036] Found reversed word spot / tops
	2015-05-08 13:53:52.579 DataflowQueue.iOS[2552:45036] Found reversed word strap / parts
	2015-05-08 13:53:52.580 DataflowQueue.iOS[2552:45036] Found reversed word ten / net
	2015-05-08 13:53:52.581 DataflowQueue.iOS[2552:45036] Found reversed word tide / edit
	2015-05-08 13:53:52.581 DataflowQueue.iOS[2552:45036] Found reversed word tops / spot
	2015-05-08 13:53:52.582 DataflowQueue.iOS[2552:45036] Found reversed word top / pot
	2015-05-08 13:53:52.582 DataflowQueue.iOS[2552:45036] Found reversed word trap / part
	2015-05-08 13:53:52.583 DataflowQueue.iOS[2552:45036] Found reversed word tug / gut
	2015-05-08 13:53:52.583 DataflowQueue.iOS[2552:45036] Found reversed word war / raw
	2015-05-08 13:53:52.584 DataflowQueue.iOS[2552:45036] Found reversed word ward / draw
	2015-05-08 13:53:52.584 DataflowQueue.iOS[2552:45036] Found reversed word was / saw
	2015-05-08 13:53:52.585 DataflowQueue.iOS[2552:45036] Found reversed word won / now
	2015-05-08 13:53:52.684 DataflowQueue.iOS[2552:44968] Found 1821 words
	2015-05-08 13:53:52.685 DataflowQueue.iOS[2552:44982] Checking for reversible words...
	2015-05-08 13:53:52.706 DataflowQueue.iOS[2552:44982] Found reversed word live / evil
	2015-05-08 13:53:52.706 DataflowQueue.iOS[2552:44982] Found reversed word evil / live
