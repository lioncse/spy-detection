using spy_detection.Data;
using Swashbuckle.AspNetCore.Filters;

namespace spy_detection.Api.Examples
{
    public class GetSpyResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new[] { new Spy
            {
                Id = 1,
                Name = "James Bond",
                Code = new[] { 0, 0, 7 }
            } };
        }
    }
}
