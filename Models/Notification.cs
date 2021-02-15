using System;



namespace wilhe1m.StructureWatch.Models{

    public class Notification{


        public static readonly int NO_INT = 0;
        public static readonly string NO_STRING="";
        public static readonly DateTime NO_DATE = new DateTime(2020,01,01);
         public long NotificationId{get;set;} = NO_INT;
        public long Id{get;set;} = NO_INT;
        public long SenderId{get;set;} =NO_INT;
        public string SenderType{get;set;}=NO_STRING;
        public string Text{get;set;}=NO_STRING;

        public DateTime Timestamp{get;set;}=NO_DATE;
        public string Type{get; set;}=NO_STRING;

        public bool Hidden {get;set;}

        System.Collections.Generic.Dictionary<string,string> _parsed = null;
        public System.Collections.Generic.Dictionary<string,string> ParsedData{get{
           if(_parsed == null){
               _parsed = new System.Collections.Generic.Dictionary<string, string>();
               string[] items = this.Text.Split("\n");
               for(int i =0 ;i < items.Length;i++ ){
                   int  idx =  items[i].IndexOf(": ");
                   if(idx>0){
                    _parsed.Add(items[i].Substring(0,idx), items[i].Substring(idx+2));
                   }
               }
           }
           return _parsed;
        }}

    }
}