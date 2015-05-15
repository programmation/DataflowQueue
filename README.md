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
	Thread started: <Thread Pool> #6
	2015-05-15 16:27:40.426 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:40.2583300Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/files/6130/6130-0.txt
	Thread started: <Thread Pool> #7
	Thread started: <Thread Pool> #8
	Thread started: <Thread Pool> #9
	2015-05-15 16:27:46.459 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:46.2343900Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/cache/epub/1727/pg1727.txt
	2015-05-15 16:27:46.459 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:46.2365490Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:27:46.459 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:46.3346280Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	2015-05-15 16:27:49.098 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:49.0473340Z][DBUG00011][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/cache/epub/1635/pg1635.txt
	2015-05-15 16:27:49.099 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:49.0473400Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:27:50.370 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:50.3699470Z][DBUG00011][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:27:51.954 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:51.9539550Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 14493 words
	2015-05-15 16:27:51.954 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:51.9539820Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	2015-05-15 16:27:51.955 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:51.9556120Z][DBUG00008][DataflowQueue.iOS.AppDelegate.FinishedLaunching L42]: Checking for reversible words...
	Thread started: <Thread Pool> #10
	2015-05-15 16:27:52.289 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888780Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word aera / area
	2015-05-15 16:27:52.289 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888870Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dog / god
	2015-05-15 16:27:52.289 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888890Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word doom / mood
	2015-05-15 16:27:52.289 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888920Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word draw / ward
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888940Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word led / del
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2888970Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word area / aera
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889000Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mid / dim
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889020Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mood / doom
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889040Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word moor / room
	2015-05-15 16:27:52.290 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889060Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889080Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word net / ten
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889110Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word not / ton
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889130Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word now / won
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889150Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word god / dog
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889170Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word port / trop
	2015-05-15 16:27:52.291 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889200Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deeps / speed
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889220Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deer / reed
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889240Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word del / led
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889260Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word reed / deer
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889290Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dew / wed
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889310Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dim / mid
	2015-05-15 16:27:52.292 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2889330Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word room / moor
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892770Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word keels / sleek
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892780Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word saw / was
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892810Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word seat / taes
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892820Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word sleek / keels
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892830Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word speed / deeps
	2015-05-15 16:27:52.293 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892840Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spot / tops
	2015-05-15 16:27:52.294 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892860Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spots / stops
	2015-05-15 16:27:52.294 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892870Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word stops / spots
	2015-05-15 16:27:52.294 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892880Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word taes / seat
	2015-05-15 16:27:52.294 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892900Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ten / net
	2015-05-15 16:27:52.294 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892910Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tis / sit
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892960Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ton / not
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892970Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tops / spot
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2892980Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word trop / port
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2893000Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ward / draw
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2893010Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word was / saw
	2015-05-15 16:27:52.295 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2893030Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word wed / dew
	2015-05-15 16:27:52.296 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2893040Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word won / now
	2015-05-15 16:27:52.296 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:52.2893050Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word wolf / flow
	2015-05-15 16:27:55.362 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.3628810Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 8084 words
	2015-05-15 16:27:55.363 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.3629040Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	2015-05-15 16:27:55.363 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.3629580Z][DBUG00012][DataflowQueue.iOS.AppDelegate.FinishedLaunching L42]: Checking for reversible words...
	2015-05-15 16:27:55.510 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5107930Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word draw / ward
	2015-05-15 16:27:55.511 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5107990Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word live / evil
	2015-05-15 16:27:55.511 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108000Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word lived / devil
	2015-05-15 16:27:55.511 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108020Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word edit / tide
	2015-05-15 16:27:55.511 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108040Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mad / dam
	2015-05-15 16:27:55.511 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108060Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108090Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mid / dim
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108110Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word net / ten
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108130Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word nod / don
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108160Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word now / won
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108180Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word god / dog
	2015-05-15 16:27:55.512 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108210Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word part / trap
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108240Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word parts / strap
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108280Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word gut / tug
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108300Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word pot / top
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108330Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dam / mad
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108350Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word del / led
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108380Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deliver / reviled
	2015-05-15 16:27:55.513 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108410Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word devil / lived
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108430Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word raw / war
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108460Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dim / mid
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108480Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dog / god
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108510Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word don / nod
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108530Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word led / del
	2015-05-15 16:27:55.514 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108560Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word reviled / deliver
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108580Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word saw / was
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108610Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spot / tops
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108630Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word strap / parts
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108660Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ten / net
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108680Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tide / edit
	2015-05-15 16:27:55.515 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108710Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tops / spot
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108740Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word top / pot
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108770Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word trap / part
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108790Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tug / gut
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108810Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word war / raw
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108840Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ward / draw
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108860Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word was / saw
	2015-05-15 16:27:55.516 DataflowQueue.iOS[65527:914231] [2015-05-15T06:27:55.5108890Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word won / now
	2015-05-15 16:27:55.631 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.6318960Z][DBUG00007][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 1821 words
	2015-05-15 16:27:55.632 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.6319610Z][DBUG00011][DataflowQueue.iOS.AppDelegate.FinishedLaunching L42]: Checking for reversible words...
	2015-05-15 16:27:55.652 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.6525070Z][DBUG00011][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word live / evil
	2015-05-15 16:27:55.652 DataflowQueue.iOS[65527:914198] [2015-05-15T06:27:55.6525120Z][DBUG00011][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live
