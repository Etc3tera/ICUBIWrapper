using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Http;

namespace APITest.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [DllImport("ICUBIWrapperCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetWordBoundary(IntPtr unicodeBytes, IntPtr result);

        [HttpGet]
        [Route("break/{input_string}")]
        public IEnumerable<string> BreakWord(string input_string)
        {
            byte[] input_bytes = Encoding.UTF8.GetBytes(input_string);
            int[] output = new int[10000];

            IntPtr test = Marshal.AllocHGlobal(output.Length * 4);
            IntPtr input = Marshal.AllocHGlobal(input_bytes.Length * 2);
            Marshal.Copy(input_bytes, 0, input, input_bytes.Length);

            // Because C++ Native use NUL as end of string, so we add 4 zero bytes to ensure Native Library know that our string end
            
            Marshal.WriteByte(input, input_bytes.Length, 0);
            Marshal.WriteByte(input, input_bytes.Length + 1, 0);
            Marshal.WriteByte(input, input_bytes.Length + 2, 0);
            Marshal.WriteByte(input, input_bytes.Length + 3, 0);

            int len = GetWordBoundary(input, test);

            Marshal.Copy(test, output, 0, len);
            Marshal.FreeHGlobal(test);

            List<string> words = new List<string>();
            int end;
            for (int i = 0; i < len - 1; i++)
            {
                end = Math.Min(input_string.Length, output[i + 1]);
                words.Add(input_string.Substring(output[i], end - output[i]));
            }

            return words;
        }
    }
}
