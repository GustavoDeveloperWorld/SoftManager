using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftManager.Data;
using SoftManager.Models;
using SoftManager.Service;

namespace SoftManager.Controllers
{
    [Authorize]
    public class UserTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public UserTasksController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        // GET: UserTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tasks.Include(u => u.AssignedToUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.Tasks
                .Include(u => u.AssignedToUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // GET: UserTasks/Create
        public IActionResult Create()
        {
            var users = _context.Users.Select(u => new
            {
                Id = u.Id,
                FullName = u.FullName
            }).ToList();

            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FullName");
            return View();
        }

        // POST: UserTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,DueDate,AssignedToUserId")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                userTask.Id = Guid.NewGuid();
                _context.Add(userTask);
                await _context.SaveChangesAsync();

                // Lógica para enviar e-mail ao usuário após a criação da tarefa
                var user = await _context.Users.FindAsync(userTask.AssignedToUserId);
                if (user != null)
                {
                    string subject = "Nova Tarefa Atribuída";
                    string body = $"Você tem uma nova tarefa: {userTask.Message}. A data de vencimento é {userTask.DueDate}.";
                    await _emailService.SendEmailAsync(user.Email, subject, body);  // Envia o e-mail
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "Id", userTask.AssignedToUserId);
            return View(userTask);
        }
        // GET: UserTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.Tasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "Id", userTask.AssignedToUserId);
            return View(userTask);
        }
        // POST: UserTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Message,DueDate,AssignedToUserId")] UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "Id", userTask.AssignedToUserId);
            return View(userTask);
        }
        // GET: UserTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.Tasks
                .Include(u => u.AssignedToUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userTask = await _context.Tasks.FindAsync(id);
            if (userTask != null)
            {
                _context.Tasks.Remove(userTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
