using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ProjectExportDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime StartDate {  get; set; }
        public decimal Budget { get; set; }
        public decimal Spent { get; set; }

        public int TotalRooms { get; set; }
        public int FinishedRooms { get; set; }

        public List<RoomDto> Rooms { get; set; }
        public List<ExpenseDto> Expenses { get; set; }
    }
}
