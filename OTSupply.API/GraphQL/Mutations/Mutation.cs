using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HotChocolate.Authorization;
using System.Security.Claims;

namespace OTSupply.API.GraphQL.Mutations
{
    public class OglasMutation
    {
        [Authorize(Policy = "Seller")]
        public async Task<Oglas> AddOglasAsync(
            [Service] OTSupplyDbContext context,
            ClaimsPrincipal claimsPrincipal,
            AddOglasDto input)
        {
            // Get user ID from claims - try multiple claim types
            var userIdString = claimsPrincipal.FindFirst("sub")?.Value ??
                              claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                              claimsPrincipal.FindFirst("nameid")?.Value ??
                              claimsPrincipal.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdString))
                throw new GraphQLException("User not authenticated");

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userIdString);

            if (prodavac == null)
                throw new GraphQLException("Prodavac profil nije pronađen");

            var oglas = new Oglas
            {
                Id = Guid.NewGuid(),
                Naslov = input.Naslov,
                Opis = input.Opis,
                Materijal = input.Materijal,
                Cena = input.Cena,
                Mesto = input.Mesto,
                Kategorija_Id = input.Kategorija_Id,
                Grad_Id = input.Grad_Id,
                Prodavac_Id = prodavac.Id
            };

            if (input.ImageUrls != null && input.ImageUrls.Count > 0)
            {
                foreach (var url in input.ImageUrls)
                {
                    oglas.ImageURLs.Add(new ImageUrl
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        OglasId = oglas.Id
                    });
                }
            }

            await context.Oglasi.AddAsync(oglas);
            await context.SaveChangesAsync();

            return oglas;
        }

        [Authorize(Policy = "Seller")]
        public async Task<Oglas> UpdateOglasAsync(
            [Service] OTSupplyDbContext context,
            ClaimsPrincipal claimsPrincipal,
            Guid oglasId,
            UpdateOglasDto input)
        {
            // Get user ID from claims - try multiple claim types
            var userIdString = claimsPrincipal.FindFirst("sub")?.Value ??
                              claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                              claimsPrincipal.FindFirst("nameid")?.Value ??
                              claimsPrincipal.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdString))
                throw new GraphQLException("User not authenticated");

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userIdString);

            if (prodavac == null)
                throw new GraphQLException("Prodavac profil nije pronađen");

            var oglas = await context.Oglasi
                .Include(o => o.ImageURLs)
                .FirstOrDefaultAsync(o => o.Id == oglasId);

            if (oglas == null)
                throw new GraphQLException("Oglas nije pronađen");

            if (oglas.Prodavac_Id != prodavac.Id)
                // Update fields directly on the tracked entity
                oglas.Naslov = input.Naslov;
            oglas.Opis = input.Opis;
            oglas.Materijal = input.Materijal;
            oglas.Cena = input.Cena;
            oglas.Mesto = input.Mesto;
            oglas.Kategorija_Id = input.Kategorija_Id;
            oglas.Grad_Id = input.Grad_Id;

            // Handle ImageUrls update
            if (input.ImageUrls != null)
            {
                // Remove existing images
                if (oglas.ImageURLs.Any())
                {
                    context.ImageUrls.RemoveRange(oglas.ImageURLs);
                }

                // Clear the collection and add new images
                oglas.ImageURLs.Clear();
                foreach (var url in input.ImageUrls.Where(u => !string.IsNullOrEmpty(u)))
                {
                    oglas.ImageURLs.Add(new ImageUrl
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        OglasId = oglas.Id
                    });
                }
            }

            // Don't call Update() since the entity is already tracked
            // Just save the changes
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the entity still exists
                var exists = await context.Oglasi.AnyAsync(o => o.Id == oglasId);
                if (!exists)
                {
                    throw new GraphQLException("Oglas je obrisan od strane drugog korisnika");
                }
                throw new GraphQLException("Oglas je izmenjen od strane drugog korisnika. Molimo pokušajte ponovo.");
            }

            // Return the updated entity with fresh data
            return await context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Prodavac)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad)
                .FirstAsync(o => o.Id == oglasId);
        }

        [Authorize(Policy = "Seller")]
        public async Task<bool> DeleteOglasAsync(
            [Service] OTSupplyDbContext context,
            ClaimsPrincipal claimsPrincipal,
            Guid oglasId)
        {
            // Get user ID from claims - try multiple claim types
            var userIdString = claimsPrincipal.FindFirst("sub")?.Value ??
                              claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                              claimsPrincipal.FindFirst("nameid")?.Value ??
                              claimsPrincipal.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdString))
                throw new GraphQLException("User not authenticated");

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userIdString);
            if (prodavac == null)
                throw new GraphQLException("Prodavac profil nije pronađen");

            var oglas = await context.Oglasi
                .Include(o => o.ImageURLs)
                .FirstOrDefaultAsync(o => o.Id == oglasId);

            if (oglas == null)
                throw new GraphQLException("Oglas nije pronađen");

            if (oglas.Prodavac_Id != prodavac.Id)
                throw new GraphQLException("Nemate pravo da obrišete ovaj oglas");

            context.ImageUrls.RemoveRange(oglas.ImageURLs);
            context.Oglasi.Remove(oglas);

            await context.SaveChangesAsync();
            return true;
        }
    }
}