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
        private readonly DbSet<CompanyEntity> _companies;

        public CompanyRepository(HotelContext context)
        {
            _db = context;
            _companies = context.Companies;
        }

        public IEnumerable<CompanyEntity> GetAll() => _companies;

        // public async Task<IEnumerable<CompanyEntity>> GetAllAsync() => await Task.Run(GetAll);
        public CompanyEntity Get(int id) => _companies.FirstOrDefault(company => company.Id == id);

        public async Task<CompanyEntity> GetAsync(int id) => await Task.Run(() => Get(id));

        public IEnumerable<CompanyEntity> Find(Func<CompanyEntity, bool> predicate) => _companies.Where(predicate);

        public async Task<IEnumerable<CompanyEntity>> FindAsync(Func<CompanyEntity, bool> predicate) =>
            await Task.Run(() => Find(predicate));

        public void Create(CompanyEntity company)
        {
            _companies.Add(company);
            _db.SaveChanges();
        }

        public async Task CreateAsync(CompanyEntity company)
        {
            await _companies.AddAsync(company);
            await _db.SaveChangesAsync();
        }

        public void Update(CompanyEntity newCompany)
        {
            var oldCompany = _companies.Find(newCompany.Id);

            if (oldCompany == null)
                return;

            oldCompany = newCompany;
            _db.SaveChanges();
        }

        public async Task UpdateAsync(CompanyEntity newCompany)
        {
            var oldCompany = await _companies.FindAsync(newCompany.Id);

            if (oldCompany != null)
            {
                oldCompany = newCompany;
                await _db.SaveChangesAsync();
            }
        }

        public void Delete(int id)
        {
            var company = _companies.Find(id);

            if (company == null)
                return;

            _companies.Remove(company);
            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _companies.FindAsync(id);

            if (company != null)
            {
                _companies.Remove(company);
                await _db.SaveChangesAsync();
            }
        }
    }
}
