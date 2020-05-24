using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChurchManagerApi.Data;
using ChurchManagerApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChurchManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountChartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountChartsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

        }

        private Guid CurrentUserId {
            get {
                if (this.User.FindFirst("Id") == null)
                    throw new Exception("User Id cannot be null");

                return Guid.Parse(this.User.FindFirst("Id").Value);
            }
        }

        // GET: api/AccountCharts
        [HttpGet]
        public async Task<IActionResult> GetAccountChart()
        {
            var user = this.User;
            var result = await _context.AccountCharts.ToListAsync();

            return base.Ok(result);
        }

        [HttpGet("optionType/{type}")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<string,string>>>> GetAccountRegister(string type)
        {
            var user = this.User;
            var charts = await _context.AccountCharts.ToListAsync();

            switch (type)
            {
                case "register":
                    return base.Ok(charts.GetAccountRegister());                    
                case "account":
                    return base.Ok(charts.GetAccounts());
                case "fund":
                    return base.Ok(charts.GetFunds());
                default:
                    return base.BadRequest("invalid parameter");                    
            }


        }

        // GET: api/AccountCharts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountChart>> GetAccountChart(Guid id)
        {
            var accountChart = await _context.AccountCharts.FindAsync(id);

            if (accountChart == null)
            {
                return NotFound();
            }

            return accountChart;
        }

        // PUT: api/AccountCharts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountChart(Guid id, AccountChart accountChart)
        {
            if (id != accountChart.Id)
            {
                return BadRequest();
            }

            var isvalidCodeOrName = _context.AccountCharts
                .Where(i => i.Id != id)
                .Where(x => x.Code == accountChart.Code || x.Name == accountChart.Name).Count() > 0;

            if (isvalidCodeOrName)
                return BadRequest("Code or Name already in used.");

            if (ModelState.IsValid)
            {
                _context.Entry(accountChart).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountChartExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            return NoContent();
        }

        // POST: api/AccountCharts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<AccountChart>> PostAccountChart(AccountChart accountChart)
        {
            var exist = _context.AccountCharts.Where(x => x.Code ==  accountChart.Code || 
                x.Name == accountChart.Name).Count() > 0;

            if (exist)
                return BadRequest("Code or Name already in used.");
         

            accountChart.EnteredBy = this.CurrentUserId;
            accountChart.DateEntered = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.AccountCharts.Add(accountChart);
                await _context.SaveChangesAsync();
            }
            else
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            return CreatedAtAction("GetAccountChart", new { id = accountChart.Id }, accountChart);
        }

        // DELETE: api/AccountCharts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccountChart>> DeleteAccountChart(Guid id)
        {
            var accountChart = await _context.AccountCharts.FindAsync(id);
            if (accountChart == null)
            {
                return NotFound();
            }

            _context.AccountCharts.Remove(accountChart);
            await _context.SaveChangesAsync();

            return accountChart;
        }

        private bool AccountChartExists(Guid id)
        {
            return _context.AccountCharts.Any(e => e.Id == id);
        }
    }
}
