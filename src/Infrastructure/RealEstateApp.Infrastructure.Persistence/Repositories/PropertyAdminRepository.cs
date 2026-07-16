using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class PropertyAdminRepository : IPropertyAdminRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyAdminRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Property>> GetByAgentIdIncludingSoldAsync(string agentId)
        => await _context.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.AgentId == agentId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task DeleteWithRelatedDataAsync(int propertyId)
    {

        var property = await _context.Properties.FindAsync(propertyId);
        if (property is not null)
            _context.Properties.Remove(property);
    }
}