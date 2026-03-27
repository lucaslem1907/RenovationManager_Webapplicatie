using Shared.DTO;

namespace Application.Interfaces
{
    public interface IExportExcelService
    {
        byte[] ProjectReport(ProjectExportDto project);
    }
}
