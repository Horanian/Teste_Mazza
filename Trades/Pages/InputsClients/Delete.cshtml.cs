using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Trades.Data;
using Trades.Model;

namespace Trades.Pages.InputsClients
{
    public class DeleteModel : PageModel
    {
        private readonly Trades.Data.ApplicationDbContext _context;

        public DeleteModel(Trades.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inputs Inputs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Inputs = await _context.Inputs.FirstOrDefaultAsync(m => m.Id == id);

            if (Inputs == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Inputs = await _context.Inputs.FindAsync(id);

            if (Inputs != null)
            {
                _context.Inputs.Remove(Inputs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
