﻿using AutoMapper;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Calcpad.web.Helpers;
using Calcpad.web.Services;
using Calcpad.web.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calcpad.web.Controllers
{
    public class WorksheetController : Controller
    {
        private const string SettingsKey = "Settings";
        private const string ValuesKey = "Values";

        private readonly IWorksheetService _worksheetService;
        private readonly IParserService _parserService;
        private readonly IMapper _mapper;
        private readonly IOpenXmlService _openXmlService;

        public WorksheetController(IWorksheetService worksheetService, IParserService parserService, IOpenXmlService openXmlService, IMapper mapper)
        {
            _worksheetService = worksheetService;
            _parserService = parserService;
            _openXmlService = openXmlService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, string slug)
        {
            if (!id.HasValue)
                id = 1;

            Worksheet worksheet = await _worksheetService.GetByIdAsync(id.Value);
            if (worksheet == null)
                return NotFound();

            if (slug == null)
            {
                slug = TextHelper.SeoFriendly(worksheet.Title);
                return RedirectToRoute("slug", new { id, slug });
            }
            WorksheetViewModel model = _mapper.Map<WorksheetViewModel>(worksheet);
            if (TempData.TryGetValue(SettingsKey, out object value))
                model.Settings = JsonSerializer.Deserialize<ParserSettings>(value as string);
            else
                model.Settings = new ParserSettings();

            string key = ValuesKey + id.ToString();
            if (TempData.ContainsKey(key))
            {
                if (TempData[key] is string[] Values)
                    model.Html = _parserService.ReplaceInputFields(model.Html, Values);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(WorksheetCalculateModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                Worksheet worksheet = await _worksheetService.GetByIdAsync(inputModel.Id);
                WorksheetViewModel model = new(inputModel)
                {
                    Html = _parserService.ReplaceInputFields(worksheet.Html, inputModel.Var),
                };
                return View(model);
            }
            
            Core.Settings parserSettings = _mapper.Map<Core.Settings>(inputModel.Settings);
            if (inputModel.IsFree || User.Identity.IsAuthenticated)
            {
                string code = await _worksheetService.GetSourceCodeAsync(inputModel.Id);
                WorksheetViewModel viewModel = new(inputModel)
                {
                    Html = await _parserService.CalculateAsync(code, parserSettings, inputModel.Var),
                };
                TempData[SettingsKey] = JsonSerializer.Serialize(inputModel.Settings);
                TempData[ValuesKey + inputModel.Id.ToString()] = inputModel.Var;
                return View(viewModel);
            }
            else
            {
                Worksheet worksheet = await _worksheetService.GetByIdAsync(inputModel.Id);
                string code = _parserService.GetInputTextAndValues(worksheet.SourceCode, out string[] Var);
                WorksheetViewModel viewModel = new(inputModel)
                {
                    Html = await _parserService.CalculateAsync(code, parserSettings, Var)
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadAsync(string title, string html)
        {
            string url = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase}";
            FileContentResult file = await _openXmlService.GetDocxFileAsync(html, title + ".docx", url);
            return file;
        }
    }
}