using Microsoft.EntityFrameworkCore;
using EfSqliteDemo.Models;

namespace EfSqliteDemo.Data;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=todo.db");
}