using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trades.Data;
using Trades.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Trades.Pages.InputsClients
{
    public class CreateModel : PageModel
    {
        private readonly Trades.Data.ApplicationDbContext _context;

        public CreateModel(Trades.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Inputs Inputs { get; set; }
        [BindProperty]
        public OutPuts OutPuts { get; set; }
        public IEnumerable<OutPuts> Outs { get; set; }
        // increase Trades
        private async Task<string> FindTrades()
        {
            var allTrades = await _context.OutPuts.ToListAsync();

            Outs = allTrades;
            var maxTradeNumber = allTrades
                .Where(t => Regex.IsMatch(t.Trade, @"^trade\d+$")) // Filtra apenas os trades que correspondem ao padrão "trade" seguido de um número
                .Select(t => int.Parse(Regex.Match(t.Trade, @"\d+").Value)) // Extrai o número dos trades usando expressões regulares
                .DefaultIfEmpty() // Garante que haja pelo menos um elemento na sequência
                .Max(); // Encontra o número máximo
            var newTradeNumber = maxTradeNumber + 1;

            // Cria o novo trade
            var newTrade = "trade" + newTradeNumber;
            return newTrade;
        }
        private async Task<string> StartTrades ()
        {
            OutPuts.Trade = "trade" + 1;
            _context.Inputs.Add(Inputs);
            await _context.SaveChangesAsync();
            var idClient = await _context.Inputs.FirstOrDefaultAsync(i => i.Id == Inputs.Id);
            var IdTrade = await _context.OutPuts.FirstOrDefaultAsync(t => t.UserId == OutPuts.UserId);
            OutPuts.UserId = idClient.Id;
            _context.OutPuts.Add(OutPuts);
            await _context.SaveChangesAsync();

            return "ok";
        }
        private async Task<string> AdjustTrades()
        {

            var newTrade = await FindTrades();
            OutPuts.Trade = newTrade;
            _context.Inputs.Add(Inputs);
            await _context.SaveChangesAsync();
            var idClient = await _context.Inputs.FirstOrDefaultAsync(i => i.Id == Inputs.Id);
            var IdTrade = await _context.OutPuts.FirstOrDefaultAsync(t => t.UserId == OutPuts.UserId);
            OutPuts.UserId = idClient.Id;
            _context.OutPuts.Add(OutPuts);
            await _context.SaveChangesAsync();

            return "ok";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await FindTrades();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Inputs.Value > 1000000 && Inputs.ClientSector == "Private")
            {
                OutPuts.TradeType = "HIGHRISK";

                if (Outs == null)
                {
                    await StartTrades();
                }
               if (Outs != null)
                {
                    await AdjustTrades();
                }
            }
            else if (Inputs.Value > 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "MEDIUMRISK";
                if (Outs == null)
                {
                    await StartTrades();
                }
                if (Outs != null)
                {
                    await AdjustTrades();
                }
            }
            else if(Inputs.Value < 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "LOWRISK";
                if (Outs == null)
                {
                    await StartTrades();
                }
                if (Outs != null)
                {
                    await AdjustTrades();
                }
            }
            else if (Inputs.Value <= 1000000 && Inputs.ClientSector == "Private" 
                || Inputs.Value == 1000000 && Inputs.ClientSector == "Private" 
                || Inputs.Value == 1000000 && Inputs.ClientSector == "Public")
            {
                OutPuts.TradeType = "Uncaterogorized Risks";
                if (Outs == null)
                {
                    await StartTrades();
                }
                if (Outs != null)
                {
                    await AdjustTrades();
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
