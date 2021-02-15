# StructureWatch
A local netcore powered eve notification aggregator.

Basically, over multiple characters it is easy to miss a notification.

This tool aggregates them to ensure that you have offiline access to them. in particular, this tool allows a user to monitor for abandoning structures they have assets in. It also allow access to the raw notification feed in case that is somehow desired.

## Limitations.

The tool assumes that you are only sharing access to it to trusted people. 


## Tech stuff
Uses
* netcore 3.1
* sqlite

At startup will try to write a database in the install root.


## Code dumbness
All times are UTC times.

## Thanks
This code owes a big debt to seraphx2 especially this code base: https://github.com/seraphx2/ESI.NET
