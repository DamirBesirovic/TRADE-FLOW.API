using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;

namespace OTSupply.API.GraphQL.Queries
{
    public class OglasQuery
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Oglas> GetOglasi([Service] OTSupplyDbContext context)
        {
            return context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Prodavac)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad);
        }

        public async Task<Oglas?> GetOglasByIdAsync(
            [Service] OTSupplyDbContext context,
            Guid id)
        {
            return await context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Prodavac)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Oglas> GetOglasiByProdavac(
            [Service] OTSupplyDbContext context,
            Guid prodavacId)
        {
            return context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Prodavac)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad)
                .Where(o => o.Prodavac_Id == prodavacId);
        }
    }
}