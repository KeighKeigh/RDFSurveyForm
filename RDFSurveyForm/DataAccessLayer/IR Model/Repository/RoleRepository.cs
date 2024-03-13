﻿using Microsoft.EntityFrameworkCore;
using RDFSurveyForm.Data;
using RDFSurveyForm.DATA_ACCESS_LAYER.HELPERS;
using RDFSurveyForm.DataAccessLayer.Interface;
using RDFSurveyForm.Dto.ModelDto.RoleDto;
using RDFSurveyForm.Model;

namespace RDFSurveyForm.DataAccessLayer.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly StoreContext _context;

        public RoleRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewRole(AddRoleDto role)
        {
            var addrole = new Role
            {
                RoleName = role.RoleName,
                CreatedAt = DateTime.Now,
                Permission = role.Permission,
                

            };
            await _context.CRole.AddAsync(addrole);
            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<bool> UpdatedPermission(UpdateRoleDto role)
        {
            var verification = role.Permission;
            var updatePermission = await _context.CRole.FirstOrDefaultAsync(x => x.Id == role.Id);
            var verify = updatePermission.Permission;
            if (verify.Count >= verification.Count) 
            {
                return true;
            }
            if (verify.Count <= verification.Count)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateRole(UpdateRoleDto role)
        {
            
            var updaterole = await _context.CRole.FirstOrDefaultAsync(r => r.Id == role.Id);

            
            
            if (updaterole != null)
            {
                updaterole.RoleName = role.RoleName;
                updaterole.Permission = role.Permission;
                updaterole.EditedBy = role.EditedBy;
                await _context.SaveChangesAsync();
                return true;
            }                   
            return false;
        }

        

        public async Task<bool> DeleteRole(int Id)
        {
            var deleterole = await _context.CRole.FirstOrDefaultAsync(info => info.Id == Id);
            if (deleterole != null)
            {
                _context.Remove(deleterole);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SetInActive(int Id)
        {
            var setinactive = await _context.CRole.FirstOrDefaultAsync(s => s.Id == Id);
            if (setinactive != null)
            {

                setinactive.IsActive = !setinactive.IsActive;
            }

            return true;
        }

        public async Task<PagedList<GetRoleDto>> CustomerListPagnation(UserParams userParams, bool? status, string search)
        {

            var result = _context.CRole.Select(x => new GetRoleDto
            {
                Id = x.Id,
                RoleName = x.RoleName,              
                CreatedAt = DateTime.Now,
                IsActive = x.IsActive,
                Permission = x.Permission,
                EditedBy = x.EditedBy,

            });

            if (status != null)
            {
                result = result.Where(x => x.IsActive == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => Convert.ToString(x.Id).ToLower().Contains(search.Trim().ToLower())
                || Convert.ToString(x.RoleName).ToLower().Contains(search.Trim().ToLower()));
            }


            return await PagedList<GetRoleDto>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);


        }
    }
}
