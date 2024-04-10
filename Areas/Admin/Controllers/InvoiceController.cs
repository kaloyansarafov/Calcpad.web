using Calcpad.web.Data.Services;
using Calcpad.web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Calcpad.web.Areas.Admin.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string sortOrder)
        {
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";

            var invoices = await _invoiceService.GetAllAsync();
            var invoicesViewModel = _mapper.Map<IEnumerable<InvoiceViewModel>>(invoices);

            switch (sortOrder)
            {
                case "id_desc":
                    invoicesViewModel = invoicesViewModel.OrderByDescending(i => i.Id);
                    break;
                case "Date":
                    invoicesViewModel = invoicesViewModel.OrderBy(i => i.CreatedDate);
                    break;
                case "date_desc":
                    invoicesViewModel = invoicesViewModel.OrderByDescending(i => i.CreatedDate);
                    break;
                case "Amount":
                    invoicesViewModel = invoicesViewModel.OrderBy(i => i.NetAmmount);
                    break;
                case "amount_desc":
                    invoicesViewModel = invoicesViewModel.OrderByDescending(i => i.NetAmmount);
                    break;
                default:
                    invoicesViewModel = invoicesViewModel.OrderBy(i => i.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var onePageOfInvoices = invoicesViewModel.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            ViewBag.OnePageOfInvoices = onePageOfInvoices;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageCount = Math.Ceiling((double)invoicesViewModel.Count() / pageSize);
            return View();
        }
    }
}