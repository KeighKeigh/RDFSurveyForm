﻿using RDFSurveyForm.Data;
using RDFSurveyForm.DataAccessLayer.Interface;
using RDFSurveyForm.DataAccessLayer.IR_Setup.Interface;
using RDFSurveyForm.DataAccessLayer.IR_Setup.Repository;
using RDFSurveyForm.DataAccessLayer.Repository;


namespace RDFSurveyForm.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;

        public IUserRepository Customer { get; set; }
        public IRoleRepository CRole { get; set; }
        public IDepartmentRepository Department { get; set; }
        public IBranchRepository Branches { get; set; }
        public IGroupRepository Groups { get; set; }
        public ICategoryRepository Category { get; set;}

        public IGroupSurveyRepository GroupSurvey { get; }


        public UnitOfWork(StoreContext context)
        {
            _context = context;

            Customer = new UserRepository(_context);

            CRole = new RoleRepository(_context);

            Department = new DepartmentRepository(_context);

            Branches = new BranchRepository(_context);

            Groups = new GroupRepository(_context);

            Category = new CategoryRepository(_context);

            //Question = new QuestionsRepository(_context);

            GroupSurvey = new GroupSurveyRepository(_context);

            //SurveyScores = new SurveyScoreRepository(_context);

            //Generator = new GeneratorRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
