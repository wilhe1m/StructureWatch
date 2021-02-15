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
    public class CharacterController:ControllerBase{

        StructureContext context ;       
        public CharacterController(StructureContext _context ){
            context = _context;
       }
        
     
        [HttpGet]
        [Route("Remove")]
      
        public async Task Remove(long id){


            var  character = context.Characters.Where(c => c.CharacterID ==id).FirstOrDefault();
            if(character !=null){
                context.Remove(character);
                await context.SaveChangesAsync();
            }
        
            

            Response.Redirect("/Me/Characters");

        
        }

        
    }
}