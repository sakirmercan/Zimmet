using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZimmetApp.Data;
using ZimmetApp.Models;

namespace ZimmetApp.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly AppDbContext _db;

        public AssignmentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Assignments
                .Include(a => a.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(a =>
                    (a.Person!.FullName).Contains(q) ||
                    a.ItemName.Contains(q) ||
                    (a.SerialOrLine ?? "").Contains(q));
            }

            var list = await query
                .OrderByDescending(a => a.AssignedDate)
                .ThenBy(a => a.Person!.FullName)
                .ToListAsync();

            return View(list);
        }

       
        [HttpGet]
        public async Task<IActionResult> Manage(int? id)
        {
            await LoadPersonsSelectList();

            if (TempData["SelectedPersonId"] is int selectedId)
                ViewBag.SelectedPersonId = selectedId;

            if (id == null)
                return View(new Assignment());

            var entity = await _db.Assignments.FindAsync(id.Value);
            if (entity == null) return NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(Assignment model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPersonsSelectList();
                return View(model);
            }

            if (model.Id == 0)
            {
                _db.Assignments.Add(model);
            }
            else
            {
                var existing = await _db.Assignments.FindAsync(model.Id);
                if (existing == null) return NotFound();

                existing.PersonId = model.PersonId;
                existing.ItemType = model.ItemType;
                existing.ItemName = model.ItemName;
                existing.SerialOrLine = model.SerialOrLine;
                existing.AssignedDate = model.AssignedDate;
                existing.Notes = model.Notes;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Assignments.FindAsync(id);
            if (entity == null) return NotFound();

            _db.Assignments.Remove(entity);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerson(string fullName, string? department, string? phone)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                TempData["PersonError"] = "Kişi adı zorunludur.";
                return RedirectToAction(nameof(Manage));
            }

            var person = new Person
            {
                FullName = fullName.Trim(),
                Department = string.IsNullOrWhiteSpace(department) ? null : department.Trim(),
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim()
            };

            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            TempData["SelectedPersonId"] = person.Id;

            return RedirectToAction(nameof(Manage));
        }

        private async Task LoadPersonsSelectList()
        {
            var persons = await _db.Persons
                .OrderBy(p => p.FullName)
                .Select(p => new { p.Id, p.FullName })
                .ToListAsync();

            ViewBag.Persons = new SelectList(persons, "Id", "FullName");
        }
    }
}
