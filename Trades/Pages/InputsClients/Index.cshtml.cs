using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Trades.Data;
using Trades.Model;
using Trades.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using TradesS.SearchTrades;

namespace Trades.Pages.InputsClients
{
    public class IndexModel : PageModel
    {
        private readonly Trades.Data.ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public IndexModel(Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IList<Inputs> Inputs { get;set; }
        public IList<OutPuts> OutPuts { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<OutPuts> Outputs { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string sortOrder, int? pageIndex, int page = 1,
            string Trades = null, string categories = null)
        {
         
            Inputs = await _context.Inputs.ToListAsync();
            OutPuts = await _context.OutPuts.ToListAsync();
            IQueryable<OutPuts> usersIQ = _context.Outputs.AsQueryable();

       

            if (!string.IsNullOrEmpty(Trades))
            {
                usersIQ = usersIQ.Where(u => u.Trade.ToLower().Contains(Trades.ToLower()));
            }
            if (!string.IsNullOrEmpty(categories))
            {
                usersIQ = usersIQ.Where(u => u.TradeType.ToLower().Contains(categories.ToLower()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    usersIQ = usersIQ.OrderByDescending(s => s.ID);
                    break;
                case "Date":
                    usersIQ = usersIQ.OrderBy(s => s.ID);
                    break;
                case "date_desc":
                    usersIQ = usersIQ.OrderByDescending(s => s.UserId);
                    break;
                default:
                    usersIQ = usersIQ.OrderBy(s => s.UserId);
                    break;
            }
            var pageSize = _configuration.GetValue("PageSize", 4);
            Outputs = await PaginatedList<OutPuts>.CreateAsync(usersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int userId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var input = await _context.Inputs.FindAsync(id);
            var output = await _context.OutPuts.FindAsync(userId);

            if (Inputs != null)
            {
                _context.Inputs.Remove(input);
                await _context.SaveChangesAsync();
            }

            if (OutPuts != null)
            {
                _context.OutPuts.Remove(output);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");

        }
    }
}
