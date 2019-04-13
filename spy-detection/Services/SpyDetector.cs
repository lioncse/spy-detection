namespace spy_detection.Services
{
    public class SpyDetector
    {
        public bool ContainsSpy(int[] message, int[] code)
        {
            // Assumption: A message is formed of an array of integers,
            // and the spy’s code name must appear, in that  order, for the spy to present
            // Example
            // Message Code Name Spy Contains spy?
            // [1,2,4,0,0,7,5] [0,0,7] James Bond true
            // [0,2,2,0,4,7,0] [0,0,7] James Bond true
            // [1,2,0,7,4,4,0] [0,0,7] James Bond false
            // [3,6,0,1,2,6,4] [3,1,4] Ethan Hunt true
            // [3,3,1,5,1,4,4] [3,1,4] Ethan Hunt true
            // [4,1,3,8,4,3,1] [3,1,4] Ethan Hunt false

            var contains = false;
            var current = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == code[current])
                {
                    current++;
                    if (current == code.Length)
                    {
                        contains = true;
                        break;
                    }
                }
            }

            return contains;
        }
    }
}
