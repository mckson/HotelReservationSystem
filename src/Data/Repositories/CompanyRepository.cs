using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class CompanyRepository : IRepository<CompanyEntity>
    {
        private readonly HotelContext _db;

        public CompanyRepository(HotelContext context)
        {
            _db = context;
        }

        public IEnumerable<CompanyEntity> GetAll()
        {
            return _db.Companies
                .Include(c => c.Hotels);
        }

        public async Task<IEnumerable<CompanyEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public CompanyEntity Get(int id)
        {
            return _db.Companies
                .Where(company => company.Id == id)
                .Include(c => c.Hotels)
                .FirstOrDefault();
        }

        public async Task<CompanyEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public IEnumerable<CompanyEntity> Find(Func<CompanyEntity, bool> predicate)
        {
            return _db.Companies
                .Include(company => company.Hotels)
                .Where(predicate);
        }

        public async Task<IEnumerable<CompanyEntity>> FindAsync(Func<CompanyEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public void Create(CompanyEntity company)
        {
            _db.Companies.Add(company);
        }

        public async Task CreateAsync(CompanyEntity company)
        {
            await _db.Companies.AddAsync(company);
        }

        public void Update(CompanyEntity newCompany)
        {
            var oldCompany = _db.Companies.Find(newCompany.Id);

            if (oldCompany != null) oldCompany = newCompany;
        }

        public async Task UpdateAsync(CompanyEntity newCompany)
        {
            var oldCompany = await _db.Companies.FindAsync(newCompany.Id);

            if (oldCompany != null) oldCompany = newCompany;
        }

        public void Delete(int id)
        {
            var company = _db.Companies.Find(id);

            if (company != null)
                _db.Companies.Remove(company);
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _db.Companies.FindAsync(id);

            if (company != null)
                _db.Companies.Remove(company);
        }
    }
}
