﻿ using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDFSurveyForm.Data;
using RDFSurveyForm.DATA_ACCESS_LAYER.EXTENSIONS;
using RDFSurveyForm.DATA_ACCESS_LAYER.HELPERS;
using RDFSurveyForm.Dto.SetupDto.CategoryDto;
using RDFSurveyForm.Dto.SetupDto.GroupDto;
using RDFSurveyForm.Services;

namespace RDFSurveyForm.Controllers.SetupController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StoreContext _context;

        public CategoryController(IUnitOfWork unitOfWork,StoreContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoryDto category)
        {
            var categoryChecker = await _unitOfWork.Category.PercentageChecker(category);
            if (categoryChecker == false)
            {
                return BadRequest("% exceeded 100%!");
            }
            var categoryExist = await _unitOfWork.Category.CategoryAlreadyExist(category.CategoryName);
            if (categoryExist == false)
            {
                return BadRequest("Category Already Exist!");
            }
            
            await _unitOfWork.Category.AddCategory(category);
            return Ok("Category Added!");
        }

        [HttpPut("UpdateCategory/{Id:int}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto category, [FromRoute] int Id)
        {
            category.Id = Id;
            var categoryExist = await _unitOfWork.Category.CategoryAlreadyExist(category.CategoryName);
            var updateCategory = await _context.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (categoryExist == false && category.CategoryName != updateCategory.CategoryName)
            {
                return Ok("Category Name Already Exist!");


            }

            var categoryId = await _unitOfWork.Category.UdpateCategory(category);
            if (categoryId == false)
            {
                return BadRequest("Category Id Not Found!");
            }
            return Ok("Update Successfuly!");
        }

        [HttpGet("CategoryListPagnation")]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> CategoryListPagnation([FromQuery] UserParams userParams, bool? status, string search)
        {
            var posummary = await _unitOfWork.Category.CategoryPagnation(userParams, status, search);
            Response.AddPaginationHeader(posummary.CurrentPage, posummary.PageSize, posummary.TotalCount, posummary.TotalPages, posummary.HasNextPage, posummary.HasPreviousPage);

            var posummaryResult = new
            {
                posummary,
                posummary.CurrentPage,
                posummary.PageSize,
                posummary.TotalCount,
                posummary.TotalPages,
                posummary.HasNextPage,
                posummary.HasPreviousPage
            };
            return Ok(posummaryResult);
        }

        [HttpGet("Setinactive/{Id:int}")]
        public async Task<IActionResult> SetInactive([FromRoute] int Id)
        {
            var setinactive = await _unitOfWork.Category.SetInactive(Id);
            if (setinactive == false)
            {
                return BadRequest("Category does not exist!");
            }
            return Ok("Updated");
        }

        [HttpDelete("DeleteGroup/{Id:int}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int Id)
        {
            var deleteGroup = await _unitOfWork.Category.DeleteCategory(Id);
            if (deleteGroup == false)
            {
                return BadRequest("Category Does not Exist!");
            }
            return Ok("Group Deleted");
        }
    }
}