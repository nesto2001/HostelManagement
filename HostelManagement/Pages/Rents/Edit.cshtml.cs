using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.BusinessObject;
using DataAccess;
using DataAccess.Repository;

namespace HostelManagement.Pages.Rents
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.HostelManagementContext _context;

        public EditModel(DataAccess.HostelManagementContext context)
        {
            _context = context;
        }


        [BindProperty]
        public Rent Rent { get; set; }
        [BindProperty]
        public int MonthRent { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Rent = await _context.Rents
                .Include(h => h.RoomMembers)
                .Include(h => h.Bills)
                .FirstOrDefaultAsync(m => m.RentId == id);
            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomId == Rent.RoomId);
            ViewData["RoomPrice"] = room.Price;
            if (Rent == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Rent.EndRentDate = Rent.EndRentDate.AddMonths(MonthRent);
            _context.Attach(Rent).State = EntityState.Modified;
            foreach (var item in Rent.RoomMembers)
            {
                item.EndRentDate = item.EndRentDate.AddMonths(MonthRent);
                _context.Attach(item).State = EntityState.Modified;
            }
            foreach (var item in Rent.Bills)
            {
                item.EndRentDate = item.EndRentDate.AddMonths(MonthRent);
                _context.Attach(item).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }

    }
}
