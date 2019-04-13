using spy_detection.Data;

namespace spy_detection.Api
{
    public class SpyDetectionResult
    {
        public bool Contains { get; set; }
        public Spy Spy { get; set; }
    }
}
