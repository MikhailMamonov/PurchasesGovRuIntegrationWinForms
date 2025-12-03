using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace PurchasesGovRuIntegration.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<Dictionary<string,Dictionary<string,string>>> ReadContent44FZAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                throw new ApplicationException($"Аукцион с данным реестровым номером не найден. Ответ сервера : {response.ReasonPhrase}");
            }
            var dataAsString = await response.Content.ReadAsStringAsync();

            var rows = new Dictionary<string,Dictionary<string,string>>();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(dataAsString);

            var wrapper = document.DocumentNode.SelectSingleNode("//div[@class='wrapper']");

            var rowNodes = wrapper.SelectNodes(".//div[contains(@class, 'container')]"); 
            if (rowNodes != null)
            {
                foreach (var rowNode in rowNodes)
                {
                    var rowInfoNode = rowNode.SelectSingleNode(".//div[contains(@class, 'row')]");
                    var colNode = rowInfoNode?.SelectSingleNode(".//div[contains(@class, 'col')]");
                    var title = colNode?.SelectSingleNode(".//h2")?.InnerText ?? "Name not available";
                    var sections = colNode?.SelectNodes("./section[contains(@class, 'blockInfo__section')]");
                    var sectionDictionary = new Dictionary<string,string>();

                    if (sections != null && sections.Count > 0)
                    {
                        foreach (var section in sections)
                        {

                            var sectionTitles = section.SelectNodes("./span[contains(@class, 'section__title')]")?
                                .Select(cell => cell?.InnerText ?? "Name not available").ToArray();
                            if (sectionTitles != null && sectionTitles.Length > 0)
                            {
                                if (sectionTitles.Length > 1)
                                {
                                    var tableList = new List<string>();
                                    List<string> rowList = new List<string>();
                                    var tables = section.SelectNodes(".//table[contains(@class, 'blockInfo__table')]").ToArray();

                                    for (int i = 0;i < sectionTitles.Length;i++)
                                    {
                                        if (tables != null && tables.Length > 0)
                                        {
                                            var tableRows = tables[i].SelectNodes("./tr");
                                            
                                            for (int j = 0;j < tableRows.Count;j++)
                                            {
                                                if (j == 0)
                                                {
                                                    var colsArr = tableRows[j].SelectNodes(".//th[contains(@class, 'table__row-item')]").Select(row => row?.InnerText ?? "Name not available").ToArray();
                                                    rowList
                                                        .Add(String.Join(", ",colsArr));
                                                }
                                                else
                                                {
                                                    var colsArr = tableRows[j].SelectNodes(".//td[contains(@class, 'table__row-item')]").Select(row => row?.InnerText ?? "Name not available").ToArray();
                                                    rowList
                                                        .Add(String.Join(", ",colsArr));
                                                }
                                            }
                                        }

                                        tableList.Add(sectionTitles[i]);
                                        tableList.AddRange(rowList);
                                    }

                                    sectionDictionary.Add(sectionTitles.First(),String.Join("\n",tableList));
                                }
                                else
                                {
                                    var sectionInfo = section.SelectSingleNode(".//span[contains(@class, 'section__info')]")?.InnerText ?? string.Empty;
                                    sectionDictionary.Add(sectionTitles.First(),sectionInfo);
                                }
                            }
                            else
                            {
                                var sectionInfo = section.SelectSingleNode(".//span[contains(@class, 'section__info')]")?.InnerText ?? string.Empty;
                                sectionDictionary.Add(" ",sectionInfo);
                            }
                        }

                        rows.Add(title.Replace("\t","").Replace("\n"," ").Trim(),sectionDictionary);
                    }
                    else
                    {
                        var table = colNode?.SelectSingleNode(".//table[contains(@class, 'blockInfo__table')]");
                        if (table != null)
                        {
                            List<string> rowList = new List<string>();
                            var tableRows = table.SelectNodes(".//thead/tr").ToList();
                             
                            tableRows.AddRange(table.SelectNodes(".//tbody/tr").ToList());
                            tableRows.AddRange(table.SelectNodes(".//tfoot/ tr").ToList());

                            for (int j = 0;j < tableRows.Count;j++)
                            {
                                if (j == 0)
                                {
                                    var colsArr = tableRows[j].SelectNodes(".//th[contains(@class, 'tableBlock__col_header')]").Select(row => row?.InnerText ?? "Name not available").ToArray();
                                    rowList
                                        .Add(String.Join(", ",colsArr));
                                }
                                else
                                {
                                    var colsArr = tableRows[j].SelectNodes(".//td[contains(@class, 'tableBlock__col')]").Select(row => row?.InnerText ?? "Name not available").ToArray();
                                    rowList
                                        .Add(String.Join(", ",colsArr));
                                }
                            }

                            sectionDictionary.Add("",String.Join(Environment.NewLine,rowList));
                            rows.Add(title.Replace("\t","").Replace("\n"," ").Trim(),sectionDictionary);


                        }

                    }
                }
            }
            return rows;
        }

        public static async Task<Dictionary<string,Dictionary<string,string>>> ReadContent223FZAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                throw new ApplicationException($"Аукцион с данным реестровым номером не найден. Ответ сервера : {response.ReasonPhrase}");
            }
            var dataAsString = await response.Content.ReadAsStringAsync();

            var rows = new Dictionary<string,Dictionary<string,string>>();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(dataAsString);

            var wrapper = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'wrapper')]");

            var rowNodes = wrapper.SelectNodes("//div[contains(@class, 'card-common-content')]");
            if (rowNodes != null)
            {
                foreach (var cardNode in rowNodes)
                {

                    var sectionNode = cardNode.SelectSingleNode(".//section[contains(@class, 'common-text')]");
                    var containerNode = cardNode.SelectSingleNode(".//div[contains(@class, 'container')]");
                    var rowNode = cardNode.SelectSingleNode(".//div[contains(@class, 'row')]");

                    var columns = rowNode?.SelectNodes(".//div[contains(@class, 'col-9')]");
                    if (columns != null && columns.Count() > 0)
                    {
                        var title = columns.First().SelectSingleNode(".//div[contains(@class, 'common-text__caption')]")?.InnerText ?? "Name not available";
                        var columnDictionary = new Dictionary<string,string>();
                        foreach (var column in columns.Skip(1))
                        {
                            var columnTitle = column.SelectSingleNode(".//div[contains(@class, 'common-text__title')]")?.InnerText ?? string.Empty;
                            var columnnValue = column.SelectSingleNode(".//div[contains(@class, 'common-text__value')]")?.InnerText ?? string.Empty;
                            columnDictionary.Add(columnTitle,columnnValue);

                        }

                        rows.Add(title.Replace("\t","").Replace("\n"," ").Trim(),columnDictionary);
                    }
                }
            }

            return rows;
        }
    }
}
