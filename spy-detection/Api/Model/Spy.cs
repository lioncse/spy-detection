using System.Linq;

namespace spy_detection.Data
{
    public class Spy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Code { get; set; }
    }

    public static class SpyExtension
    {
        public static bool IsCodeEqual(this Spy spy, int[] code)
        {
            return spy.Code.Length == code.Length && spy.Code.Select((c, i) => new { c, i }).All(v => v.c != code[v.i]);
        }
    }
}
