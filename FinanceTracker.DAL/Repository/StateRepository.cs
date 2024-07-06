using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _UserContext;
        public StateRepository(ApplicationDbContext userContext)
        {
            _UserContext = userContext;
        }
        async Task<List<StateModel>> IStateRepository.GetAllStates(int countryId)
        {
            var allStates=await _UserContext.States.Where(x=>x.CountryId == countryId).ToListAsync();;
            return allStates;
        }
    }
}