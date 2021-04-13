/*
Structure watch functions

Wilhelm Beck 2021 wilhe1m@outlook.com

QOL upgrades to basic html buttons
*/

var sw={
    "pollOne":function(e){
        var x = new XMLHttpRequest();
        x.open("GET","/api/Notifications");
        x.send();

        return false;
    },
    "pollAll":function(e){
        var x = new XMLHttpRequest();
        x.open("GET","/api/Notifications/All");
        x.send();

        return false;
    }
};