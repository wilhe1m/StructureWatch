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
* All times are UTC times.
* in the config host names do not end with / stuff breaks.

## Running internet facing.
This can be helpful t share amongst your corp members. This assumes you are gonna use linux and are sortof comfortable with the whole thing:

* create a new application on https://developers.eveonline.com/ pointed to hostname/api/SSO (note that https certs are free via lets encrypt so do that bad boy via certbot)
* edit app.config to include the new secrets
* create password file using htpasswd
* setup a reverse proxy, nginx is the easiest. Sample config file is included in Scripts
* setup a service that starts structurewatch at start.
* test to ensure it all works




## Thanks
This code owes a big debt to seraphx2 especially this code base: https://github.com/seraphx2/ESI.NET

## appsettings.json 

`````
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "EsiConfig": {
    "EsiUrl": "https://esi.evetech.net",
    "SSOUrl":"https://login.eveonline.com",
    "DataSource": "tranquility",
    "ClientId": "xxx",
    "SecretKey": "xxx",
    "CallbackUrl": "https://localhost:5001/api/SSO",
    "UserAgent": "Structure Watch",
    "AuthVersion": "v2"
  },
  "StructureWatchConfig":{
    "databaseConnectionString":"Data Source=StructureWatch.db"
  }
}
`````
## Change Log

### 0.0.6
* Update Main page
* Minor layout changes
* Add Linux Build to publish.bat script.
* Add sample nginx config
* Add sample linux service file

### 0.0.5
 * Add new svg Icon
 * Only allow poll every 20 minutes.
 * minor style changed
 * Add update buttons to characters page
 * Stop using https, expected config is either local or using a reverse proxy
 
### 0.0.4
 * fix issue with refresh tokens not being sent correctly
 * add link to structure details have to paste into note pad in game though
 * tweak op bar layout.
 * add and ignore test project


## TODO
* fix url for browser to use config.
* write install script and apache reverse proxy config
* dont load on the UI thread, so that we do a spinner or something on the buttons
* settings page?
* show git version someplace


