<a href='https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3BUM7DDBVPCU6'><img src='https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif' alt='' border='0'>



<h1>Introduction</h1>

Welcome to the Drive Cleanser Wiki. Here I will discuss just what Drive Cleanser is and what it is not.  How it came about and the future of the application.<br>
<br>
<br>
<h1>Details</h1>

Drive Cleanser started out as one of those projects I wanted to build just to see if I could.  One day I sold an old hard drive and wanted to securely wipe the contents to ensure nothing I had on the drive could be retrieved once I gave it to the new owner.  Typically when I want to wipe a drive I will use a boot CD that contains DBAN.  DBAN has always worked great in the past and has been my default "go-to" program for all my drive wiping needs.<br>
<br>
In this particular case though - I was working on a few things within windows and didn't want to reboot or spend too much time rebooting and waiting for the wipe process to complete. I decided to search the web and find some Windows applications that could do the job for me. This way I could wipe the drive and still work on whatever it was that had my attention at the time.<br>
<br>
I downloaded the first free application I found, fired it up and began the wipe process.  After about 30 minutes the application was only 2% in to the wipe process. This was unacceptable since the drive was only a 60GB SATA drive.<br>
<br>
<h2>C# To The Rescue</h2>
I fired up Visual Studio and quickly hammered out an application that would:<br>
<ol><li>Determine the total drive space (formatted usable drive space)<br>
</li><li>Loop and create 1GB files until the drive was filled<br>
</li><li>When done, format the drive.</li></ol>

This worked OK since it made sure that the drive had been completely filled with garbage and then formatted. I was fairly certain nothing could be retrieved from the drive when I finished.  If anything <b>maybe</b> someone could retrieve a list of files that the drive once contained.  I then began to wonder if accessing the disk directly and filling each and every sector would be any faster.<br>
<br>
I fired up google to research how I could accomplish this with C#.  I had a <i>general idea</i> how to go about it but needed some good material to fill in the gaps I had.<br>
<br>
<h2>PInvoke It</h2>
I came across lots of articles about accessing a disk directly but almost everyone I came across was snippets here and there. Nothing actually was a complete solution - only bits and pieces.<br>
<br>
After about 12 hours of playing with my code I had a solution I was sure would work.  I ran my code against a small drive, then downloaded a Drive Hex Editor to examine the disk and ensure it worked correctly.  The hex editor showed the entire drive - with the exception of the last 3KB - had been filled.  I knew what the problem was and quickly changed my code to fill the entire drive. I examined the drive again and oddly enough, only the first 2GB of drive was filled with my 0xFF bytes.<br>
<br>
I spent several hours trying to figure out what I had broke in my code to cause this.  As it turns out, I didn't break anything. The original hex editor I downloaded was showing me incorrect information about the drive. I then downloaded Active Disk Editor. This application worked a lot better and didn't lead me in the wrong direction when attempting to test my program.<br>
<br>
<h1>What Now</h1>
After a lot of time looking up Win32 API calls, I got the program to work correctly. It still needs work as far as allowing command line parameters to be passed in, exception handling and so on but it's still very much a work in progress.<br>
<br>
Drive Cleanser isn't meant to be better than everything else - by no means do I think it will out perform DBAN or many of the other applications out there. The reason I made it and released in to the open source world is so others can use it, improve upon it and maybe even integrate it in to their own applications.<br>
<br>
I am hoping that by providing this code to the C# community, it will help others realize just how easy it is to directly access drive contents and help them with other projects.