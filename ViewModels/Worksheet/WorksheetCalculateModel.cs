using Calcpad.web.Validation;

namespace Calcpad.web.ViewModels
{
    public class WorksheetCalculateModel : WorksheetBaseModel
    {
        [ValidNumbers(ErrorMessage = "Invalid numbers.")]
        public string[] Var { get; set; }
        public ParserSettings Settings { get; set; }
    }
}
