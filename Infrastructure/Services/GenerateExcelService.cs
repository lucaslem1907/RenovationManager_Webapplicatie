using Application.Interfaces;
using ClosedXML.Excel;
using Shared.DTO;
using System;


namespace Infrastructure.Services
{
    internal class GenerateExcelService : IExportExcelService
    {

        public byte[] ProjectReport(ProjectExportDto project)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Project Overview");
            var sheet2 = workbook.Worksheets.Add("Rooms");
            var sheet3 = workbook.Worksheets.Add("Expenses");

            var totalBudget = project.Budget;
            var Name = project.Name;
            var address = project.Address;
            var startDate = project.StartDate;
            var usedBudget = project.Spent;
            var percentage = (usedBudget / totalBudget) * 100;
            var rooms = project.Rooms;
            var expenses = project.Expenses;
            var status = usedBudget > totalBudget ? "Over budget" : "binnen budget";


            // Sheet 1 project Overview
            sheet.Cell(1, 1).Value = "Project naam";
            sheet.Cell(1, 2).Value = project.Name;

            sheet.Cell(2, 1).Value = "Totaal budget";
            sheet.Cell(2, 2).Value = totalBudget;

            sheet.Cell(3, 1).Value = "Gespendeerd budget";
            sheet.Cell(3, 2).Value = usedBudget;

            sheet.Cell(4, 1).Value = "% gebruikt";
            sheet.Cell(4, 2).Value = percentage;
            sheet.Cell(4, 2).Style.NumberFormat.Format = "0.00%";

            sheet.Cell(5, 1).Value = "Adres";
            sheet.Cell(5, 2).Value = address;

            sheet.Cell(6, 1).Value = "Startdatum";
            sheet.Cell(6, 2).Value = startDate;

            sheet.Cell(7, 1).Value = "Status";
            sheet.Cell(7, 2).Value = status;

            //kamer sheet
            sheet2.Cell(1, 1).Value = "Naam Kamer";
            sheet2.Cell(1, 2).Value = "Status Kamer";
            int roomstarter = 2;
            foreach (var item in rooms)
            {
                sheet2.Cell(roomstarter, 1).Value = item.Name;
                sheet2.Cell(roomstarter, 2).Value = item.Status.ToString();
                var layoutcell = sheet2.Cell(roomstarter, 2);
                switch (layoutcell.Value.ToString())
                {

                    case "not_started":
                        layoutcell.Style.Font.FontColor = XLColor.Red;
                        layoutcell.Value = "Niet gestart";
                        break;
                    case "in_progress":
                        layoutcell.Style.Font.FontColor = XLColor.Orange;
                        layoutcell.Value = "In behandeling";
                        break;
                    case "done":
                        layoutcell.Style.Font.FontColor = XLColor.Green;
                        layoutcell.Value = "Afgewerkt";
                        break;

                }

                roomstarter++;
            }

            //expenses sheet
            sheet3.Cell(1, 1).Value = "Naam Expense";
            sheet3.Cell(1, 2).Value = "Hoeveelheid";
            var expensestarter = 2;
            foreach (var item in expenses)
            {

                sheet3.Cell(expensestarter, 1).Value = item.Name;
                sheet3.Cell(expensestarter, 2).Value = item.Amount;
                sheet3.Cell(expensestarter, 3).Value = item.Status.ToString();

                sheet3.Cell(expensestarter, 2).Style.NumberFormat.Format = "€ #,##0.00";

                if (sheet3.Cell(expensestarter, 3).Value.ToString() == "unpaid") 
                {sheet3.Cell(expensestarter, 3).Style.Font.FontColor = XLColor.Red; }

                expensestarter++;
            }

            var statuscell = sheet.Cell(7, 2);
            if (status == "Over budget") { statuscell.Style.Font.FontColor = XLColor.Red; }
            else { statuscell.Style.Font.FontColor = XLColor.Green; }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
