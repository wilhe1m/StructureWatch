using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using wilhe1m.StructureWatch.Models;
using wilhe1m.StructureWatch.Services;

namespace wilhe1m.Controllers{
     [ApiController]
    [Route("api/[controller]")]
    
 ///https://developers.eveonline.com/blog/article/sso-to-authenticated-calls
    public class NotificationsController:ControllerBase{

       public NotificationsController(){

       }
        
        [HttpGet]
      
        public async Task GetNotifications(){
            
        
            if(!String.IsNullOrEmpty(User.Identity.Name)){
                using(StructureContext context = new StructureContext()){
                    var character = context.Characters.Where(c=> c.CharacterName == User.Identity.Name).FirstOrDefault();
                    if(character !=null){
                        await Polling.UpdateOneCharacter(context, character);
                    }
                }
            }

            Response.Redirect("/Me/Notifications");

        
        }
        [HttpGet]
        [Route("All")]
        public async Task FullyUpdateData(){
            
        
            if(!String.IsNullOrEmpty(User.Identity.Name)){
                using(StructureContext context = new StructureContext()){
                   await Polling.FullyUpdateData(context);
                }
            }

            Response.Redirect("/Me/Notifications");

        
        }
        [HttpGet]
        [Route("Hide")]
      
        public async Task Hide(long id){
            
        
        
            using(StructureContext context = new StructureContext()){
                var  notif = context.Notifications.Find(id);
                if(notif !=null){
                    notif.Hidden = true;
                    context.Update(notif);
                    await context.SaveChangesAsync();
                }
            }
            

            Response.Redirect("/Me/Notifications");

        
        }

        
    }
}