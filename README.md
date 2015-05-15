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
	2015-05-15 16:06:32.770 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:32.7424120Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/files/6130/6130-0.txt
	Thread started: <Thread Pool> #7
	Thread started: <Thread Pool> #8
	Thread started: <Thread Pool> #9
	2015-05-15 16:06:38.035 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:38.0346100Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/cache/epub/1727/pg1727.txt
	2015-05-15 16:06:38.037 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:38.0371320Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:06:38.142 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:38.1420770Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	Thread started: <Thread Pool> #10
	2015-05-15 16:06:41.137 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:41.1375770Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoDownloadAsync L68]: Downloading http://www.gutenberg.org/cache/epub/1635/pg1635.txt
	2015-05-15 16:06:41.137 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:41.1375920Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:06:42.339 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:42.3392950Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoCreateWordList L87]: Creating word list...
	2015-05-15 16:06:43.729 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:43.7298960Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 14493 words
	2015-05-15 16:06:43.730 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:43.7299340Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	2015-05-15 16:06:43.734 DataflowQueue.iOS[64265:902153] Checking for reversible words...
	Checking for reversible words...
	2015-05-15 16:06:44.099 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0995720Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dog / god
	2015-05-15 16:06:44.099 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0995990Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word doom / mood
	2015-05-15 16:06:44.100 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996030Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word draw / ward
	2015-05-15 16:06:44.100 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996050Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word aera / area
	2015-05-15 16:06:44.100 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996080Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word area / aera
	2015-05-15 16:06:44.100 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996110Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live
	2015-05-15 16:06:44.100 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996130Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word god / dog
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996160Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deeps / speed
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996190Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deer / reed
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996220Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word del / led
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996240Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dew / wed
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996270Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dim / mid
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996300Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word keels / sleek
	2015-05-15 16:06:44.101 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996330Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word led / del
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996350Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mid / dim
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996380Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mood / doom
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996410Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word moor / room
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996430Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word net / ten
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996460Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word not / ton
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996500Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word now / won
	2015-05-15 16:06:44.102 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996520Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word port / trop
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996550Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word reed / deer
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996570Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word room / moor
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996680Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word saw / was
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996710Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word seat / taes
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996740Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word sleek / keels
	2015-05-15 16:06:44.103 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996770Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word speed / deeps
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996790Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spot / tops
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996820Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spots / stops
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996850Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word stops / spots
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996880Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word taes / seat
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996910Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ten / net
	2015-05-15 16:06:44.104 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996940Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tis / sit
	2015-05-15 16:06:44.105 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996960Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ton / not
	2015-05-15 16:06:44.105 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0996990Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tops / spot
	2015-05-15 16:06:44.105 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997010Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word trop / port
	2015-05-15 16:06:44.105 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997040Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ward / draw
	2015-05-15 16:06:44.106 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997070Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word was / saw
	2015-05-15 16:06:44.106 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997100Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word wed / dew
	2015-05-15 16:06:44.106 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997120Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word wolf / flow
	2015-05-15 16:06:44.106 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:44.0997150Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word won / now
	2015-05-15 16:06:47.174 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.1740460Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 8084 words
	2015-05-15 16:06:47.174 DataflowQueue.iOS[64265:902153] Checking for reversible words...
	Checking for reversible words...
	2015-05-15 16:06:47.175 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.1740770Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L110]: Filtering word list...
	2015-05-15 16:06:47.355 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3557690Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word draw / ward
	2015-05-15 16:06:47.356 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3557770Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word edit / tide
	2015-05-15 16:06:47.356 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3557790Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live
	2015-05-15 16:06:47.356 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3557800Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word god / dog
	2015-05-15 16:06:47.357 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3557810Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word gut / tug
	2015-05-15 16:06:47.357 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558130Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dam / mad
	2015-05-15 16:06:47.357 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558170Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word del / led
	2015-05-15 16:06:47.357 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558190Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word led / del
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558230Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word deliver / reviled
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558270Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word devil / lived
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558300Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dim / mid
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558320Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word live / evil
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558360Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word lived / devil
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558380Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word dog / god
	2015-05-15 16:06:47.358 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558410Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word don / nod
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558440Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mad / dam
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558460Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word mid / dim
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558490Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word net / ten
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558520Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word nod / don
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558540Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word now / won
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558570Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word parts / strap
	2015-05-15 16:06:47.359 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558600Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word part / trap
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558620Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word pot / top
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558650Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word raw / war
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558680Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word reviled / deliver
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558720Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word saw / was
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558740Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word spot / tops
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558770Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word strap / parts
	2015-05-15 16:06:47.360 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558800Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ten / net
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558820Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tide / edit
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558850Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word top / pot
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558880Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tops / spot
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558900Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word trap / part
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558930Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word tug / gut
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558960Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word war / raw
	2015-05-15 16:06:47.361 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3558980Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word ward / draw
	2015-05-15 16:06:47.362 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3559010Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word was / saw
	2015-05-15 16:06:47.362 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.3559030Z][DBUG00008][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word won / now
	2015-05-15 16:06:47.426 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.4259330Z][DBUG00009][DataflowQueue.ReversedWordFinder.DoFilterWordList L118]: Found 1821 words
	2015-05-15 16:06:47.426 DataflowQueue.iOS[64265:902186] Checking for reversible words...
	Checking for reversible words...
	2015-05-15 16:06:47.449 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.4499150Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word live / evil
	2015-05-15 16:06:47.450 DataflowQueue.iOS[64265:902147] [2015-05-15T06:06:47.4499240Z][DBUG00012][DataflowQueue.ReversedWordFinder.DoPrint L156]: Found reversed word evil / live