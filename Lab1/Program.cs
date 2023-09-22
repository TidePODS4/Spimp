namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите числа через пробел:");
            var input = Console.ReadLine();
            var list = input?.Split(' ').Select(int.Parse);

            Console.WriteLine("Введите n: ");
            var minCnt = int.Parse(Console.ReadLine()!);

            FilterList(list!, minCnt).ToList().ForEach(x => Console.Write(x + " "));        
        }

        public static IEnumerable<int> FilterList(IEnumerable<int> list, int minCnt) =>
            list
            .Where(x => list
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count())[x] > minCnt);
    }
}