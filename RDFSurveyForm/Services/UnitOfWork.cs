﻿using RDFSurveyForm.Data;
using RDFSurveyForm.DataAccessLayer.Interface;
using RDFSurveyForm.DataAccessLayer.IR_Setup.Interface;
using RDFSurveyForm.DataAccessLayer.IR_Setup.Repository;
using RDFSurveyForm.DataAccessLayer.Repository;
using RDFSurveyForm.Setup;

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

        public UnitOfWork(StoreContext context)
        {
            _context = context;

            Customer = new UserRepository(_context);

            CRole = new RoleRepository(_context);

            Department = new DepartmentRepository(_context);

            Branches = new BranchRepository(_context);

            Groups = new GroupRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}