using CoreTodosMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTodosMVC.Controllers
{
  [Authorize()]
  public class TodosViaApiController : Controller
  {
    public async Task<IActionResult> Index()
    {
      var todoList = await ApiHelper.GetListFromApiAsync<ToDoItem>(
        "/Todos", HttpContext);

      return View(todoList);
    }

    public async Task<IActionResult> IndexWithError()
    {
      var todoList = await ApiHelper.GetListFromApiAsync<ToDoItem>(
        "/Todos/Error", HttpContext);

      return View("Index", todoList);
    }

  }
}
