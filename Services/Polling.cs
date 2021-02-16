using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Options;
using wilhe1m.StructureWatch.Models;
using System.Threading.Tasks;

namespace wilhe1m.StructureWatch.Services{

    /// This class pulls data tot he local cache from EVEAPIS
    public class Polling{

        

        public Polling(){
            
        }

        public static async Task FullyUpdateData(StructureContext context){

            //get all public structres
            //UpdatePublicStructureList(context);
            //GET ALL CHARs
            var all_chars = context.Characters.ToList<Models.Character>();

            foreach(Character c in all_chars){
                await UpdateOneCharacter(context, c);
            }

            //at this point pull 

        }
        public static void UpdatePublicStructureList(StructureContext context){
            //get items htat are not in the database already. add them witha first seen date.
            // List<long> publicStructures = await ESIClient.Universe.Structures();
            // List<long> known= context.Structures.Select(s=>s.Id).ToList();
            // List<long> newStructures = publicStructures.RemoveAll(known);
        }
        public static async Task UpdateOneCharacter(StructureContext context,  Character character){
            
            if(string.IsNullOrEmpty(character.AccessToken)){
                throw new Exception("User had no existing token.");
            }
            if(character.ExpiresAt < DateTime.UtcNow){
               var token = await EVESwagger.GetToken(Token.REFRESH, character.RefreshToken);
               character.ConsumeToken(token);
                
                context.Update(character);
                
                await context.SaveChangesAsync();
            }

            //get notifications
           
            List<Notification> notifications = await EVESwagger.GetNotificationsByCharacterId(character.CharacterID, character.AccessToken);
            //deduplciated
            notifications = notifications.Where(n=> context.Notifications.Select(x=> x.NotificationId).Contains(n.NotificationId) == false).ToList();
            context.Notifications.AddRange(notifications);
            context.SaveChanges();
          
        }

        public static void UpdateStructures(StructureContext context, List<int> structuresIdList){

        }
    }
}

