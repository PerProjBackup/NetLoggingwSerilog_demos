﻿using CoreFlogger;
using CoreTodosMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTodosMVC.Controllers
{
  public class TodosController : Controller
  {
    private ToDoDbContext _db;

    public TodosController(ToDoDbContext context)
    {
      _db = context;
    }

    [TrackUsage("ToDos", "Core MVC", "View Todo List")]
    public async Task<IActionResult> Index()
    {
      return View(await _db.ToDoItems.ToListAsync());
    }

    public async Task<IActionResult> IndexError()
    {
      return View("Index", await _db.ToDoItems
        .Include("badstuff").ToListAsync());
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
        throw new ArgumentNullException("Todo Id should not be null!");

      WebHelper.LogWebDiagnostic("TodDos", "Core MVC",
        "Just checking in....", HttpContext,
        new Dictionary<string, object> { { "Very", "Important" } });

      var toDoItem = _db.ToDoItems.Find(id);
      if (toDoItem == null)
        throw new Exception($"No To-Do found with item numer [{id}]!");
      return View(toDoItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TrackUsage("ToDos", "Core MVC", "Edit ToDo")]
    public ActionResult Edit([Bind("Id", "Item", "Completed")] ToDoItem toDoItem)
    {
      if (ModelState.IsValid)
      {
        _db.Entry(toDoItem).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(toDoItem);
    }
  }
}
