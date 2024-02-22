using Calcpad.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calcpad.web.Services
{
    public interface IParserService
    {
        public Task<string> CalculateAsync(string code, Settings settings, string[] fields = null);
        public Task<string> ParseAsync(string code, Settings settings, string[] fields = null);
        public Task<string> ParseWorksheetAsync(string code);
        public string GetInputTextAndValues(string code, out string[] fields);
        public string ReplaceInputFields(string html, string[] values);
    }

    public class ParserService : IParserService
    {
        private readonly ExpressionParser _parser;

        public ParserService()
        {
            _parser = new ExpressionParser();
        }

        public async Task<string> CalculateAsync(string code, Settings settings, string[] fields = null)
        {
            return await Task.Run(() => Parse(code, settings, fields, true));
        }

        public async Task<string> ParseAsync(string code, Settings settings, string[] fields = null)
        {
            return await Task.Run(() => Parse(code, settings, fields, false));
        }

        public async Task<string> ParseWorksheetAsync(string code)
        {
            string inputText = GetInputTextAndValues(code, out string[] fields);
            Settings settings = new();
            return await ParseAsync(inputText, settings, fields);
        }
        public string ReplaceInputFields(string html, string[] fields)
        {
            System.Text.StringBuilder sb = new();
            int i = 0, i1 = 0, i2 = 0, l = html.Length, n = fields.Length;
            while (i1 < l)
            {
                i1 = html.IndexOf("\"Var\"", i2);
                if (i1 < 0)
                {
                    sb.Append(html[i2..]);
                    i1 = l;
                }
                else
                {
                    i1 = html.IndexOf("value", i1);
                    i1 = html.IndexOf('"', i1);
                    sb.Append(html[i2..i1]);
                    if (i < n)
                    {
                        sb.Append(fields[i]);
                        i++;
                    }
                    i2 = html.IndexOf('"', i1 + 1) + 1;
                }
            }
            return sb.ToString();
        }

        private string Parse(string code, Settings settings, string[] fields, bool calculate)
        {
            if (string.IsNullOrWhiteSpace(code))
                return "0";

            if (settings != null)
                _parser.Settings = settings;

            if (fields != null)
                _parser.Parse(SetInputValues(code, fields) + '\n', calculate);
            else
                _parser.Parse(code + '\n', calculate);

            return _parser.HtmlResult;
        }

        public string GetInputTextAndValues(string code, out string[] fields)
        {
            if (code.Contains('\v'))
            {
                string[] s = code.Split('\v');
                fields = s[1].Split('\t');
                return s[0];
            }
            else
                fields = null;
            return code;
        }

        private static string SetInputValues(string code, string[] fields)
        {
            if (fields is null || fields.Length == 0)
                return code;

            var sb = new StringBuilder(10000);
            var values = new Queue<string>();
            for (int i = 0, len = fields.Length; i < len; ++i)
                values.Enqueue(fields[i]);
            var lines = code.AsSpan().EnumerateLines();
            foreach (var line in lines)
            {
                var len = sb.Length;
                var s = line.ToString();
                MacroParser.SetLineInputFields(s, sb, values, false);
                if (sb.Length == len)
                    sb.AppendLine(s);
                else
                    sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}
