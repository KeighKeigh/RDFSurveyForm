﻿using Microsoft.EntityFrameworkCore;
using RDFSurveyForm.Data;
using RDFSurveyForm.DATA_ACCESS_LAYER.HELPERS;
using RDFSurveyForm.DataAccessLayer.Interface;
using RDFSurveyForm.Dto.ModelDto.DepartmentDto;
using RDFSurveyForm.Dto.ModelDto.RoleDto;
using RDFSurveyForm.Model;

namespace RDFSurveyForm.DataAccessLayer.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly StoreContext _context;

        public DepartmentRepository(StoreContext context)
        {
            _context = context;
        }

        //public async Task<bool> EnteredNull(string none)
        //{
        //    var entered = await _context.Department.FirstOrDefaultAsync();

        //}
        public async Task<bool> ExistingDepartment(string department)
        {
            var existingDepartment = await _context.Department.FirstOrDefaultAsync(x => x.DepartmentName == department);
            if (existingDepartment == null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AddDepartment(AddDepartmentDto department)
        {
            var AddDept = new Department
            {
                DepartmentName = department.DepartmentName,
                CreatedAt = DateTime.Now,

            };
            await _context.Department.AddAsync(AddDept);
            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<bool> UpdateDepartment(UpdateDepartmentDto department)
        {
            var updatedept = await _context.Department.FirstOrDefaultAsync(d => d.Id == department.Id);
                if(updatedept != null)
            {
                updatedept.DepartmentName = department.DepartmentName;
                updatedept.EditedBy = department.EditedBy;
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }



        public async Task<bool> IsActiveValidation(int Id)
        {
            var userList = await _context.Customer.Where(x => x.IsActive == true).ToListAsync();
            var validation = userList.FirstOrDefault(x => x.DepartmentId == Id);
            if(validation == null)
            {
                return false;
            }
            return true;
                
        }
        

        public async Task<bool> SetIsActive(int Id)
        {
            var setIsactive = await _context.Department.FirstOrDefaultAsync(x => x.Id == Id);
            if (setIsactive != null)
            {
                setIsactive.IsActive = !setIsactive.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PagedList<GetDepartmentDto>> CustomerListPagnation(UserParams userParams, bool? status, string search)
        {

            var result = _context.Department.Select(x => new GetDepartmentDto
            {
                Id = x.Id,
                DepartmentName = x.DepartmentName,
                CreatedAt = DateTime.Now,
                IsActive = x.IsActive,
                EditedBy = x.EditedBy,

            });

            if (status != null)
            {
                result = result.Where(x => x.IsActive == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => Convert.ToString(x.Id).ToLower().Contains(search.Trim().ToLower())
                || Convert.ToString(x.DepartmentName).ToLower().Contains(search.Trim().ToLower()));
            }


            return await PagedList<GetDepartmentDto>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);


        }

    }
}
