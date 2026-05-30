using System;
using System.Linq;
using AssignmentApp.DAL.Repositories.Sales;

class Program {
    static async System.Threading.Tasks.Task Main() {
        try {
            var repo = new OrderRepository();
            var all = await repo.GetAllAsync();
            var first = all.FirstOrDefault();
            Console.WriteLine(""MaHoaDon: "" + (first?.MaHoaDon ?? ""NULL""));
            Console.WriteLine(""TongTien: "" + first?.TongTien);
        } catch (Exception ex) {
            Console.WriteLine(""ERROR: "" + ex.Message);
        }
    }
}