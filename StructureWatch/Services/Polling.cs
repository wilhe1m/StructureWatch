using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wilhe1m.StructureWatch.Models;

namespace wilhe1m.StructureWatch.Services
{
    /// This class pulls data tot he local cache from EVEAPIS
    public class Polling
    {
        public static async Task FullyUpdateData(StructureContext context)
        {
            //get all public structres
            //UpdatePublicStructureList(context);
            //GET ALL CHARs
            var all_chars = context.Characters.ToList();
            List<String> Failed = new List<string>();

            foreach (var c in all_chars) {
                try{
                await UpdateOneCharacter(context, c);
                }
                catch(Exception ex){
                   Failed.Add(c.CharacterName+":" +ex.Message);
                }
            }

            //at this point pull 
            if(Failed.Count>0){
                throw new Exception("Failed to poll all characters:"+ String.Join("\n", Failed));
            }
        }

        public static void UpdatePublicStructureList(StructureContext context)
        {
            //get items htat are not in the database already. add them witha first seen date.
            // List<long> publicStructures = await ESIClient.Universe.Structures();
            // List<long> known= context.Structures.Select(s=>s.Id).ToList();
            // List<long> newStructures = publicStructures.RemoveAll(known);
        }

        public static async Task UpdateOneCharacter(StructureContext context, Character character, bool force = false)
        {
            if (string.IsNullOrEmpty(character.AccessToken)) throw new Exception("User had no existing token.");
            if (character.ExpiresAt < DateTime.UtcNow)
            {
                var token = await EVESwagger.GetToken(Token.REFRESH, character.RefreshToken);
                character.ConsumeToken(token);

                context.Update(character);

                await context.SaveChangesAsync();
            }

            //get notifications
           // if (force || character.ExpiresAt < DateTime.UtcNow){
                var notifications =
                    await EVESwagger.GetNotificationsByCharacterId(character.CharacterID, character.AccessToken);
                //deduplciated
                //TODO this SHould work except it doesnt so we need to figure out why.
                // var Current = context.Notifications.Select(x => x.NotificationId).ToArray();
                notifications = notifications.Where(n => context.Notifications.Select(x => x.NotificationId).Contains(n.NotificationId) == false).ToList();
                context.Notifications.AddRange(notifications);
                context.SaveChanges();
           // }
        }

        public static void UpdateStructures(StructureContext context, List<int> structuresIdList)
        {
        }
    }
}