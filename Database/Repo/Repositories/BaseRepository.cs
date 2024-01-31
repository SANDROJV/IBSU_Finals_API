using Database.Data;
using Database.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repo.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly LibraryContext _context;

    public BaseRepository(LibraryContext context)
    {
        _context = context;
    }
    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Add(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}