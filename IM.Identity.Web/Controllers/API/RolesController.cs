using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IM.Identity.BI.Service;
using IM.Identity.BI.Service.Manager;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.Web.Controllers.API
{
    public class RolesController : ApiController
    {
        private readonly RolesService _rolesService = (RolesService)ServiceManager.GetServiceInstance(typeof(RolesService));

        public async Task<IEnumerable<IdentityRole>> Get()
        {
            var roles = await _rolesService.Get();

            return roles;
        }

        [ResponseType(typeof(IdentityRole))]
        public async Task<IHttpActionResult> Get(string id)
        {
            var role = await _rolesService.Get(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [ResponseType(typeof(IdentityRole))]
        public async Task<IHttpActionResult> Post(IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _rolesService.Insert(role);
            if (result.Succeeded)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return CreatedAtRoute("DefaultApi", new { id = role.Id }, role);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(string id, IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.Id)
            {
                return BadRequest();
            }

            var result = await _rolesService.Update(role);
            if (result.Succeeded)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
