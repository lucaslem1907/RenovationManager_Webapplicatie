using Application.Interfaces;
using Shared.DTO;

namespace Application.Services
{
    public class GenerateProjectExcel
    {
        private IExportExcelService _export;
        private IProjectRepository _project;
        public GenerateProjectExcel(IExportExcelService export, IProjectRepository project)
        {
            _export = export;
            _project = project;
        }

        public async Task<byte[]> GenerateExcel(Guid projectId)
        {
            var project = await _project.GetByIdWithDetails(projectId);

            var projectExportDto = new ProjectExportDto
            {
                Name = project.Name,
                Address = project.Address,
                Budget = project.Budget ?? 0,
                Spent = project.Expenses.Sum(x => x.Amount),
                StartDate = project.StartDate,
                Rooms = project.Rooms
                   .Where(r => r.ProjectId == projectId)
                   .Select(r => new RoomDto { Name = r.Name, Status = r.Status })
                   .ToList(),
                Expenses = project.Expenses
                    .Where(r => r.ProjectId == projectId)
                    .Select(r => new ExpenseDto { Name = r.Name, Amount = r.Amount, Status = r.Status })
                    .ToList(),

            };

            return _export.ProjectReport(projectExportDto);

        }
    }
}
