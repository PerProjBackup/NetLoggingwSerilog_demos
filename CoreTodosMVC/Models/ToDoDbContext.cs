using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTodosMVC.Models
{
  public class ToDoDbContext : DbContext
  {
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base (options) { } //ef core interception not generally available as of recording time

    public DbSet<ToDoItem> ToDoItems { get; set; }
  }
}
