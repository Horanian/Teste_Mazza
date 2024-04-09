using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trades.Data;
using Trades.Model;
using Trades.Pages.InputsClients;
using System.Text.RegularExpressions;

namespace Trades.Pages.InputsClients
{
    public class EditModel : PageModel
    {
        private readonly Trades.Data.ApplicationDbContext _context;

        public EditModel(Trades.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inputs Inputs { get; set; }
  
        [BindProperty]
        public OutPuts OutPuts { get; set; }

        private async Task<string> SetOutPuts(int id)
        {
            var outputsToUpdate = await _context.OutPuts.FindAsync(id);
            outputsToUpdate.TradeType = OutPuts.TradeType;

            if (await TryUpdateModelAsync<OutPuts>(
         outputsToUpdate,
         "OutPuts",
         o => o.Trade,
         o => o.TradeType,
         o => o.UserId
         ))
                await _context.SaveChangesAsync();
            return "ok";
        }

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
         
            OutPuts = await _context.OutPuts
       .Include(o => o.InputId).FirstOrDefaultAsync(m => m.ID == id);

            if (OutPuts == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Inputs, "Id", "ClientName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userId, int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Inputs.Value > 1000000 && Inputs.ClientSector == "Private")
            {
                OutPuts.TradeType = "HIGHRISK";
                await SetOutPuts(id);
            }
            else if (Inputs.Value > 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "MEDIUMRISK";
                await SetOutPuts(id);
            }
            else if (Inputs.Value < 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "LOWRISK";
                await SetOutPuts(id);
            }

            else if (Inputs.Value <= 1000000 && Inputs.ClientSector == "Private"
                || Inputs.Value == 1000000 && Inputs.ClientSector == "Private"
                || Inputs.Value == 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "Uncaterogorized Risks";
                await SetOutPuts(id);
            }

            _context.Attach(Inputs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InputsExists(Inputs.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InputsExists(int id)
        {
            return _context.Inputs.Any(e => e.Id == id);
        }

    }
}
